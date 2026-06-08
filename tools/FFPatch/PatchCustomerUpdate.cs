using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Surgical Update patch: timer defer block only. Never replace full Update from DonorCompile
/// (stub eGameMode.MainGame = 0 vs game value 5 breaks startup → black screen).
/// </summary>
internal static class PatchCustomerUpdate
{
    internal static void InjectPendingTimerDefer(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "Update");
        var body = method.Body;
        if (HasPendingTimerDefer(body))
        {
            Console.WriteLine("Update: ForceComplete pending timer defer already present (skip)");
            return;
        }

        var refreshCall = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RefreshQuestSymbol");
        if (refreshCall == null)
        {
            throw new InvalidOperationException("Update: RefreshQuestSymbol call not found.");
        }

        FieldReference BorrowField(string name)
        {
            foreach (var helperName in new[] { "TryForceCompleteChainTask", "PrepareTaskForForceComplete", "NeedsForceCompleteTaskStart" })
            {
                var helper = type.Methods.FirstOrDefault(m => m.Name == helperName && m.Body != null);
                if (helper?.Body == null)
                {
                    continue;
                }

                var match = helper.Body.Instructions.FirstOrDefault(i =>
                    i.OpCode == OpCodes.Ldfld &&
                    i.Operand is FieldReference fr &&
                    fr.Name == name);
                if (match?.Operand is FieldReference fieldRef)
                {
                    return fieldRef;
                }
            }

            throw new InvalidOperationException("Update: field ref not found for timer defer: " + name);
        }

        var stGrantField = BorrowField("m_iSTGrantTimer");
        var csuField = BorrowField("m_iCSUCheckTimer");
        var hTaskField = BorrowField("m_iHTaskID");
        var remainTimeField = body.Instructions
            .First(i => i.OpCode == OpCodes.Ldfld && i.Operand is FieldReference fr && fr.Name == "m_fRemainTime")
            .Operand as FieldReference
            ?? throw new InvalidOperationException("Update: m_fRemainTime not found.");

        var chainField = type.Fields.First(f => f.Name == "bForceCompleteChain");
        var pendingIdField = type.Fields.First(f => f.Name == "m_iForceCompletePendingTaskId");
        var getTask = type.Methods.First(m => m.Name == "GetTask" && m.Parameters.Count == 1);
        var isExist = type.Methods.First(m => m.Name == "IsExistTaskInActiveMission" && m.Parameters.Count == 1);
        var requestEnd = type.Methods.First(m => m.Name == "RequestForceCompleteTaskEnd" && m.Parameters.Count == 1);
        var getMeRef = body.Instructions
            .First(i => i.OpCode == OpCodes.Callvirt && i.Operand is MethodReference mr && mr.Name == "GetMe")
            .Operand as MethodReference
            ?? throw new InvalidOperationException("Update: could not resolve GetMe for timer defer.");
        var cnMissionNodeType = type.Module.Types.First(t => t.Name == "cnMissionNode");
        var getStateRef = type.Module.ImportReference(
            cnMissionNodeType.Methods.First(m => m.Name == "GetMissionState" && m.Parameters.Count == 0));

        var il = body.GetILProcessor();
        var skip = refreshCall.Next ?? throw new InvalidOperationException("Update: no instruction after RefreshQuestSymbol.");

        var timerHasValue = il.Create(OpCodes.Nop);
        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, chainField),
            il.Create(OpCodes.Brfalse_S, skip),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, pendingIdField),
            il.Create(OpCodes.Bge_S, skip),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, pendingIdField),
            il.Create(OpCodes.Call, getTask),
            il.Create(OpCodes.Stloc_2),
            il.Create(OpCodes.Ldloc_2),
            il.Create(OpCodes.Brfalse_S, skip),
            il.Create(OpCodes.Ldloc_2),
            il.Create(OpCodes.Callvirt, getMeRef),
            il.Create(OpCodes.Brfalse_S, skip),
            il.Create(OpCodes.Ldloc_2),
            il.Create(OpCodes.Callvirt, getMeRef),
            il.Create(OpCodes.Stloc_3),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ldloc_3),
            il.Create(OpCodes.Ldfld, stGrantField),
            il.Create(OpCodes.Blt_S, timerHasValue),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ldloc_3),
            il.Create(OpCodes.Ldfld, csuField),
            il.Create(OpCodes.Bge_S, skip),
            timerHasValue,
            il.Create(OpCodes.Ldloc_2),
            il.Create(OpCodes.Ldfld, remainTimeField),
            il.Create(OpCodes.Ldc_R4, 0f),
            il.Create(OpCodes.Bgt_Un_S, skip),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldloc_3),
            il.Create(OpCodes.Ldfld, hTaskField),
            il.Create(OpCodes.Call, isExist),
            il.Create(OpCodes.Brfalse_S, skip),
            il.Create(OpCodes.Ldloc_2),
            il.Create(OpCodes.Callvirt, getStateRef),
            il.Create(OpCodes.Ldc_I4_1),
            il.Create(OpCodes.Bne_Un_S, skip),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Stfld, pendingIdField),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldloc_2),
            il.Create(OpCodes.Call, requestEnd)
        };

        InsertAfterInOrder(il, refreshCall, block);
        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"Update: injected ForceComplete pending timer defer (MaxStack={body.MaxStackSize}).");
    }

    internal static bool HasHealthyMainGameMode(MethodBody body)
    {
        for (var i = 0; i < body.Instructions.Count - 2; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldsfld &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldc_I4_5 &&
                body.Instructions[i + 2].OpCode == OpCodes.Callvirt &&
                body.Instructions[i + 2].Operand is MethodReference mr &&
                mr.Name == "IsGameMode")
            {
                return true;
            }
        }

        return false;
    }

    internal static bool HasPendingTimerDefer(MethodBody body)
    {
        return body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestForceCompleteTaskEnd");
    }

    private static void InsertAfterInOrder(ILProcessor il, Instruction after, Instruction[] block)
    {
        var anchor = after;
        foreach (var instr in block)
        {
            il.InsertAfter(anchor, instr);
            anchor = instr;
        }
    }
}
