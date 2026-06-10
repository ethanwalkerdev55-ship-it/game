using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class ChainIlSplice
{
    internal static void SpliceEmit(
        ILProcessor il,
        Instruction insertBefore,
        MethodDefinition host,
        TypeDefinition targetType,
        MethodDefinition donorEmit,
        VariableDefinition taskLocal)
    {
        var hostBody = host.Body;
        EnsureDonorVariables(hostBody, donorEmit.Body);

        var donorRet = donorEmit.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToHashSet();
        var map = new Dictionary<Instruction, Instruction>();
        foreach (var ins in donorEmit.Body.Instructions)
        {
            if (donorRet.Contains(ins))
            {
                continue;
            }

            map[ins] = Instruction.Create(OpCodes.Nop);
        }

        foreach (var ins in donorEmit.Body.Instructions)
        {
            if (donorRet.Contains(ins))
            {
                continue;
            }

            var c = map[ins];
            if (ins.OpCode == OpCodes.Ldarg_1)
            {
                c.OpCode = OpCodes.Ldloc;
                c.Operand = taskLocal;
            }
            else
            {
                c.OpCode = ins.OpCode;
                var operand = MapOperand(
                    ins, host, targetType, taskLocal, map, donorEmit.Body, insertBefore, donorRet);
                if (operand == null && ins.OpCode.OperandType != OperandType.InlineNone)
                {
                    throw new System.InvalidOperationException(
                        "ChainIlSplice: null operand for " + ins.OpCode + " in " + host.Name);
                }

                c.Operand = operand;
            }

            il.InsertBefore(insertBefore, c);
        }
    }

    private static void EnsureDonorVariables(MethodBody host, MethodBody donor)
    {
        while (host.Variables.Count < donor.Variables.Count)
        {
            var idx = host.Variables.Count;
            host.Variables.Add(new VariableDefinition(donor.Variables[idx].VariableType));
        }
    }

    private static object? MapOperand(
        Instruction ins,
        MethodDefinition host,
        TypeDefinition targetType,
        VariableDefinition taskLocal,
        Dictionary<Instruction, Instruction> map,
        MethodBody donorBody,
        Instruction insertBefore,
        HashSet<Instruction> donorRet) =>
        ins.Operand switch
        {
            null => null,
            Instruction branch => ResolveBranchTarget(branch, map, insertBefore, donorRet),
            Instruction[] branches => branches.Select(b => ResolveBranchTarget(b, map, insertBefore, donorRet)).ToArray(),
            VariableDefinition v => host.Body.Variables[v.Index],
            ParameterDefinition p => p.Index == 0 ? host.Body.ThisParameter! : taskLocal,
            FieldReference f => RemapField(f, targetType, host),
            MethodReference m => RemapMethod(m, targetType, host),
            TypeReference t => host.Module.ImportReference(t),
            string or int or long or float or double or byte or sbyte => ins.Operand,
            _ => throw new System.InvalidOperationException("ChainIlSplice: operand " + ins.Operand.GetType().Name)
        };

    private static Instruction ResolveBranchTarget(
        Instruction branch,
        Dictionary<Instruction, Instruction> map,
        Instruction insertBefore,
        HashSet<Instruction> donorRet) =>
        donorRet.Contains(branch) ? insertBefore : map[branch];

    private static FieldReference RemapField(FieldReference f, TypeDefinition targetType, MethodDefinition host)
    {
        if (f.DeclaringType.Name is "cnMissionManager" or "cnMissionManagerV2Donor")
        {
            return targetType.Fields.First(x => x.Name == f.Name);
        }

        return host.Module.ImportReference(f);
    }

    private static MethodReference RemapMethod(MethodReference m, TypeDefinition targetType, MethodDefinition host)
    {
        if (m.DeclaringType.Name is "cnMissionManager" or "cnMissionManagerV2Donor")
        {
            var local = targetType.Methods.FirstOrDefault(x =>
                x.Name == m.Name && x.Parameters.Count == m.Parameters.Count);
            if (local != null)
            {
                return local;
            }
        }

        return host.Module.ImportReference(m);
    }
}
