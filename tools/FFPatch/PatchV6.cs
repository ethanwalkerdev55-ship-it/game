using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// V6: mod_v2-style complete — no DelMissionTaskChecker, depth-based checker bypass, direct RequestTaskComplete.
/// </summary>
internal static class PatchV6
{
    public static bool Apply(string goldenPath)
    {
        goldenPath = Path.GetFullPath(goldenPath);
        if (!File.Exists(goldenPath))
        {
            Console.Error.WriteLine("Golden not found: " + goldenPath);
            return false;
        }

        var needsDirectComplete = !HasValidDirectRequestTaskComplete(goldenPath);
        var preV6 = goldenPath + ".pre-v6.bak";
        if (!needsDirectComplete && File.Exists(preV6) && GoldenHasCorruptTryForceComplete(goldenPath))
        {
            Console.WriteLine("Golden has corrupt V6 TryForceComplete IL — restoring from pre-v6.bak");
            File.Copy(preV6, goldenPath, true);
            needsDirectComplete = true;
        }

        if (!needsDirectComplete && !NeedsTimerDeferFix(goldenPath))
        {
            Console.WriteLine("Golden already has V6 direct complete.");
            return true;
        }

        if (needsDirectComplete && File.Exists(preV6) && GoldenHasCorruptTryForceComplete(goldenPath))
        {
            Console.WriteLine("Restoring golden from pre-v6.bak before V6 re-apply");
            File.Copy(preV6, goldenPath, true);
        }

        if (needsDirectComplete && !File.Exists(goldenPath + ".pre-v6.bak"))
        {
            var backup = goldenPath + ".pre-v6.bak";
            File.Copy(goldenPath, backup, true);
            Console.WriteLine("Backup: " + backup);
        }

        var resolver = new DefaultAssemblyResolver();
        var goldenDir = Path.GetDirectoryName(goldenPath)!;
        resolver.AddSearchDirectory(goldenDir);
        var inspectBundle = Path.GetFullPath(Path.Combine(goldenDir, "..", "..", "..", "_inspect_bundle", "main.bak"));
        if (Directory.Exists(inspectBundle))
        {
            resolver.AddSearchDirectory(inspectBundle);
        }

        var tempPath = goldenPath + ".v6.tmp";
        using (var asm = AssemblyDefinition.ReadAssembly(goldenPath, new ReaderParameters
        {
            AssemblyResolver = resolver,
            ReadWrite = false
        }))
        {
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            PatchForceCompleteRemoveDelChecker(type);
            PatchRequestTaskCompleteChainBypass(type);
            PatchTryForceCompleteSkipTimerDefer(type);
            PatchTryForceCompleteDirectRequestTaskComplete(type);
            PatchRequestTaskCompleteAlwaysLogSent(type);
            asm.Write(tempPath);
        }

        File.Copy(tempPath, goldenPath, true);
        File.Delete(tempPath);
        Console.WriteLine("Golden V6 patched: " + goldenPath);
        return Verify(goldenPath);
    }

    internal static void PatchForceCompleteRemoveDelChecker(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var body = method.Body;
        var il = body.GetILProcessor();
        for (var i = 0; i < body.Instructions.Count; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Call &&
                body.Instructions[i].Operand is MethodReference mr &&
                mr.Name == "DelMissionTaskChecker")
            {
                il.Remove(body.Instructions[i - 2]);
                il.Remove(body.Instructions[i - 2]);
                il.Remove(body.Instructions[i - 2]);
                il.Remove(body.Instructions[i - 2]);
                Console.WriteLine("ForceCompleteCurrentTask: removed DelMissionTaskChecker call");
                return;
            }
        }

