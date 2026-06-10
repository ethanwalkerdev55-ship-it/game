using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Mark instance-chain advance in ProcessEndSucc so stale ProcessStartSucc(466) can be ignored.
/// </summary>
internal static class PatchProcessEndSuccInstanceChainFlag
{
    internal static void Apply(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessEndSucc");
        var body = method.Body;
        if (HasFlag(body))
        {
            Console.WriteLine("ProcessEndSucc: instance chain flag already present (skip)");
            return;
        }

        var anchor = FindOutgoingRequestTaskStart(body);
        var task2Var = FindOutgoingTask2Local(body, anchor);
        var loopTemp = type.Fields.First(f => f.Name == "m_iloopTemp");
        var getMe = IlLookup.FindGetMe(type.Module);
        var requireInstance = IlLookup.FindField(body, "m_iRequireInstanceID");

        var il = body.GetILProcessor();
        var cont = anchor;
        var skip = il.Create(OpCodes.Nop);

        il.InsertBefore(cont, il.Create(OpCodes.Ldloc, task2Var));
        il.InsertBefore(cont, il.Create(OpCodes.Callvirt, getMe));
        il.InsertBefore(cont, il.Create(OpCodes.Ldfld, requireInstance));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ble_S, skip));
        il.InsertBefore(cont, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4_1));
        il.InsertBefore(cont, il.Create(OpCodes.Stfld, loopTemp));
        il.InsertBefore(cont, skip);

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndSucc: instance chain flag injected (MaxStack={body.MaxStackSize}).");
    }

    private static Instruction FindOutgoingRequestTaskStart(MethodBody body)
    {
        for (var i = 0; i < body.Instructions.Count - 2; i++)
        {
            var ins = body.Instructions[i];
            if (ins.OpCode != OpCodes.Call || ins.Operand is not MethodReference mr || mr.Name != "RequestTaskStart")
            {
                continue;
            }

            var prev = body.Instructions[i - 1];
            if (prev.OpCode == OpCodes.Ldc_I4_0 &&
                body.Instructions[i - 2].OpCode == OpCodes.Ldfld &&
                body.Instructions[i - 2].Operand is FieldReference fr &&
                fr.Name == "m_iHTaskID")
            {
                return body.Instructions[i - 3];
            }
        }

        throw new System.InvalidOperationException("ProcessEndSucc: outgoing RequestTaskStart anchor not found.");
    }

    private static VariableDefinition FindOutgoingTask2Local(MethodBody body, Instruction anchor)
    {
        for (var scan = anchor.Previous; scan != null; scan = scan.Previous)
        {
            if (scan.OpCode == OpCodes.Stloc && scan.Operand is VariableDefinition v)
            {
                return v;
            }

            if (scan.OpCode == OpCodes.Stloc_0 || scan.OpCode == OpCodes.Stloc_1 ||
                scan.OpCode == OpCodes.Stloc_2 || scan.OpCode == OpCodes.Stloc_3)
            {
                return body.Variables[scan.OpCode.Value - OpCodes.Stloc_0.Value];
            }
        }

        throw new System.InvalidOperationException("ProcessEndSucc: outgoing task2 local not found.");
    }

    private static bool HasFlag(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Stfld &&
            i.Operand is FieldReference fr &&
            fr.Name == "m_iloopTemp" &&
            i.Previous?.OpCode == OpCodes.Ldc_I4_1 &&
            i.Previous.Previous?.OpCode == OpCodes.Ldarg_0);
}
