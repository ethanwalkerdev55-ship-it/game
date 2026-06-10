using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using DnlibOpCode = dnlib.DotNet.Emit.OpCode;
using dnlib.DotNet.Writer;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CecilMethod = Mono.Cecil.MethodDefinition;
using CecilType = Mono.Cecil.TypeDefinition;
using CecilInstruction = Mono.Cecil.Cil.Instruction;
using DnlibInstruction = dnlib.DotNet.Emit.Instruction;

namespace FFPatch;

/// <summary>
/// Write patched methods into the client PE using dnlib PreserveRids (avoids Cecil +4096 .text growth).
/// </summary>
internal static class DnlibPreserveWriter
{
    private static readonly Dictionary<short, DnlibOpCode> DnlibOpCodesByValue = BuildDnlibOpCodeMap();

    private static readonly string[] AlwaysSyncMethodNames =
    {
        "ForceCompleteCurrentTask",
        "RequestTaskComplete",
        "ProcessEndFail",
        "ProcessStartSucc",
    };

    public static bool Write(string outputPath, string clientBasePath, CecilType cecilMissionType)
    {
        outputPath = Path.GetFullPath(outputPath);
        clientBasePath = Path.GetFullPath(clientBasePath);
        var slotBytes = (int)new FileInfo(clientBasePath).Length;

        var mod = ModuleDefMD.Load(clientBasePath);
        var dnType = mod.Find("cnMissionManager", isReflectionName: false)
            ?? throw new InvalidOperationException("dnlib: cnMissionManager missing.");

        var syncMethods = cecilMissionType.Methods
            .Where(m => !m.HasGenericParameters && AlwaysSyncMethodNames.Any(n => n == m.Name))
            .ToList();

        foreach (var cecilMethod in syncMethods)
        {
            if (dnType.Methods.FirstOrDefault(m => m.Name == cecilMethod.Name && !m.HasGenericParameters) == null)
            {
                AddMethod(mod, dnType, cecilMethod);
                Console.WriteLine("dnlib-preserve: added method " + cecilMethod.Name);
            }
        }

        foreach (var cecilMethod in syncMethods)
        {
            var dnMethod = dnType.Methods.First(m => m.Name == cecilMethod.Name && !m.HasGenericParameters);
            TransplantBody(mod, cecilMethod, dnMethod);
        }

        var opts = new ModuleWriterOptions(mod)
        {
            MetadataOptions = new MetadataOptions
            {
                Flags = MetadataFlags.PreserveRids | MetadataFlags.KeepOldMaxStack
            }
        };

        var temp = outputPath + ".dnlib.tmp";
        mod.Write(temp, opts);
        File.Copy(temp, outputPath, true);
        File.Delete(temp);

        var outSize = (int)new FileInfo(outputPath).Length;
        if (outSize != slotBytes)
        {
            Console.WriteLine($"dnlib-preserve: output {outSize} != slot {slotBytes}, running dll-slot-fit...");
            if (!PatchDllSlotFit.Fit(outputPath, slotBytes, clientBasePath))
            {
                Console.Error.WriteLine($"dnlib-preserve: could not fit into client slot {slotBytes}.");
                return false;
            }
        }

        Console.WriteLine($"dnlib-preserve: OK {slotBytes} bytes (PreserveRids).");
        return true;
    }

    private static MethodDef AddMethod(ModuleDefMD mod, TypeDef dnType, CecilMethod cecilMethod)
    {
        var retType = ImportCecilTypeSig(mod, cecilMethod.ReturnType);
        var paramTypes = cecilMethod.Parameters.Select(p => ImportCecilTypeSig(mod, p.ParameterType)).ToArray();
        var sig = MethodSig.CreateInstance(retType, paramTypes);
        var method = new MethodDefUser(
            cecilMethod.Name,
            sig,
            (dnlib.DotNet.MethodAttributes)cecilMethod.Attributes);
        dnType.Methods.Add(method);
        return method;
    }

