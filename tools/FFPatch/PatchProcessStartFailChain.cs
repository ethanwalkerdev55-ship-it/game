using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Surgical ProcessStartFail: instance start fail → complete with bError:true during chain.
/// </summary>
internal static class PatchProcessStartFailChain
{
    internal static void Apply(TypeDefinition type)
    {
        var handler = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteOnStartFail");
        if (handler == null)
        {
            throw new InvalidOperationException("ForceCompleteOnStartFail missing — build donor first.");
        }

        var method = type.Methods.First(m => m.Name == "ProcessStartFail");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessStartFail: chain handler hook already present (skip)");
            return;
        }

        var delChecker = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "DelMissionTaskChecker");
        if (delChecker == null)
        {
            throw new InvalidOperationException("ProcessStartFail: DelMissionTaskChecker call not found.");
        }

        FieldReference? taskNumField = null;
        Instruction? packetLoad = null;
        for (var ins = delChecker.Previous; ins != null; ins = ins.Previous)
        {
            if (ins.OpCode == OpCodes.Ldfld &&
                ins.Operand is FieldReference fr &&
                fr.Name == "iTaskNum")
            {
                taskNumField = fr;
                packetLoad = ins.Previous;
                break;
            }
        }

        if (taskNumField == null || packetLoad == null)
        {
            throw new InvalidOperationException("ProcessStartFail: iTaskNum load before DelMissionTaskChecker not found.");
        }

        var insertAt = delChecker.Next ?? delChecker;
        var il = body.GetILProcessor();

        var loadPacket = CloneInstruction(il, packetLoad);
        var loadTaskNum = il.Create(OpCodes.Ldfld, taskNumField);

        il.InsertBefore(insertAt, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(insertAt, loadPacket);
        il.InsertBefore(insertAt, loadTaskNum);
        il.InsertBefore(insertAt, il.Create(OpCodes.Call, handler));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessStartFail: chain handler hook injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "ForceCompleteOnStartFail");

    private static Instruction CloneInstruction(ILProcessor il, Instruction src)
    {
        return src.Operand switch
        {
            null => il.Create(src.OpCode),
            VariableDefinition v => il.Create(src.OpCode, v),
            FieldReference f => il.Create(src.OpCode, f),
            ParameterDefinition p => il.Create(src.OpCode, p),
            MethodReference m => il.Create(src.OpCode, m),
            TypeReference t => il.Create(src.OpCode, t),
            string s => il.Create(src.OpCode, s),
            int n => il.Create(src.OpCode, n),
            long l => il.Create(src.OpCode, l),
            float f => il.Create(src.OpCode, f),
            double d => il.Create(src.OpCode, d),
            Instruction target => il.Create(src.OpCode, target),
            _ => throw new InvalidOperationException("ProcessStartFail: unsupported operand clone.")
        };
    }
}
