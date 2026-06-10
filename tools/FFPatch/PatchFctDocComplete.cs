using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Replace FCT hotkey tail (generic RTC) with inlined EmitDocCompletePacket; keep m_iloopTemp for chain hooks.
/// </summary>
internal static class PatchFctDocComplete
{
    internal static void Apply(TypeDefinition type, TypeDefinition donorType)
    {
        var method = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var body = method.Body;
        if (HasDocCompleteEmit(body))
        {
            Console.WriteLine("FCT: doc-complete emit already inlined (skip)");
            return;
        }

        var rtcCall = body.Instructions.First(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete" &&
            mr.Parameters.Count == 3);
        var taskVar = FindTargetLocal(body, rtcCall);
        var tailStart = FindTailStart(body, rtcCall, type.Fields.First(f => f.Name == "m_iloopTemp"));
        var removeThrough = rtcCall;
        var scan = rtcCall.Next;
        while (scan != null && ClearsLoopTemp(scan, type.Fields.First(f => f.Name == "m_iloopTemp")))
        {
            removeThrough = scan;
            scan = scan.Next;
        }

        var il = body.GetILProcessor();
        var insertAt = removeThrough.Next
            ?? throw new InvalidOperationException("FCT: no continuation after doc-complete tail.");

        var toRemove = new List<Instruction>();
        for (var ins = tailStart; ins != null; ins = ins.Next)
        {
            toRemove.Add(ins);
            if (ins == removeThrough)
            {
                break;
            }
        }

        foreach (var ins in toRemove)
        {
            il.Remove(ins);
        }
        var loopTemp = type.Fields.First(f => f.Name == "m_iloopTemp");
        var donorEmit = donorType.Methods.First(m => m.Name == "EmitDocCompletePacketHotkey");

        il.InsertBefore(insertAt, il.Create(OpCodes.Ldarg_0));
        il.InsertBefore(insertAt, il.Create(OpCodes.Ldc_I4_1));
        il.InsertBefore(insertAt, il.Create(OpCodes.Stfld, loopTemp));
        ChainIlSplice.SpliceEmit(il, insertAt, method, type, donorEmit, taskVar);

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"FCT: inlined EmitDocCompletePacket (MaxStack={body.MaxStackSize}).");
    }

    private static bool HasDocCompleteEmit(MethodBody body) =>
        body.Instructions.Count(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete" &&
            mr.Parameters.Count == 3) >= 2;

    private static VariableDefinition FindTargetLocal(MethodBody body, Instruction rtcCall)
    {
        for (var ins = rtcCall.Previous; ins != null; ins = ins.Previous)
        {
            if (ins.OpCode != OpCodes.Callvirt ||
                ins.Operand is not MethodReference { Name: "GetMe" })
            {
                continue;
            }

            var load = ins.Previous;
            if (load == null)
            {
                continue;
            }

            if (load.OpCode == OpCodes.Ldloc && load.Operand is VariableDefinition v)
            {
                return v;
            }

            if (load.OpCode == OpCodes.Ldloc_0 || load.OpCode == OpCodes.Ldloc_1 ||
                load.OpCode == OpCodes.Ldloc_2 || load.OpCode == OpCodes.Ldloc_3)
            {
                return body.Variables[load.OpCode.Value - OpCodes.Ldloc_0.Value];
            }
        }

        throw new InvalidOperationException("FCT: target local not found before RequestTaskComplete.");
    }

    private static Instruction FindTailStart(MethodBody body, Instruction rtcCall, FieldDefinition loopTemp)
    {
        for (var ins = rtcCall.Previous; ins != null; ins = ins.Previous)
        {
            if (ins.OpCode == OpCodes.Stfld &&
                ins.Operand is FieldReference fr &&
                fr.Name == "m_iloopTemp" &&
                ins.Previous?.OpCode == OpCodes.Ldc_I4_1)
            {
                return ins.Previous.Previous ?? ins.Previous;
            }
        }

        for (var ins = rtcCall.Previous; ins != null; ins = ins.Previous)
        {
            if (ins.OpCode == OpCodes.Stfld &&
                ReferenceEquals(ins.Operand, loopTemp) &&
                ins.Previous?.OpCode == OpCodes.Ldc_I4_1)
            {
                return ins.Previous.Previous ?? ins.Previous;
            }
        }

        throw new InvalidOperationException("FCT: m_iloopTemp=1 tail start not found.");
    }

    private static bool ClearsLoopTemp(Instruction ins, FieldDefinition loopTemp) =>
        ins.OpCode == OpCodes.Stfld &&
        ins.Operand is FieldReference fr &&
        fr.Name == "m_iloopTemp" &&
        ins.Previous?.OpCode == OpCodes.Ldc_I4_0;
}
