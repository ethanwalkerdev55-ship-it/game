using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Surgical ProcessEndFail: delegate to ForceCompleteOnEndFail during active chain (no full transplant).
/// </summary>
internal static class PatchProcessEndFailChain
{
    internal static void Apply(TypeDefinition type)
    {
        var handler = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteOnEndFail");
        if (handler == null)
        {
            throw new System.InvalidOperationException("ForceCompleteOnEndFail missing — build donor first.");
        }

        var method = type.Methods.First(m => m.Name == "ProcessEndFail");
        var body = method.Body;
        if (HasChainHook(body))
        {
            Console.WriteLine("ProcessEndFail: chain handler hook already present (skip)");
            return;
        }

        var getTaskCall = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "GetTask");
        if (getTaskCall == null)
        {
            throw new System.InvalidOperationException("ProcessEndFail: GetTask call not found.");
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
            throw new InvalidOperationException("ProcessEndFail: task local not found.");
        }

        Instruction? insertAt = null;
        bool LoadsTaskVar(Instruction ins) =>
            (ins.OpCode == OpCodes.Ldloc && ins.Operand == taskVar) ||
            (ins.OpCode == OpCodes.Ldloc_0 && taskVar.Index == 0) ||
            (ins.OpCode == OpCodes.Ldloc_1 && taskVar.Index == 1) ||
            (ins.OpCode == OpCodes.Ldloc_2 && taskVar.Index == 2) ||
            (ins.OpCode == OpCodes.Ldloc_3 && taskVar.Index == 3);

        for (var i = 0; i < body.Instructions.Count - 1; i++)
        {
            if (!LoadsTaskVar(body.Instructions[i]))
            {
                continue;
            }

            var next = body.Instructions[i + 1];
            if (next.OpCode == OpCodes.Brfalse || next.OpCode == OpCodes.Brfalse_S)
            {
                insertAt = next.Next;
                break;
            }

            if (next.OpCode == OpCodes.Callvirt &&
                next.Operand is MethodReference getMe &&
                getMe.Name == "GetMe" &&
                i + 2 < body.Instructions.Count &&
                (body.Instructions[i + 2].OpCode == OpCodes.Brfalse || body.Instructions[i + 2].OpCode == OpCodes.Brfalse_S))
            {
                insertAt = body.Instructions[i + 2].Next;
                break;
            }
        }

        if (insertAt == null)
        {
            throw new InvalidOperationException("ProcessEndFail: null-check continue point not found.");
        }

        var errorField = body.Instructions
            .First(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference fr && fr.Name == "iErrorCode")
            .Operand as FieldReference;

        var il = body.GetILProcessor();
        var cont = insertAt;
        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldloc, taskVar),
            il.Create(OpCodes.Ldloc_0),
            il.Create(OpCodes.Ldfld, errorField!),
            il.Create(OpCodes.Call, handler),
            il.Create(OpCodes.Brfalse_S, cont),
            il.Create(OpCodes.Ret)
        };

        foreach (var ins in block.Reverse())
        {
            il.InsertBefore(cont, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessEndFail: chain handler hook injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasChainHook(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "ForceCompleteOnEndFail");
}
