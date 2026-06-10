using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Ignore stale ProcessStartSucc(466) while instance tasks 463/667 are already active.
/// </summary>
internal static class PatchProcessStartSuccBlock
{
    internal static void Apply(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessStartSucc");
        var body = method.Body;
        if (HasBlock(body))
        {
            Console.WriteLine("ProcessStartSucc: stale 466 block already present (skip)");
            return;
        }

        var taskNumVar = FindTaskNumLocal(body);
        var insertAt = FindInsertBeforeGetTask(body);
        var isExist = type.Methods.First(m => m.Name == "IsExistTaskInActiveMission" && m.Parameters.Count == 1);

        var il = body.GetILProcessor();
        var cont = insertAt;
        var blockRet = il.Create(OpCodes.Ret);

        var block = new[]
        {
            il.Create(OpCodes.Ldloc, taskNumVar),
            il.Create(OpCodes.Ldc_I4, 466),
            il.Create(OpCodes.Bne_Un_S, cont),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldc_I4, 463),
            il.Create(OpCodes.Call, isExist),
            il.Create(OpCodes.Brtrue_S, blockRet),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldc_I4, 667),
            il.Create(OpCodes.Call, isExist),
            il.Create(OpCodes.Brtrue_S, blockRet),
            blockRet
        };

        foreach (var ins in block.Reverse())
        {
            il.InsertBefore(cont, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessStartSucc: stale 466 block injected (MaxStack={body.MaxStackSize}).");
    }

    private static VariableDefinition FindTaskNumLocal(MethodBody body)
    {
        for (var i = 0; i < body.Instructions.Count - 2; i++)
        {
            if (body.Instructions[i + 1].OpCode != OpCodes.Ldfld ||
                body.Instructions[i + 1].Operand is not FieldReference fr ||
                fr.Name != "iTaskNum")
            {
                continue;
            }

            var store = body.Instructions[i + 2];
            if (store.OpCode == OpCodes.Stloc && store.Operand is VariableDefinition v)
            {
                return v;
            }

            if (store.OpCode == OpCodes.Stloc_1 && body.Variables.Count > 1)
            {
                return body.Variables[1];
            }
        }

        throw new InvalidOperationException("ProcessStartSucc: iTaskNum local not found for block.");
    }

    private static Instruction FindInsertBeforeGetTask(MethodBody body)
    {
        var delChecker = body.Instructions.First(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "DelMissionTaskChecker");
        for (var scan = delChecker.Next; scan != null; scan = scan.Next)
        {
            if (scan.OpCode == OpCodes.Call &&
                scan.Operand is MethodReference mr &&
                mr.Name == "GetTask" &&
                mr.Parameters.Count == 1)
            {
                return scan;
            }
        }

        throw new InvalidOperationException("ProcessStartSucc: GetTask anchor not found for block.");
    }

    private static bool HasBlock(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "IsExistTaskInActiveMission" &&
            i.Previous?.OpCode == OpCodes.Ldc_I4 &&
            i.Previous.Operand is int n &&
            n == 463);
}