        Console.WriteLine("ForceCompleteCurrentTask: DelMissionTaskChecker call not found (skip)");
    }

    /// <summary>
    /// main.current requires SearchMissionTaskChecker &gt;= 0 before starting a chain.
    /// That silently no-ops when the checker list is empty — hotkey appears dead.
    /// </summary>
    internal static void PatchForceCompleteSkipCheckerGate(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var body = method.Body;
        if (HasForceCompleteCheckerGateRemoved(body))
        {
            Console.WriteLine("ForceCompleteCurrentTask: checker gate already removed (skip)");
            return;
        }

        var il = body.GetILProcessor();
        for (var i = 0; i < body.Instructions.Count - 2; i++)
        {
            if (body.Instructions[i].OpCode != OpCodes.Call ||
                body.Instructions[i].Operand is not MethodReference mr ||
                mr.Name != "SearchMissionTaskChecker")
            {
                continue;
            }

            var branch = body.Instructions[i + 1];
            var fallThrough = body.Instructions[i + 2];
            if (branch.OpCode != OpCodes.Bge_S || fallThrough.OpCode != OpCodes.Ret)
            {
                continue;
            }

            var continueAt = (Instruction)branch.Operand!;
            var gateStart = body.Instructions[i];
            for (var j = i - 1; j >= 0; j--)
            {
                if (body.Instructions[j].OpCode == OpCodes.Ldc_I4_0)
                {
                    gateStart = body.Instructions[j];
                    break;
                }
            }

            foreach (var instr in body.Instructions)
            {
                if (instr.OpCode == OpCodes.Brtrue_S && instr.Operand == gateStart)
                {
                    instr.Operand = continueAt;
                }
            }

            var toRemove = new List<Instruction>();
            var removing = false;
            foreach (var instr in body.Instructions)
            {
                if (instr == gateStart)
                {
                    removing = true;
                }

                if (removing)
                {
                    toRemove.Add(instr);
                }

                if (instr == fallThrough)
                {
                    break;
                }
            }

            foreach (var instr in toRemove)
            {
                il.Remove(instr);
            }

            Console.WriteLine("ForceCompleteCurrentTask: removed SearchMissionTaskChecker gate");
            return;
        }

        Console.WriteLine("ForceCompleteCurrentTask: SearchMissionTaskChecker gate not found (skip)");
    }

    internal static bool HasForceCompleteCheckerGateRemoved(MethodBody body)
    {
        return !body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "SearchMissionTaskChecker");
    }

    internal static void PatchRequestTaskCompleteChainBypass(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        var body = method.Body;
        if (HasChainDepthBypass(body))
        {
            Console.WriteLine("RequestTaskComplete: chain depth bypass already present (skip)");
            return;
        }

        var depthField = type.Fields.First(f => f.Name == "m_iForceCompleteChainDepth");
        var chainField = type.Fields.First(f => f.Name == "bForceCompleteChain");

        Instruction? getTaskCall = null;
        for (var i = 0; i < body.Instructions.Count - 2; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldarg_1 &&
                body.Instructions[i + 2].OpCode == OpCodes.Call &&
                body.Instructions[i + 2].Operand is MethodReference mr &&
                mr.Name == "GetTask")
            {
                getTaskCall = body.Instructions[i];
                break;
            }
        }

        if (getTaskCall == null)
        {
            throw new InvalidOperationException("RequestTaskComplete: GetTask call not found.");
        }

        Instruction? insertAfter = null;
        for (var i = 0; i < body.Instructions.Count - 2; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldc_I4_0 &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldarg_1 &&
                body.Instructions[i + 2].OpCode == OpCodes.Bge_S)
            {
                insertAfter = body.Instructions[i + 2];
                break;
            }
        }

        if (insertAfter == null)
        {
            throw new InvalidOperationException("RequestTaskComplete: task id guard not found.");
        }

        var il = body.GetILProcessor();
        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, depthField),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Bgt_S, getTaskCall),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, chainField),
            il.Create(OpCodes.Brtrue_S, getTaskCall)
        };

        InsertAfterInOrder(il, insertAfter, block);
        Console.WriteLine("RequestTaskComplete: bypass checker when chain depth > 0 or bForceCompleteChain");
    }

    internal static void PatchTryForceCompleteSkipTimerDefer(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "TryForceCompleteChainTask");
        var body = method.Body;
        if (!NeedsTimerDeferFix(body))
        {
            Console.WriteLine("TryForceCompleteChainTask: timer defer skip already present (skip)");
            return;
        }

        Instruction? depthIncrement = null;
        for (var i = 0; i < body.Instructions.Count - 4; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i + 2].OpCode == OpCodes.Ldfld &&
                body.Instructions[i + 2].Operand is FieldReference fr &&
                fr.Name == "m_iForceCompleteChainDepth" &&
                body.Instructions[i + 3].OpCode == OpCodes.Ldc_I4_1 &&
                body.Instructions[i + 4].OpCode == OpCodes.Add)
            {
                depthIncrement = body.Instructions[i];
                break;
            }
        }

        if (depthIncrement == null)
        {
            throw new InvalidOperationException("TryForceCompleteChainTask: chain depth increment not found.");
        }

        for (var i = 0; i < body.Instructions.Count - 1; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Stfld &&
                body.Instructions[i].Operand is FieldReference fr &&
                fr.Name == "m_iForceCompletePendingTaskId" &&
                body.Instructions[i + 1].OpCode == OpCodes.Ret &&
                body.Instructions.IndexOf(body.Instructions[i + 1]) < body.Instructions.IndexOf(depthIncrement))
            {
                body.Instructions[i + 1].OpCode = OpCodes.Br;
                body.Instructions[i + 1].Operand = depthIncrement;
                Console.WriteLine("TryForceCompleteChainTask: timer defer early return removed (mod_v2 style)");
                return;
            }
        }

        throw new InvalidOperationException("TryForceCompleteChainTask: timer defer return not found.");
    }

    private static void PatchTryForceCompleteDirectRequestTaskComplete(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "TryForceCompleteChainTask");
        var body = method.Body;
        var il = body.GetILProcessor();

        if (HasValidDirectRequestTaskComplete(body))
        {
            Console.WriteLine("TryForceCompleteChainTask: direct RequestTaskComplete already valid (skip)");
            return;
        }

        RemoveCorruptTryForceCompleteTail(body, il);

        Instruction? endCall = null;
        for (var i = 0; i < body.Instructions.Count; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Call &&
                body.Instructions[i].Operand is MethodReference mr &&
                mr.Name == "RequestForceCompleteTaskEnd")
            {
                endCall = body.Instructions[i];
                break;
            }
        }

        if (endCall == null)
        {
            throw new InvalidOperationException("TryForceCompleteChainTask: RequestForceCompleteTaskEnd call not found.");
        }

        var endIdx = body.Instructions.IndexOf(endCall);
        var startIdx = endIdx;
        while (startIdx > 0 &&
               body.Instructions[startIdx - 1].OpCode != OpCodes.Call &&
               (body.Instructions[startIdx - 1].Operand is not MethodReference mr || mr.Name != "Log"))
        {
            startIdx--;
        }

        while (startIdx > 0 && body.Instructions[startIdx - 1].OpCode == OpCodes.Call &&
               body.Instructions[startIdx - 1].Operand is MethodReference logMr && logMr.Name == "Log")
        {
            break;
        }

        for (var i = endIdx - 1; i >= 0; i--)
        {
            if (body.Instructions[i].OpCode == OpCodes.Call &&
                body.Instructions[i].Operand is MethodReference mr &&
                mr.Name == "AddMissionTaskChecker")
            {
                startIdx = i - 2;
                break;
            }

            if (body.Instructions[i].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldarg_1 &&
                i + 2 < body.Instructions.Count &&
                body.Instructions[i + 2] == endCall)
            {
                startIdx = i;
                break;
            }
        }

        var taskIdField = body.Instructions
            .First(instr => instr.OpCode == OpCodes.Ldfld && instr.Operand is FieldReference fr && fr.Name == "m_iHTaskID")
            .Operand as FieldReference;
        var requestTaskComplete = type.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        var requestRef = method.Module.ImportReference(requestTaskComplete);

        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldloc_0),
            il.Create(OpCodes.Ldfld, taskIdField!),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Ldc_I4_0),
            il.Create(OpCodes.Call, requestRef)
        };

        var removeCount = endIdx - startIdx + 1;
        for (var r = 0; r < removeCount; r++)
        {
            il.Remove(body.Instructions[startIdx]);
        }

        var anchor = body.Instructions[startIdx];
        InsertBeforeInOrder(il, anchor, block);
        Console.WriteLine("TryForceCompleteChainTask: direct RequestTaskComplete (mod_v2 style)");
    }

    private static void RemoveCorruptTryForceCompleteTail(MethodBody body, ILProcessor il)
    {
        for (var i = 0; i < body.Instructions.Count; i++)
        {
            if (body.Instructions[i].OpCode != OpCodes.Call ||
                body.Instructions[i].Operand is not MethodReference mr ||
                mr.Name != "RequestTaskComplete")
            {
                continue;
            }

            if (i >= 5 &&
                body.Instructions[i - 5].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i - 4].OpCode == OpCodes.Ldloc_0)
            {
                return;
            }

            var ret = body.Instructions.Last(instr => instr.OpCode == OpCodes.Ret);
            while (body.Instructions.IndexOf(ret) > i)
            {
                il.Remove(ret);
                ret = body.Instructions.Last(instr => instr.OpCode == OpCodes.Ret);
            }

            while (i < body.Instructions.Count && body.Instructions[i].OpCode != OpCodes.Ret)
            {
                il.Remove(body.Instructions[i]);
            }

            Console.WriteLine("TryForceCompleteChainTask: removed corrupt RequestTaskComplete tail");
            return;
        }
    }

    private static void PatchRequestTaskCompleteAlwaysLogSent(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        var body = method.Body;
        var sendPacket = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "SendPacket");
        if (sendPacket == null)
        {
            return;
        }

        var sendIndex = body.Instructions.IndexOf(sendPacket);
        if (sendIndex >= 2 &&
            body.Instructions[sendIndex - 1].OpCode == OpCodes.Nop &&
            body.Instructions[sendIndex - 2].OpCode == OpCodes.Call)
        {
            for (var i = sendIndex - 1; i >= Math.Max(0, sendIndex - 12); i--)
            {
                if (body.Instructions[i].OpCode == OpCodes.Brfalse_S)
                {
                    body.Instructions[i].OpCode = OpCodes.Nop;
                    body.Instructions[i].Operand = null;
                    Console.WriteLine("RequestTaskComplete: sent log guard removed (always log)");
                    return;
                }
            }
        }
    }

    internal static bool HasChainDepthBypass(MethodBody body)
    {
        for (var i = 0; i < body.Instructions.Count - 4; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldfld &&
                body.Instructions[i].Operand is FieldReference fr &&
                fr.Name == "m_iForceCompleteChainDepth" &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldc_I4_0 &&
                body.Instructions[i + 2].OpCode == OpCodes.Bgt_S)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasValidDirectRequestTaskComplete(MethodBody body)
    {
        if (body.Instructions.Any(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "RequestForceCompleteTaskEnd"))
        {
            return false;
        }

        for (var i = 0; i < body.Instructions.Count; i++)
        {
            if (body.Instructions[i].OpCode != OpCodes.Call ||
                body.Instructions[i].Operand is not MethodReference mr ||
                mr.Name != "RequestTaskComplete")
            {
                continue;
            }

            if (i < 5)
            {
                return false;
            }

            return body.Instructions[i - 5].OpCode == OpCodes.Ldarg_0 &&
                   body.Instructions[i - 4].OpCode == OpCodes.Ldloc_0 &&
                   body.Instructions[i - 3].OpCode == OpCodes.Ldfld &&
                   body.Instructions[i - 2].OpCode == OpCodes.Ldc_I4_0 &&
                   body.Instructions[i - 1].OpCode == OpCodes.Ldc_I4_0;
        }

        return false;
    }

    private static bool NeedsTimerDeferFix(MethodBody body)
    {
        Instruction? depthIncrement = null;
        for (var i = 0; i < body.Instructions.Count - 4; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i + 1].OpCode == OpCodes.Ldarg_0 &&
                body.Instructions[i + 2].OpCode == OpCodes.Ldfld &&
                body.Instructions[i + 2].Operand is FieldReference fr &&
                fr.Name == "m_iForceCompleteChainDepth")
            {
                depthIncrement = body.Instructions[i];
                break;
            }
        }

        if (depthIncrement == null)
        {
            return false;
        }

        for (var i = 0; i < body.Instructions.Count - 1; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Stfld &&
                body.Instructions[i].Operand is FieldReference fr &&
                fr.Name == "m_iForceCompletePendingTaskId" &&
                body.Instructions[i + 1].OpCode == OpCodes.Ret &&
                body.Instructions.IndexOf(body.Instructions[i + 1]) < body.Instructions.IndexOf(depthIncrement))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasValidDirectRequestTaskComplete(string goldenPath)
    {
        using var asm = AssemblyDefinition.ReadAssembly(goldenPath);
        var method = asm.MainModule.Types.First(t => t.Name == "cnMissionManager")
            .Methods.First(m => m.Name == "TryForceCompleteChainTask");
        return HasValidDirectRequestTaskComplete(method.Body);
    }

    private static bool NeedsTimerDeferFix(string goldenPath)
    {
        using var asm = AssemblyDefinition.ReadAssembly(goldenPath);
        var method = asm.MainModule.Types.First(t => t.Name == "cnMissionManager")
            .Methods.First(m => m.Name == "TryForceCompleteChainTask");
        return NeedsTimerDeferFix(method.Body);
    }

    private static bool GoldenHasCorruptTryForceComplete(string goldenPath)
    {
        using var asm = AssemblyDefinition.ReadAssembly(goldenPath);
        var body = asm.MainModule.Types.First(t => t.Name == "cnMissionManager")
            .Methods.First(m => m.Name == "TryForceCompleteChainTask").Body;
        return body.Instructions.Any(i =>
                   i.OpCode == OpCodes.Call &&
                   i.Operand is MethodReference mr &&
                   mr.Name == "RequestTaskComplete") &&
               !HasValidDirectRequestTaskComplete(body);
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

    private static void InsertBeforeInOrder(ILProcessor il, Instruction before, Instruction[] block)
    {
        foreach (var instr in block)
        {
            il.InsertBefore(before, instr);
        }
    }

    private static bool Verify(string goldenPath)
    {
        if (HasValidDirectRequestTaskComplete(goldenPath) && !NeedsTimerDeferFix(goldenPath))
        {
            Console.WriteLine("V6 direct complete OK.");
            return true;
        }

        Console.Error.WriteLine("V6 direct complete missing or timer defer still present.");
        return false;
    }
}
