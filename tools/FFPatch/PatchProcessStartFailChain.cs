using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class PatchProcessStartFailChain
{
    internal static void Apply(TypeDefinition type, TypeDefinition donorType)
    {
        var donorEmit = donorType.Methods.First(m => m.Name == "EmitDocCompletePacket");
        var method = type.Methods.First(m => m.Name == "ProcessStartFail");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessStartFail: chain handler hook already present (skip)");
            return;
        }

        var delChecker = body.Instructions.First(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "DelMissionTaskChecker");
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
            throw new System.InvalidOperationException("ProcessStartFail: iTaskNum load not found.");
        }

        var taskVar = new VariableDefinition(type.Module.ImportReference(
            type.Module.Types.First(t => t.Name == "cnMissionNode")));
        body.Variables.Add(taskVar);
        var getTask = type.Methods.First(m => m.Name == "GetTask" && m.Parameters.Count == 1);
        var loopTemp = type.Fields.First(f => f.Name == "m_iloopTemp");
        var logMethod = IlLookup.FindLog(body);
        var toString = type.Module.ImportReference(typeof(int).GetMethod("ToString", System.Type.EmptyTypes)!);
        var il = body.GetILProcessor();
        var insertAt = delChecker.Next ?? delChecker;
        var emitSite = il.Create(OpCodes.Nop);

        var loadPacket = CloneInstruction(il, packetLoad);
        var loadTaskNum = il.Create(OpCodes.Ldfld, taskNumField);

        var gate = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, loopTemp),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ble_S, insertAt),
            loadPacket,
            loadTaskNum,
            il.Create(OpCodes.Call, getTask),
            il.Create(OpCodes.Stloc, taskVar),
            il.Create(OpCodes.Ldloc, taskVar),
            il.Create(OpCodes.Brfalse_S, insertAt),
            il.Create(OpCodes.Ldstr, "ForceCompleteV2: instance start fail complete task "),
            CloneInstruction(il, packetLoad),
            il.Create(OpCodes.Ldfld, taskNumField),
            il.Create(OpCodes.Call, toString),
            il.Create(OpCodes.Call, logMethod),
            il.Create(OpCodes.Br_S, emitSite)
        };

        foreach (var ins in gate.Reverse())
        {
            il.InsertBefore(insertAt, ins);
        }

        il.InsertBefore(insertAt, emitSite);
        ChainIlSplice.SpliceEmit(il, insertAt, method, type, donorEmit, taskVar);
        il.InsertBefore(insertAt, il.Create(OpCodes.Ret));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessStartFail: inline chain hook injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldstr &&
            i.Operand as string == "ForceCompleteV2: instance start fail complete task ");

    private static Instruction CloneInstruction(ILProcessor il, Instruction src) =>
        src.Operand switch
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
            _ => throw new System.InvalidOperationException("ProcessStartFail: unsupported operand clone.")
        };
}
