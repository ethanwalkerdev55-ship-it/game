using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class PatchProcessEndSuccChain
{
    internal static void Apply(TypeDefinition type, TypeDefinition donorType)
    {
        _ = donorType;
        var method = type.Methods.First(m => m.Name == "ProcessEndSucc");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessEndSucc: chain handler hook already present (skip)");
            return;
        }

        var taskVar = FindTaskLocal(body);
        var outgoingField = body.Instructions
            .Select(i => i.Operand as FieldReference)
            .First(fr => fr?.Name == "m_iSUOutgoingTask");
        var anchor = body.Instructions.First(i =>
            i.OpCode == OpCodes.Ldfld &&
            (ReferenceEquals(i.Operand, outgoingField) ||
             (i.Operand is FieldReference fr && fr.Name == "m_iSUOutgoingTask")));
        while (anchor.Previous != null && anchor.Previous.OpCode != OpCodes.Callvirt)
        {
            anchor = anchor.Previous;
        }

        var loopTemp = type.Fields.First(f => f.Name == "m_iloopTemp");
        var nextVar = new VariableDefinition(type.Module.ImportReference(
            type.Module.Types.First(t => t.Name == "cnMissionNode")));
        body.Variables.Add(nextVar);
        var getNext = type.Methods.First(m => m.Name == "GetNextTaskNode" && m.Parameters.Count == 1);
        var getMe = IlLookup.FindGetMe(type.Module);
        var taskIdField = IlLookup.FindField(body, "m_iHTaskID");
        var rtc = type.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        var il = body.GetILProcessor();
        var cont = anchor;

        il.InsertBefore(cont, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ldfld, loopTemp));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ble_S, cont));
        il.InsertBefore(cont, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ldloc, taskVar));
        il.InsertBefore(cont, il.Create(OpCodes.Call, getNext));
        il.InsertBefore(cont, il.Create(OpCodes.Stloc, nextVar));
        il.InsertBefore(cont, il.Create(OpCodes.Ldloc, nextVar));
        il.InsertBefore(cont, il.Create(OpCodes.Brfalse_S, cont));
        il.InsertBefore(cont, il.Create(OpCodes.Ldloc, nextVar));
        il.InsertBefore(cont, il.Create(OpCodes.Callvirt, getMe));
        il.InsertBefore(cont, il.Create(OpCodes.Ldfld, taskIdField));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4_0));
        il.InsertBefore(cont, il.Create(OpCodes.Call, rtc));
        il.InsertBefore(cont, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(cont, il.Create(OpCodes.Ldfld, loopTemp));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4_1));
        il.InsertBefore(cont, il.Create(OpCodes.Add));
        il.InsertBefore(cont, il.Create(OpCodes.Stfld, loopTemp));
        il.InsertBefore(cont, il.Create(OpCodes.Ret));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndSucc: minimal chain hook injected (MaxStack={body.MaxStackSize}).");
    }

    private static VariableDefinition FindTaskLocal(MethodBody body)
    {
        var getTaskCall = body.Instructions.First(i =>
            i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.Name == "GetTask");
        var scan = getTaskCall.Next;
        for (var n = 0; n < 8 && scan != null; n++, scan = scan.Next)
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

        throw new InvalidOperationException("ProcessEndSucc: task local not found.");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "GetNextTaskNode") &&
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete" &&
            mr.Parameters.Count == 3);
}