    private static void TransplantBody(ModuleDefMD mod, CecilMethod source, MethodDef target)
    {
        if (source.Body == null)
        {
            return;
        }

        var body = new CilBody
        {
            InitLocals = source.Body.InitLocals,
            MaxStack = (ushort)Math.Max(source.Body.MaxStackSize, (ushort)16),
            KeepOldMaxStack = true
        };

        foreach (var local in source.Body.Variables)
        {
            body.Variables.Add(new Local(ImportCecilTypeSig(mod, local.VariableType)));
        }

        var map = new Dictionary<CecilInstruction, DnlibInstruction>();
        foreach (var ins in source.Body.Instructions)
        {
            var clone = new DnlibInstruction(ToDnlibOpCode(ins.OpCode));
            body.Instructions.Add(clone);
            map[ins] = clone;
        }

        foreach (var ins in source.Body.Instructions)
        {
            var operand = ImportOperand(mod, target, body, ins, map);
            if (operand == null && ins.OpCode.OperandType != Mono.Cecil.Cil.OperandType.InlineNone)
            {
                var cecilOp = ins.Operand == null ? "null" : ins.Operand.GetType().Name + ":" + ins.Operand;
                throw new InvalidOperationException(
                    "dnlib: import null in " + target.Name + " opcode " + ins.OpCode + " cecil=" + cecilOp);
            }

            map[ins].Operand = operand;
        }

        body.SimplifyBranches();
        body.OptimizeBranches();
        target.Body = body;
        Console.WriteLine("dnlib-preserve: transplanted " + target.Name);
    }

    private static object? ImportOperand(
        ModuleDefMD mod,
        MethodDef target,
        CilBody body,
        CecilInstruction ins,
        Dictionary<CecilInstruction, DnlibInstruction> map) =>
        ins.Operand switch
        {
            null => null,
            string s => s,
            int i => i,
            long l => l,
            float f => f,
            double d => d,
            sbyte sb => sb,
            byte b => b,
            ParameterDefinition p => ResolveParameter(target, ins, p),
            VariableDefinition v => body.Variables[v.Index],
            FieldReference fr => ResolveField(mod, fr),
            MethodReference mr => ResolveMethod(mod, mr),
            TypeReference tr => mod.ResolveToken(tr.MetadataToken.ToInt32()),
            CecilInstruction branch => map[branch],
            CecilInstruction[] branches => branches.Select(b => map[b]).ToArray(),
            _ => throw new InvalidOperationException("dnlib: unsupported operand in " + target.Name)
        };

