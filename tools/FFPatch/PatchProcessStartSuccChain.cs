using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Surgical ProcessStartSucc: advance force-complete chain after task start (e.g. 463 instance task).
/// </summary>
internal static class PatchProcessStartSuccChain
{
    internal static void Apply(TypeDefinition type)
    {
        var handler = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteOnStartSucc");
        if (handler == null)
        {
            throw new InvalidOperationException("ForceCompleteOnStartSucc missing — build donor first.");
        }

        var method = type.Methods.First(m => m.Name == "ProcessStartSucc");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessStartSucc: chain handler hook already present (skip)");
            return;
        }

        VariableDefinition? taskNumVar = null;
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
                taskNumVar = v;
                break;
            }

            if (store.OpCode == OpCodes.Stloc_1 && body.Variables.Count > 1)
            {
                taskNumVar = body.Variables[1];
                break;
            }
        }

        if (taskNumVar == null)
        {
            throw new InvalidOperationException("ProcessStartSucc: iTaskNum local not found.");
        }

        var anchor = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Ldstr && i.Operand as string == "ProcessStartSucc ");
        if (anchor == null)
        {
            throw new InvalidOperationException("ProcessStartSucc: anchor log not found.");
        }

        anchor = anchor.Next ?? anchor;
        while (anchor != null && anchor.OpCode == OpCodes.Call)
        {
            anchor = anchor.Next;
        }

        var il = body.GetILProcessor();
        il.InsertBefore(anchor, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(anchor, il.Create(OpCodes.Ldloc, taskNumVar));
        il.InsertBefore(anchor, il.Create(OpCodes.Call, handler));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessStartSucc: chain handler hook injected after anchor (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "ForceCompleteOnStartSucc");
}
