using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Hotkey diagnostics + active-mission fallback without growing the FCT donor (1520640 slot).
/// </summary>
internal static class PatchForceCompleteHotkeyFix
{
    private const string EntryStamp = "ForceCompleteV2: patch build 2026-06-09-doc504c";

    internal static void ApplyFctEntryLog(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var body = method.Body;
        if (body.Instructions.Count > 2 &&
            body.Instructions[0].OpCode == OpCodes.Ldstr &&
            body.Instructions[0].Operand as string == EntryStamp &&
            body.Instructions[1].OpCode == OpCodes.Call &&
            body.Instructions[1].Operand is MethodReference { Name: "Log" })
        {
            Console.WriteLine("ForceCompleteCurrentTask: entry log already at method start (skip)");
            return;
        }

        var il = body.GetILProcessor();
        var loggerLog = IlLookup.FindLog(body);

        Instruction? existingLogCall = null;
        Instruction? existingLdstr = null;
        for (var i = 0; i < body.Instructions.Count - 1; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ldstr &&
                body.Instructions[i].Operand as string == EntryStamp &&
                body.Instructions[i + 1].OpCode == OpCodes.Call &&
                body.Instructions[i + 1].Operand is MethodReference { Name: "Log" })
            {
                existingLdstr = body.Instructions[i];
                existingLogCall = body.Instructions[i + 1];
                break;
            }
        }

        if (existingLdstr != null && existingLogCall != null &&
            body.Instructions.IndexOf(existingLdstr) < 4)
        {
            Console.WriteLine("ForceCompleteCurrentTask: entry log already present (skip)");
            return;
        }

