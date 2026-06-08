using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Mandatory IL checks before a patched DLL is considered deployable.
/// </summary>
internal static class PatchVerify
{
    private static readonly string[] LiteRequiredMethods =
    {
        "ForceCompleteCurrentTask",
        "GetForceCompleteTarget",
        "PrepareTaskForForceComplete",
        "NeedsForceCompleteTaskStart",
        "TryForceCompleteChainTask",
        "ForceCompleteEmitTaskEndPacket",
        "RequestForceCompleteTaskEnd"
    };

    public static bool Run(string dllPath, bool liteMode = false)
    {
        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            Console.Error.WriteLine("verify-il: not found: " + dllPath);
            return false;
        }

        using var asm = AssemblyDefinition.ReadAssembly(dllPath);
        var type = asm.MainModule.Types.FirstOrDefault(t => t.Name == "cnMissionManager");
        if (type == null)
        {
            Console.Error.WriteLine("verify-il: cnMissionManager missing.");
            return false;
        }

        var ok = true;
        ok &= CheckMarkers(dllPath, clientMode: true);
        ok &= CheckRequiredMethods(type, liteMode);
        ok &= CheckTryForceCompletePath(type, liteMode);
        ok &= CheckNoDelInForceComplete(type);
        ok &= CheckChainBypass(type);
        ok &= CheckNoLegacyTimerStack(type);

        if (ok)
        {
            Console.WriteLine("verify-il: ALL CHECKS PASSED.");
        }
        else
        {
            Console.Error.WriteLine("verify-il: FAILED — do not deploy.");
        }

