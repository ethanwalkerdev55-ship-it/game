using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Surgical ProcessEndFail: when force-complete chain is active, skip vanilla fail-outgoing restart.
/// Does not replace the full method body (full transplant breaks game load).
/// </summary>
internal static class PatchProcessEndFailGuard
{
    internal static void Apply(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessEndFail");
        var body = method.Body;
        if (HasGuard(body))
        {
            Console.WriteLine("ProcessEndFail: fail-outgoing guard already present (skip)");
            return;
        }

        var chainField = type.Fields.First(f => f.Name == "bForceCompleteChain");
        var failOutgoingLog = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Ldstr && i.Operand as string == "Fail Outgoing Task : ");
        if (failOutgoingLog == null)
        {
            throw new System.InvalidOperationException("ProcessEndFail: Fail Outgoing Task log not found.");
        }

        var il = body.GetILProcessor();
        var cont = failOutgoingLog;
        var br = il.Create(OpCodes.Brfalse_S, cont);
        var ret = il.Create(OpCodes.Ret);
        var ldfld = il.Create(OpCodes.Ldfld, chainField);
        var ldarg = il.Create(OpCodes.Ldarg_0);

        il.InsertBefore(cont, ldarg);
        il.InsertBefore(cont, ldfld);
        il.InsertBefore(cont, br);
        il.InsertBefore(cont, ret);

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndFail: fail-outgoing guard injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasGuard(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldfld &&
            i.Operand is FieldReference fr &&
            fr.Name == "bForceCompleteChain" &&
            i.Previous?.OpCode == OpCodes.Ldarg_0);
}