        var first = body.Instructions[0];
        il.InsertBefore(first, il.Create(OpCodes.Call, loggerLog));
        il.InsertBefore(first, il.Create(OpCodes.Ldstr, EntryStamp));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("ForceCompleteCurrentTask: entry log injected (hotkey ack).");
    }

    internal static void ApplyFctActiveListFallback(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var body = method.Body;
        if (HasActiveListFallback(body))
        {
            Console.WriteLine("ForceCompleteCurrentTask: active-list fallback already present (skip)");
            return;
        }

        var getSelectedIdx = body.Instructions.ToList().FindIndex(i =>
            i.OpCode == OpCodes.Call && i.Operand is MethodReference { Name: "GetSelectedActiveMission" });
        if (getSelectedIdx < 0)
        {
            throw new InvalidOperationException("ForceCompleteCurrentTask: GetSelectedActiveMission call not found.");
        }

        Instruction? nullRet = null;
        for (var i = getSelectedIdx + 1; i < body.Instructions.Count; i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Ret)
            {
                nullRet = body.Instructions[i];
                break;
            }
        }

        if (nullRet == null)
        {
            throw new InvalidOperationException("ForceCompleteCurrentTask: post-selection ret not found.");
        }

        var targetVar = FindTargetLocal(body, getSelectedIdx);
        var listField = IlLookup.FindField(body, "m_ActivateMissionList");
        var getCount = FindCallvirtInType(type, "get_Count");
        var getItem = FindCallvirtInType(type, "get_Item");
        var missionNodeType = type.Module.Types.First(t => t.Name == "cnMissionNode");

        var il = body.GetILProcessor();
        var skipFallback = il.Create(OpCodes.Nop);
        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, listField),
            il.Create(OpCodes.Callvirt, getCount),
            il.Create(OpCodes.Brfalse_S, skipFallback),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, listField),
            il.Create(OpCodes.Dup),
            il.Create(OpCodes.Callvirt, getCount),
            il.Create(OpCodes.Ldc_I4_1),
            il.Create(OpCodes.Sub),
            il.Create(OpCodes.Callvirt, getItem),
            il.Create(OpCodes.Castclass, missionNodeType),
            il.Create(OpCodes.Stloc, targetVar),
            skipFallback
        };

        foreach (var ins in block.Reverse())
        {
            il.InsertBefore(nullRet, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("ForceCompleteCurrentTask: active-list fallback injected.");
    }

    private static VariableDefinition FindTargetLocal(MethodBody body, int afterGetSelectedIdx)
    {
        for (var i = afterGetSelectedIdx + 1; i < Math.Min(afterGetSelectedIdx + 8, body.Instructions.Count); i++)
        {
            if (body.Instructions[i].OpCode == OpCodes.Stloc && body.Instructions[i].Operand is VariableDefinition v)
            {
                return v;
            }

            var op = body.Instructions[i].OpCode;
            if (op == OpCodes.Stloc_0 || op == OpCodes.Stloc_1 || op == OpCodes.Stloc_2 || op == OpCodes.Stloc_3)
            {
                var idx = op == OpCodes.Stloc_0 ? 0 :
                    op == OpCodes.Stloc_1 ? 1 :
                    op == OpCodes.Stloc_2 ? 2 : 3;
                return body.Variables[idx];
            }
        }

        throw new InvalidOperationException("ForceCompleteCurrentTask: target local not found.");
    }

    private static MethodReference FindCallvirtInType(TypeDefinition type, string name)
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            var hit = method.Body!.Instructions.FirstOrDefault(i =>
                i.OpCode == OpCodes.Callvirt && i.Operand is MethodReference mr && mr.Name == name);
            if (hit?.Operand is MethodReference mr)
            {
                return mr;
            }
        }

        throw new InvalidOperationException("IlLookup: callvirt " + name + " not found.");
    }

    internal static void ApplyGetSelectedFallback(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "GetSelectedActiveMission");
        var body = method.Body;
        if (HasActiveListFallback(body))
        {
            Console.WriteLine("GetSelectedActiveMission: active-list fallback already present (skip)");
            return;
        }

        var listField = IlLookup.FindField(body, "m_ActivateMissionList");
        var getCount = FindCallvirt(body, "get_Count");
        var getItem = FindCallvirt(body, "get_Item");
        var missionNodeType = type.Module.Types.First(t => t.Name == "cnMissionNode");

        var il = body.GetILProcessor();
        var ret = body.Instructions.Last(i => i.OpCode == OpCodes.Ret);
        var skipFallback = il.Create(OpCodes.Ret);

        var block = new[]
        {
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, listField),
            il.Create(OpCodes.Callvirt, getCount),
            il.Create(OpCodes.Brfalse_S, skipFallback),
            il.Create(OpCodes.Ldarg_0),
            il.Create(OpCodes.Ldfld, listField),
            il.Create(OpCodes.Dup),
            il.Create(OpCodes.Callvirt, getCount),
            il.Create(OpCodes.Ldc_I4_1),
            il.Create(OpCodes.Sub),
            il.Create(OpCodes.Callvirt, getItem),
            il.Create(OpCodes.Castclass, missionNodeType),
            il.Create(OpCodes.Ret),
            skipFallback
        };

        foreach (var ins in block.Reverse())
        {
            il.InsertBefore(ret, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("GetSelectedActiveMission: active-list fallback injected.");
    }

    private static MethodReference FindCallvirt(MethodBody body, string name) =>
        body.Instructions
            .Where(i => i.OpCode == OpCodes.Callvirt && i.Operand is MethodReference mr && mr.Name == name)
            .Select(i => (MethodReference)i.Operand!)
            .First();

    private static bool HasActiveListFallback(MethodBody body)
    {
        for (var i = 0; i < body.Instructions.Count - 3; i++)
        {
            if (body.Instructions[i].OpCode != OpCodes.Callvirt ||
                body.Instructions[i].Operand is not MethodReference mr ||
                mr.Name != "get_Count")
            {
                continue;
            }

            if (body.Instructions[i + 1].OpCode == OpCodes.Ldc_I4_1 &&
                body.Instructions[i + 2].OpCode == OpCodes.Sub)
            {
                return true;
            }
        }

        return false;
    }
}
