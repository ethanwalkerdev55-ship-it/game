using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Block vanilla fail-outgoing restart to 466 after instance defeat fail (ERR-001).
/// </summary>
internal static class PatchProcessEndFailLoopGuard
{
    internal static void Apply(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessEndFail");
        var body = method.Body;
        if (HasGuard(body))
        {
            Console.WriteLine("ProcessEndFail: loop guard already present (skip)");
            return;
        }

        var failOutgoingLog = body.Instructions.First(i =>
            i.OpCode == OpCodes.Ldstr && i.Operand as string == "Fail Outgoing Task : ");
        var taskVar = FindTaskLocal(body);
        var outgoingField = body.Instructions
            .Select(i => i.Operand as FieldReference)
            .First(fr => fr?.Name == "m_iFOutgoingTask");

        var il = body.GetILProcessor();
        var cont = failOutgoingLog;
        var skip = il.Create(OpCodes.Nop);
        var ret = il.Create(OpCodes.Ret);

        il.InsertBefore(cont, il.Create(OpCodes.Ldloc, taskVar));
        il.InsertBefore(cont, il.Create(OpCodes.Callvirt, IlLookup.FindGetMe(type.Module)));
        il.InsertBefore(cont, il.Create(OpCodes.Ldfld, outgoingField));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4, 466));
        il.InsertBefore(cont, il.Create(OpCodes.Bne_Un_S, cont));
        il.InsertBefore(cont, il.Create(OpCodes.Ldloc, taskVar));
        il.InsertBefore(cont, il.Create(OpCodes.Callvirt, IlLookup.FindGetMe(type.Module)));
        il.InsertBefore(cont, il.Create(OpCodes.Ldfld, IlLookup.FindField(body, "m_iHTaskID")));
        il.InsertBefore(cont, il.Create(OpCodes.Dup));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4, 463));
        il.InsertBefore(cont, il.Create(OpCodes.Beq_S, skip));
        il.InsertBefore(cont, il.Create(OpCodes.Ldc_I4, 667));
        il.InsertBefore(cont, il.Create(OpCodes.Bne_Un_S, cont));
        il.InsertBefore(cont, skip);
        il.InsertBefore(cont, ret);

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndFail: ERR-001 guard injected (MaxStack={body.MaxStackSize}).");
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

        throw new InvalidOperationException("ProcessEndFail: task local not found.");
    }

    private static bool HasGuard(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldc_I4 && i.Operand is int v && v == 463 &&
            i.Next?.OpCode == OpCodes.Beq_S);
}
