using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Block fail-outgoing ProcessStartSucc (466) while force-complete chain has advanced past it.
/// </summary>
internal static class PatchProcessStartSuccBlock
{
    internal static void Apply(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessStartSucc");
        var body = method.Body;
        if (HasBlock(body))
        {
            Console.WriteLine("ProcessStartSucc: chain block already present (skip)");
            return;
        }

        VariableDefinition? taskNumVar = null;
        Instruction? insertAt = null;
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
                insertAt = store.Next;
                break;
            }

            if (store.OpCode == OpCodes.Stloc_1 && body.Variables.Count > 1)
            {
                taskNumVar = body.Variables[1];
                insertAt = store.Next;
                break;
            }
        }

        if (taskNumVar == null || insertAt == null)
        {
            throw new InvalidOperationException("ProcessStartSucc: iTaskNum local not found for block.");
        }

        var chainField = type.Fields.First(f => f.Name == "bForceCompleteChain");
        var depthField = type.Fields.First(f => f.Name == "m_iForceCompleteChainDepth");

        var il = body.GetILProcessor();
        var cont = insertAt;
        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, chainField),
            il.Create(OpCodes.Brfalse_S, cont),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, depthField),
            il.Create(OpCodes.Brfalse_S, cont),
            il.Create(OpCodes.Ldloc, taskNumVar),
            il.Create(OpCodes.Ldc_I4, 466),
            il.Create(OpCodes.Bne_Un_S, cont),
            il.Create(OpCodes.Ret)
        };

        foreach (var ins in block.Reverse())
        {
            il.InsertBefore(cont, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"ProcessStartSucc: fail-outgoing 466 block injected (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasBlock(MethodBody body) =>
        body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldc_I4 && i.Operand is int n && n == 466 &&
            i.Next?.OpCode == OpCodes.Bne_Un_S);
}