    private static IField ResolveField(ModuleDefMD mod, FieldReference fr)
    {
        var typeName = fr.DeclaringType?.Name ?? "cnMissionManager";
        var type = mod.Find(typeName, isReflectionName: false)
            ?? mod.Find(fr.DeclaringType?.FullName ?? typeName, isReflectionName: false);
        var field = type?.Fields.FirstOrDefault(f => f.Name == fr.Name);
        if (field != null)
        {
            return field;
        }

        foreach (var candidate in mod.GetTypes())
        {
            field = candidate.Fields.FirstOrDefault(f => f.Name == fr.Name);
            if (field != null)
            {
                return field;
            }
        }

        foreach (var memberRef in mod.GetMemberRefs())
        {
            if (memberRef.IsFieldRef &&
                memberRef.Name == fr.Name &&
                memberRef.DeclaringType?.Name == fr.DeclaringType?.Name)
            {
                return memberRef;
            }
        }

        var declRef = mod.GetTypeRefs().FirstOrDefault(t => t.Name == fr.DeclaringType?.Name);
        if (declRef != null)
        {
            return new MemberRefUser(mod, fr.Name, new FieldSig(ImportCecilFieldType(mod, fr)), declRef);
        }

        try
        {
            var resolved = (IField?)mod.ResolveToken(fr.MetadataToken.ToInt32());
            if (resolved != null)
            {
                return resolved;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("dnlib: cannot resolve field " + fr.FullName, ex);
        }

        throw new InvalidOperationException("dnlib: cannot resolve field " + fr.FullName);
    }

    private static TypeSig ImportCecilFieldType(ModuleDefMD mod, FieldReference fr) =>
        fr.FieldType.MetadataType switch
        {
            MetadataType.Int32 => mod.CorLibTypes.Int32,
            MetadataType.Boolean => mod.CorLibTypes.Boolean,
            MetadataType.Single => mod.CorLibTypes.Single,
            MetadataType.String => mod.CorLibTypes.String,
            MetadataType.Object => mod.CorLibTypes.Object,
            _ => mod.CorLibTypes.Int32
        };

    private static IMethod ResolveMethod(ModuleDefMD mod, MethodReference mr)
    {
        var typeName = mr.DeclaringType?.Name ?? "cnMissionManager";
        var type = mod.Find(typeName, isReflectionName: false)
            ?? mod.Find(mr.DeclaringType?.FullName ?? typeName, isReflectionName: false);
        if (type != null)
        {
            var method = type.Methods.FirstOrDefault(m =>
                m.Name == mr.Name &&
                m.MethodSig.Params.Count == mr.Parameters.Count);
            if (method != null)
            {
                return method;
            }
        }

        foreach (var candidate in mod.GetTypes())
        {
            var method = candidate.Methods.FirstOrDefault(m =>
                m.Name == mr.Name &&
                m.MethodSig.Params.Count == mr.Parameters.Count);
            if (method != null)
            {
                return method;
            }
        }

        foreach (var memberRef in mod.GetMemberRefs())
        {
            if (memberRef.IsMethodRef &&
                memberRef.Name == mr.Name &&
                memberRef.DeclaringType?.Name == mr.DeclaringType?.Name &&
                memberRef.MethodSig.Params.Count == mr.Parameters.Count)
            {
                return memberRef;
            }
        }

        if (mr.DeclaringType?.FullName is "System.Int32" && mr.Name == "ToString" && mr.Parameters.Count == 0)
        {
            return new MemberRefUser(
                mod,
                "ToString",
                MethodSig.CreateInstance(mod.CorLibTypes.String),
                mod.CorLibTypes.Int32.TypeDefOrRef);
        }

        var declRef = mod.GetTypeRefs().FirstOrDefault(t => t.Name == mr.DeclaringType?.Name);
        if (declRef != null)
        {
            var paramTypes = mr.Parameters.Select(_ => mod.CorLibTypes.Object).ToArray();
            var sig = mr.Parameters.Count == 0
                ? MethodSig.CreateInstance(ImportCecilMethodReturn(mod, mr))
                : MethodSig.CreateStatic(ImportCecilMethodReturn(mod, mr), paramTypes);
            return new MemberRefUser(mod, mr.Name, sig, declRef);
        }

        try
        {
            var resolved = (IMethod?)mod.ResolveToken(mr.MetadataToken.ToInt32());
            if (resolved != null)
            {
                return resolved;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("dnlib: cannot resolve method " + mr.FullName, ex);
        }

        throw new InvalidOperationException("dnlib: cannot resolve method " + mr.FullName);
    }

    private static TypeSig ImportCecilMethodReturn(ModuleDefMD mod, MethodReference mr) =>
        mr.ReturnType.MetadataType switch
        {
            MetadataType.Boolean => mod.CorLibTypes.Boolean,
            MetadataType.Void => mod.CorLibTypes.Void,
            MetadataType.String => mod.CorLibTypes.String,
            MetadataType.Int32 => mod.CorLibTypes.Int32,
            _ => mod.CorLibTypes.Object
        };

    private static object? ResolveParameter(MethodDef target, CecilInstruction ins, ParameterDefinition p)
    {
        if (ins.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineNone)
        {
            return null;
        }

        // Cecil ParameterDefinition.Index is already 0-based excluding implicit 'this'.
        return target.Parameters[p.Index];
    }

    private static TypeSig ImportCecilTypeSig(ModuleDefMD mod, TypeReference tr)
    {
        if (tr.IsArray)
        {
            return new SZArraySig(ImportCecilTypeSig(mod, tr.GetElementType()));
        }

        if (tr.IsByReference)
        {
            return new ByRefSig(ImportCecilTypeSig(mod, tr.GetElementType()));
        }

        if (tr.IsPointer)
        {
            return new PtrSig(ImportCecilTypeSig(mod, tr.GetElementType()));
        }

        foreach (var type in mod.GetTypes())
        {
            if (type.FullName == tr.FullName)
            {
                return type.ToTypeSig();
            }
        }

        foreach (var typeRef in mod.GetTypeRefs())
        {
            if (typeRef.FullName == tr.FullName)
            {
                return typeRef.ToTypeSig();
            }
        }

        return tr.MetadataType switch
        {
            MetadataType.Void => mod.CorLibTypes.Void,
            MetadataType.Boolean => mod.CorLibTypes.Boolean,
            MetadataType.Int32 => mod.CorLibTypes.Int32,
            MetadataType.Single => mod.CorLibTypes.Single,
            MetadataType.String => mod.CorLibTypes.String,
            MetadataType.Object => mod.CorLibTypes.Object,
            _ => throw new InvalidOperationException("dnlib: cannot import type " + tr.FullName)
        };
    }

    private static DnlibOpCode ToDnlibOpCode(Mono.Cecil.Cil.OpCode op) =>
        DnlibOpCodesByValue.TryGetValue(op.Value, out var mapped)
            ? mapped
            : throw new InvalidOperationException("dnlib: unknown opcode " + op);

    private static Dictionary<short, DnlibOpCode> BuildDnlibOpCodeMap()
    {
        var map = new Dictionary<short, DnlibOpCode>();
        foreach (var field in typeof(global::dnlib.DotNet.Emit.OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.GetValue(null) is DnlibOpCode op)
            {
                map[op.Value] = op;
            }
        }

        return map;
    }
}