        return ok;
    }

    private static bool CheckMarkers(string dllPath, bool clientMode)
    {
        var bytes = File.ReadAllBytes(dllPath);
        var hasChain = PatchMarkers.Contains(bytes, "bForceCompleteChain");

        if (clientMode)
        {
            if (PatchMarkers.Contains(bytes, "GetForceCompleteTarget") && hasChain)
            {
                Console.WriteLine("verify-il: markers OK (GetForceCompleteTarget, bForceCompleteChain).");
                return true;
            }
        }
        else if (PatchMarkers.Contains(bytes, "ForceCompleteV2") && hasChain)
        {
            Console.WriteLine("verify-il: markers OK (ForceCompleteV2, bForceCompleteChain).");
            return true;
        }

        Console.Error.WriteLine("verify-il: missing expected patch markers.");
        return false;
    }

    private static bool CheckRequiredMethods(TypeDefinition type, bool liteMode = false)
    {
        var required = liteMode
            ? LiteRequiredMethods
            : Program.ClientTransplantMethodNames;

        var missing = required.Where(name => type.Methods.All(m => m.Name != name)).ToList();
        if (missing.Count == 0)
        {
            Console.WriteLine("verify-il: required methods present.");
            return true;
        }

        Console.Error.WriteLine("verify-il: missing methods: " + string.Join(", ", missing));
        return false;
    }

    private static bool CheckTryForceCompletePath(TypeDefinition type, bool liteMode = false)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == "TryForceCompleteChainTask");
        if (method?.Body == null)
        {
            Console.Error.WriteLine("verify-il: TryForceCompleteChainTask has no body.");
            return false;
        }

        var callsTaskEnd = method.Body.Instructions.Any(i =>
            (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestForceCompleteTaskEnd");

        var callsEmit = method.Body.Instructions.Any(i =>
            (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
            i.Operand is MethodReference mr &&
            mr.Name == "ForceCompleteEmitTaskEndPacket");

        if (liteMode)
        {
            if (callsEmit && !callsTaskEnd)
            {
                Console.WriteLine("verify-il: TryForceCompleteChainTask uses ForceCompleteEmitTaskEndPacket.");
                return true;
            }

            var fct = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteCurrentTask");
            var fctCallsChain = fct?.Body?.Instructions.Any(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "TryForceCompleteChainTask") == true;
            if (fctCallsChain && callsEmit)
            {
                Console.WriteLine("verify-il: FCT -> TryForceCompleteChainTask -> emit helper.");
                return true;
            }

            if (!callsTaskEnd && !callsEmit)
            {
                Console.Error.WriteLine("verify-il: lite path must send from FCT or TryForceComplete.");
                return false;
            }

            Console.WriteLine("verify-il: TryForceCompleteChainTask uses RequestForceCompleteTaskEnd (lite legacy).");
            return true;
        }

        if (!callsEmit)
        {
            Console.Error.WriteLine("verify-il: client path must call ForceCompleteEmitTaskEndPacket.");
            return false;
        }

        if (callsTaskEnd)
        {
            Console.Error.WriteLine("verify-il: client path must NOT call RequestForceCompleteTaskEnd.");
            return false;
        }

        Console.WriteLine("verify-il: TryForceCompleteChainTask uses emit helper (client).");
        return true;
    }

    private static bool CheckNoDelInForceComplete(TypeDefinition type)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteCurrentTask");
        if (method?.Body == null)
        {
            return true;
        }

        if (!PatchV6.HasForceCompleteCheckerGateRemoved(method.Body))
        {
            Console.Error.WriteLine("verify-il: ForceCompleteCurrentTask still gated by SearchMissionTaskChecker.");
            return false;
        }

        var callsChain = method.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "TryForceCompleteChainTask");

        if (!callsChain)
        {
            Console.Error.WriteLine("verify-il: ForceCompleteCurrentTask must call TryForceCompleteChainTask.");
            return false;
        }

        Console.WriteLine("verify-il: ForceCompleteCurrentTask uses client chain entry.");
        return true;
    }

    private static bool CheckChainBypass(TypeDefinition type)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        if (method?.Body == null)
        {
            Console.Error.WriteLine("verify-il: RequestTaskComplete(3) missing.");
            return false;
        }

        var modulePath = type.Module.FileName;
        var hasStep8Rtc = !string.IsNullOrEmpty(modulePath) &&
                          PatchMarkers.Contains(File.ReadAllBytes(modulePath), "ForceCompleteV2: sent complete packet task ");
        if (!hasStep8Rtc && !PatchV6.HasChainDepthBypass(method.Body))
        {
            Console.Error.WriteLine("verify-il: RequestTaskComplete missing chain-depth / bForceCompleteChain bypass.");
            return false;
        }

        Console.WriteLine("verify-il: RequestTaskComplete chain bypass present.");
        return true;
    }

    private static bool CheckNoLegacyTimerStack(TypeDefinition type)
    {
        var update = type.Methods.FirstOrDefault(m => m.Name == "Update");
        if (update?.Body != null &&
            update.Body.Instructions.Any(i =>
                i.OpCode == OpCodes.Ldfld &&
                i.Operand is FieldReference fr &&
                fr.Name == "m_iForceCompletePendingTaskId"))
        {
            if (PatchCustomerUpdate.HasPendingTimerDefer(update.Body))
            {
                Console.WriteLine("verify-il: Update has surgical timer defer (PatchCustomerUpdate).");
                return true;
            }

            Console.Error.WriteLine("verify-il: Update references m_iForceCompletePendingTaskId (legacy defer stack).");
            return false;
        }

        Console.WriteLine("verify-il: no legacy timer/defer stack in Update.");
        return true;
    }

    /// <summary>
    /// Patched DLL must keep client Update IL identical — transplanting Update breaks load.
    /// </summary>
    public static bool VerifyLoadSafe(string patchedPath, string clientBasePath)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        clientBasePath = Path.GetFullPath(clientBasePath);

        using var patched = AssemblyDefinition.ReadAssembly(patchedPath);
        using var client = AssemblyDefinition.ReadAssembly(clientBasePath);

        var patchedType = patched.MainModule.Types.FirstOrDefault(t => t.Name == "cnMissionManager");
        var clientType = client.MainModule.Types.FirstOrDefault(t => t.Name == "cnMissionManager");
        if (patchedType == null || clientType == null)
        {
            Console.Error.WriteLine("verify-load-safe: cnMissionManager missing.");
            return false;
        }

        var patchedUpdate = patchedType.Methods.FirstOrDefault(m => m.Name == "Update");
        var clientUpdate = clientType.Methods.FirstOrDefault(m => m.Name == "Update");
        if (patchedUpdate?.Body == null || clientUpdate?.Body == null)
        {
            Console.Error.WriteLine("verify-load-safe: Update missing.");
            return false;
        }

        if (MethodBodiesEquivalent(clientUpdate.Body, patchedUpdate.Body))
        {
            Console.WriteLine($"verify-load-safe: Update IL identical to client base (MaxStack={clientUpdate.Body.MaxStackSize}).");
        }
        else if (PatchCustomerUpdate.HasPendingTimerDefer(patchedUpdate.Body) &&
                 PatchCustomerUpdate.HasHealthyMainGameMode(patchedUpdate.Body))
        {
            Console.WriteLine($"verify-load-safe: Update has surgical timer defer only (MaxStack={patchedUpdate.Body.MaxStackSize}).");
        }
        else
        {
            Console.Error.WriteLine("verify-load-safe: Update IL changed from client base — REJECT.");
            return false;
        }
        return VerifyNoDonorRefs(patchedPath);
    }

    /// <summary>
    /// Patched DLL must not reference DonorCompile — Unity cannot load that assembly.
    /// </summary>
    public static bool VerifyNoDonorRefs(string patchedPath)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        using var patched = AssemblyDefinition.ReadAssembly(patchedPath);

        foreach (var reference in patched.MainModule.AssemblyReferences)
        {
            if (reference.Name.IndexOf("DonorCompile", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.Error.WriteLine("verify-no-donor-refs: assembly reference to " + reference.Name + " — REJECT.");
                return false;
            }
        }

        Console.WriteLine("verify-no-donor-refs: no DonorCompile assembly reference.");
        return true;
    }

    public static bool VerifyInstanceCompletePath(string patchedPath)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        var bytes = File.ReadAllBytes(patchedPath);
        if (!PatchMarkers.Contains(bytes, "ForceCompleteV2: instance zone complete task "))
        {
            Console.Error.WriteLine("verify-safe-plus: RequestForceCompleteTaskEnd missing instance zone path.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteOnEndFail"))
        {
            Console.Error.WriteLine("verify-safe-plus: ForceCompleteOnEndFail missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteOnEndSucc"))
        {
            Console.Error.WriteLine("verify-safe-plus: ForceCompleteOnEndSucc missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "instance start fail complete task "))
        {
            Console.Error.WriteLine("verify-safe-plus: ForceCompleteOnStartFail instance path missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteV2: sent complete packet task "))
        {
            Console.Error.WriteLine("verify-safe-plus: RequestTaskComplete step8 diagnostics missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteV2: RequestTaskComplete enter task "))
        {
            Console.Error.WriteLine("verify-safe-plus: RequestTaskComplete enter log missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, " chain "))
        {
            Console.Error.WriteLine("verify-safe-plus: RequestTaskComplete chain diagnostic missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteV2: patch build 2026-06-07-fct-restore"))
        {
            Console.Error.WriteLine("verify-safe-plus: FCT restore build marker missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteV2: host ProcessEndSucc task "))
        {
            Console.Error.WriteLine("verify-safe-plus: host ProcessEndSucc diag missing.");
            return false;
        }

        if (!PatchMarkers.Contains(bytes, "ForceCompleteV2: host ProcessEndFail task "))
        {
            Console.Error.WriteLine("verify-safe-plus: host ProcessEndFail diag missing.");
            return false;
        }

        if (!VerifyDirectSendPath(patchedPath))
        {
            return false;
        }

        using (var asm = AssemblyDefinition.ReadAssembly(patchedPath))
        {
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            if (!CheckRequestTaskCompleteBoolArg(type))
            {
                return false;
            }
        }

        Console.WriteLine("verify-safe-plus: instance path + chain handlers present.");
        return true;
    }

    /// <summary>
    /// DonorCompile can emit ldc.i4.0 before clt when bool is inlined as a call arg — breaks the stack.
    /// </summary>
    private static bool CheckRequestTaskCompleteBoolArg(TypeDefinition type)
    {
        var broken = false;
        foreach (var method in type.Methods.Where(m => m.Body != null))
        {
            var ins = method.Body.Instructions;
            for (var i = 0; i < ins.Count; i++)
            {
                if (ins[i].OpCode != OpCodes.Call ||
                    ins[i].Operand is not MethodReference mr ||
                    mr.Name != "RequestTaskComplete" ||
                    mr.Parameters.Count != 3)
                {
                    continue;
                }

                if (i < 2)
                {
                    continue;
                }

                var thirdArg = ins[i - 1];
                var secondArg = ins[i - 2];
                if (secondArg.OpCode != OpCodes.Ldc_I4_0 && secondArg.OpCode != OpCodes.Ldc_I4)
                {
                    continue;
                }

                if (thirdArg.OpCode == OpCodes.Ldc_I4_0)
                {
                    var prev = ins[i - 3];
                    if (prev.OpCode == OpCodes.Clt || prev.OpCode == OpCodes.Cgt)
                    {
                        Console.Error.WriteLine(
                            $"verify-il: {method.Name} RequestTaskComplete 3rd arg is ldc.i4.0 after compare — use bool local.");
                        broken = true;
                    }
                }
            }
        }

        if (broken)
        {
            return false;
        }

        Console.WriteLine("verify-il: RequestTaskComplete bool args look sane.");
        return true;
    }

    public static bool VerifyDirectSendPath(string patchedPath)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        var bytes = File.ReadAllBytes(patchedPath);
        using var asm = AssemblyDefinition.ReadAssembly(patchedPath);
        var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
        var emit = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteEmitTaskEndPacket");
        if (emit?.Body == null)
        {
            Console.Error.WriteLine("verify-direct-send: ForceCompleteEmitTaskEndPacket missing.");
            return false;
        }

        if (!emit.Body.Instructions.Any(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "SendPacket"))
        {
            Console.Error.WriteLine("verify-direct-send: emit helper must call SendPacket.");
            return false;
        }

        var taskEnd = type.Methods.First(m => m.Name == "RequestForceCompleteTaskEnd");
        var callsEmit = taskEnd.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "ForceCompleteEmitTaskEndPacket");

        if (!callsEmit)
        {
            Console.Error.WriteLine("verify-direct-send: RequestForceCompleteTaskEnd must call ForceCompleteEmitTaskEndPacket.");
            return false;
        }

        var fct = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var fctCallsChain = fct.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "TryForceCompleteChainTask");
        if (!fctCallsChain)
        {
            Console.Error.WriteLine("verify-direct-send: ForceCompleteCurrentTask must call TryForceCompleteChainTask.");
            return false;
        }

        Console.WriteLine("verify-direct-send: FCT -> chain -> emit helper.");
        return true;
    }

    private static bool MethodBodiesEquivalent(MethodBody a, MethodBody b)
    {
        var aIns = a.Instructions;
        var bIns = b.Instructions;
        if (aIns.Count != bIns.Count)
        {
            return false;
        }

        for (var i = 0; i < aIns.Count; i++)
        {
            var ai = aIns[i];
            var bi = bIns[i];
            if (ai.OpCode != bi.OpCode)
            {
                return false;
            }

            if (!OperandsEquivalent(aIns, bIns, ai, bi))
            {
                return false;
            }
        }

        return true;
    }

    private static bool OperandsEquivalent(
        IList<Instruction> aIns,
        IList<Instruction> bIns,
        Instruction ai,
        Instruction bi)
    {
        if (ai.Operand is Instruction aTarget)
        {
            return bi.Operand is Instruction bTarget &&
                   InstructionIndex(aIns, aTarget) == InstructionIndex(bIns, bTarget);
        }

        if (ai.Operand is Instruction[] aSwitch)
        {
            if (bi.Operand is not Instruction[] bSwitch || aSwitch.Length != bSwitch.Length)
            {
                return false;
            }

            for (var i = 0; i < aSwitch.Length; i++)
            {
                if (InstructionIndex(aIns, aSwitch[i]) != InstructionIndex(bIns, bSwitch[i]))
                {
                    return false;
                }
            }

            return true;
        }

        if (ai.Operand is MethodReference am && bi.Operand is MethodReference bm)
        {
            return am.Name == bm.Name && am.DeclaringType.Name == bm.DeclaringType.Name;
        }

        if (ai.Operand is FieldReference af && bi.Operand is FieldReference bf)
        {
            return af.Name == bf.Name;
        }

        if (ai.Operand is TypeReference at && bi.Operand is TypeReference bt)
        {
            return at.Name == bt.Name;
        }

        if (ai.Operand is VariableDefinition av && bi.Operand is VariableDefinition bv)
        {
            return av.Index == bv.Index;
        }

        if (ai.Operand is ParameterDefinition ap && bi.Operand is ParameterDefinition bp)
        {
            return ap.Index == bp.Index;
        }

        return Equals(ai.Operand, bi.Operand);
    }

    private static int InstructionIndex(IList<Instruction> list, Instruction target)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] == target)
            {
                return i;
            }
        }

        return -1;
    }
}

/// <summary>
/// Search raw PE bytes for metadata (#Strings UTF-8) and user-string (#US UTF-16 LE) literals.
/// Decoding the whole file as UTF-16 misses ldstr operands that are not WCHAR-aligned in the image.
/// </summary>
internal static class PatchMarkers
{
    public static bool Contains(byte[] haystack, string needle)
    {
        var ascii = Encoding.ASCII.GetBytes(needle);
        if (IndexOf(haystack, ascii) >= 0)
        {
            return true;
        }

        var utf16 = Encoding.Unicode.GetBytes(needle);
        return IndexOf(haystack, utf16) >= 0;
    }

    private static int IndexOf(byte[] haystack, byte[] needle)
    {
        if (needle.Length == 0 || haystack.Length < needle.Length)
        {
            return -1;
        }

        for (var i = 0; i <= haystack.Length - needle.Length; i++)
        {
            var match = true;
            for (var j = 0; j < needle.Length; j++)
            {
                if (haystack[i + j] != needle[j])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return i;
            }
        }

        return -1;
    }
}
