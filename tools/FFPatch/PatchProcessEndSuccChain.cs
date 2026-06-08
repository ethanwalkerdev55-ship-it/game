using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Surgical ProcessEndSucc: advance force-complete chain instead of vanilla RequestTaskStart.
/// </summary>
internal static class PatchProcessEndSuccChain
{
    internal static void Apply(TypeDefinition type)
    {
        var handler = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteOnEndSucc");
        if (handler == null)
        {
            throw new InvalidOperationException("ForceCompleteOnEndSucc missing — build donor first.");
        }

        var method = type.Methods.First(m => m.Name == "ProcessEndSucc");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessEndSucc: chain handler hook already present (skip)");
            return;
        }

        var getTaskCall = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "GetTask");
        if (getTaskCall == null)
        {
            throw new InvalidOperationException("ProcessEndSucc: GetTask call not found.");
        }

        VariableDefinition? taskVar = null;
        var scan = getTaskCall.Next;
        for (var n = 0; n < 8 && scan != null; n++, scan = scan.Next)
        {
            if (scan.OpCode == OpCodes.Stloc && scan.Operand is VariableDefinition v)
            {
                taskVar = v;
                break;
            }

            if (scan.OpCode == OpCodes.Stloc_0 || scan.OpCode == OpCodes.Stloc_1 ||
                scan.OpCode == OpCodes.Stloc_2 || scan.OpCode == OpCodes.Stloc_3)
            {
                var idx = scan.OpCode.Value - OpCodes.Stloc_0.Value;
                if (idx < body.Variables.Count)
                {
                    taskVar = body.Variables[idx];
                    break;
                }
            }
        }

        if (taskVar == null)
        {
            throw new InvalidOperationException("ProcessEndSucc: task local not found.");
        }

        var outgoingField = body.Instructions
            .Select(i => i.Operand as FieldReference)
            .FirstOrDefault(fr => fr?.Name == "m_iSUOutgoingTask");
        if (outgoingField == null)
        {
            throw new InvalidOperationException("ProcessEndSucc: m_iSUOutgoingTask field ref not found.");
        }

        var anchor = body.Instructions.First(i =>
            i.OpCode == OpCodes.Ldfld && ReferenceEquals(i.Operand, outgoingField));
        if (anchor == null)
        {
            anchor = body.Instructions.First(i =>
                i.OpCode == OpCodes.Ldfld &&
                i.Operand is FieldReference fr &&
                fr.Name == "m_iSUOutgoingTask");
        }

        if (anchor == null)
        {
            throw new InvalidOperationException("ProcessEndSucc: outgoing task anchor not found.");
        }

        while (anchor.Previous != null && anchor.Previous.OpCode != OpCodes.Callvirt)
        {
            anchor = anchor.Previous;
        }

        var il = body.GetILProcessor();
        var cont = anchor;
        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldloc, taskVar),
            il.Create(OpCodes.Call, handler),
            il.Create(OpCodes.Brfalse_S, cont),
            il.Create(OpCodes.Ret)
        };

        foreach (var ins in block.Reverse())
        {
            il.InsertBefore(cont, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndSucc: chain handler hook injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "ForceCompleteOnEndSucc");
}
