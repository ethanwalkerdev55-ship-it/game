using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class PatchProcessEndFailChain
{
    internal static void Apply(TypeDefinition type, TypeDefinition donorType)
    {
        var donorEmit = donorType.Methods.First(m => m.Name == "EmitDocCompletePacket");
        var method = type.Methods.First(m => m.Name == "ProcessEndFail");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessEndFail: chain handler hook already present (skip)");
            return;
        }

        var taskVar = FindTaskLocal(body);
        var cont = FindInsertPoint(body, taskVar);
        var errorField = body.Instructions
            .First(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference fr && fr.Name == "iErrorCode")
            .Operand as FieldReference;
        var loopTemp = type.Fields.First(f => f.Name == "m_iloopTemp");
        var il = body.GetILProcessor();

        var emitSite = il.Create(OpCodes.Nop);
        var clearRet = il.Create(OpCodes.Ret);

        var gate = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, loopTemp),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ble_S, cont),
            il.Create(OpCodes.Ldloc_0),
            il.Create(OpCodes.Ldfld, errorField!),
            il.Create(OpCodes.Ldc_I4, 13),
            il.Create(OpCodes.Beq_S, clearRet),
            il.Create(OpCodes.Ldloc_0),
            il.Create(OpCodes.Ldfld, errorField!),
            il.Create(OpCodes.Ldc_I4_1),
            il.Create(OpCodes.Beq_S, emitSite),
            il.Create(OpCodes.Ldloc_0),
            il.Create(OpCodes.Ldfld, errorField!),
            il.Create(OpCodes.Ldc_I4, 11),
            il.Create(OpCodes.Beq_S, emitSite),
            il.Create(OpCodes.Ldloc_0),
            il.Create(OpCodes.Ldfld, errorField!),
            il.Create(OpCodes.Ldc_I4, 12),
            il.Create(OpCodes.Bne_Un_S, cont),
            il.Create(OpCodes.Br_S, emitSite)
        };

        foreach (var ins in gate.Reverse())
        {
            il.InsertBefore(cont, ins);
        }

        il.InsertBefore(cont, clearRet);
        il.InsertBefore(clearRet, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(clearRet, il.Create(OpCodes.Ldc_I4_0));
        il.InsertBefore(clearRet, il.Create(OpCodes.Stfld, loopTemp));

        il.InsertBefore(cont, emitSite);
        ChainIlSplice.SpliceEmit(il, cont, method, type, donorEmit, taskVar);
        il.InsertBefore(cont, il.Create(OpCodes.Ret));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndFail: inline chain hook injected (MaxStack={body.MaxStackSize}).");
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

    private static Instruction FindInsertPoint(MethodBody body, VariableDefinition taskVar)
    {
        for (var i = 0; i < body.Instructions.Count - 1; i++)
        {
            if (!LoadsTaskVar(body.Instructions[i], taskVar))
            {
                continue;
            }

            var next = body.Instructions[i + 1];
            if (next.OpCode == OpCodes.Brfalse || next.OpCode == OpCodes.Brfalse_S)
            {
                return next.Next ?? next;
            }

            if (next.OpCode == OpCodes.Callvirt &&
                next.Operand is MethodReference { Name: "GetMe" } &&
                i + 2 < body.Instructions.Count &&
                (body.Instructions[i + 2].OpCode == OpCodes.Brfalse || body.Instructions[i + 2].OpCode == OpCodes.Brfalse_S))
            {
                return body.Instructions[i + 2].Next!;
            }
        }

        throw new InvalidOperationException("ProcessEndFail: continue point not found.");
    }

    private static bool LoadsTaskVar(Instruction ins, VariableDefinition taskVar) =>
        (ins.OpCode == OpCodes.Ldloc && ins.Operand == taskVar) ||
        (ins.OpCode == OpCodes.Ldloc_0 && taskVar.Index == 0) ||
        (ins.OpCode == OpCodes.Ldloc_1 && taskVar.Index == 1) ||
        (ins.OpCode == OpCodes.Ldloc_2 && taskVar.Index == 2) ||
        (ins.OpCode == OpCodes.Ldloc_3 && taskVar.Index == 3);

    private static MethodReference FindGetMe(ModuleDefinition module)
    {
        var node = module.Types.First(t => t.Methods.Any(m => m.Name == "GetMe"));
        return module.ImportReference(node.Methods.First(m => m.Name == "GetMe" && m.Parameters.Count == 0));
    }

    private static FieldReference FindMeField(ModuleDefinition module, string name) =>
        module.Types.First(t => t.Name == "MissionElement").Fields.First(f => f.Name == name);

    private static MethodReference FindLog(ModuleDefinition module) =>
        module.Types.First(t => t.Name == "Logger").Methods.First(m => m.Name == "Log");

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldstr &&
            (i.Operand as string == "ForceCompleteV2: retry task " ||
             i.Operand as string == "ForceCompleteV2: doc-complete task "));
}
