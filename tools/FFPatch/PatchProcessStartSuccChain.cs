using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class PatchProcessStartSuccChain
{
    internal static void Apply(TypeDefinition type, TypeDefinition donorType)
    {
        var donorEmit = donorType.Methods.First(m => m.Name == "EmitDocCompletePacket");
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
            throw new System.InvalidOperationException("ProcessStartSucc: iTaskNum local not found.");
        }

        var taskVar = new VariableDefinition(type.Module.ImportReference(
            type.Module.Types.First(t => t.Name == "cnMissionNode")));
        body.Variables.Add(taskVar);
        var getTask = type.Methods.First(m => m.Name == "GetTask" && m.Parameters.Count == 1);
        var loopTemp = type.Fields.First(f => f.Name == "m_iloopTemp");
        var fctBody = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask").Body!;
        var requireInstanceField = IlLookup.FindField(fctBody, type.Module, "m_iRequireInstanceID");
        var taskIdField = IlLookup.FindField(fctBody, type.Module, "m_iHTaskID");
        var getMe = IlLookup.FindGetMe(type.Module);
        var logMethod = IlLookup.FindLog(body);
        var toString = type.Module.ImportReference(typeof(int).GetMethod("ToString", System.Type.EmptyTypes)!);

        var anchor = body.Instructions.First(i =>
            i.OpCode == OpCodes.Ldstr && i.Operand as string == "ProcessStartSucc ");
        anchor = anchor.Next ?? anchor;
        while (anchor != null && anchor.OpCode == OpCodes.Call)
        {
            anchor = anchor.Next;
        }

        var il = body.GetILProcessor();
        var cont = anchor!;
        var emitSite = il.Create(OpCodes.Nop);

        var gate = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, loopTemp),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ble_S, cont),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldloc, taskNumVar),
            il.Create(OpCodes.Call, getTask),
            il.Create(OpCodes.Stloc, taskVar),
            il.Create(OpCodes.Ldloc, taskVar),
            il.Create(OpCodes.Brfalse_S, cont),
            il.Create(OpCodes.Ldloc, taskVar),
            il.Create(OpCodes.Callvirt, getMe),
            il.Create(OpCodes.Ldfld, requireInstanceField),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ble_S, cont),
            il.Create(OpCodes.Ldstr, "ForceCompleteV2: instance zone complete task "),
            il.Create(OpCodes.Ldloc, taskVar),
            il.Create(OpCodes.Callvirt, getMe),
            il.Create(OpCodes.Ldfld, taskIdField),
            il.Create(OpCodes.Call, toString),
            il.Create(OpCodes.Call, logMethod),
            il.Create(OpCodes.Br_S, emitSite)
        };

        foreach (var ins in gate.Reverse())
        {
            il.InsertBefore(cont, ins);
        }

        il.InsertBefore(cont, emitSite);
        ChainIlSplice.SpliceEmit(il, cont, method, type, donorEmit, taskVar);
        il.InsertBefore(cont, il.Create(OpCodes.Ret));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessStartSucc: inline chain hook injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldstr &&
            i.Operand as string == "ForceCompleteV2: instance zone complete task ");
}
