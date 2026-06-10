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
        "ForceCompleteCurrentTask"
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

    private static readonly string[] Doc504BuildMarkers =
    {
        "ForceCompleteV2: patch build 2026-06-08-doc504",
        "ForceCompleteV2: patch build 2026-06-09-doc504b",
        "ForceCompleteV2: patch build 2026-06-09-doc504e",
        "ForceCompleteV2: patch build 2026-06-09-doc504f",
        "ForceCompleteV2: patch build 2026-06-09-doc504g",
        "ForceCompleteV2: patch build 2026-06-09-doc504h",
    };

    private static bool HasDoc504BuildMarker(byte[] bytes) =>
        Doc504BuildMarkers.Any(m => PatchMarkers.Contains(bytes, m));

    private static bool CheckMarkers(string dllPath, bool clientMode)
    {
        var bytes = File.ReadAllBytes(dllPath);
        var hasChain = PatchMarkers.Contains(bytes, "m_iForceCompleteChainDepth")
            || PatchMarkers.Contains(bytes, "bForceCompleteChain")
            || PatchMarkers.Contains(bytes, "m_iloopTemp");

        if (clientMode)
        {
            if (HasDoc504BuildMarker(bytes) && hasChain)
            {
                Console.WriteLine("verify-il: markers OK (doc504 bootstrap, chain depth field).");
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
        if (liteMode)
        {
            var fctBootstrap = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteCurrentTask");
            if (fctBootstrap?.Body != null)
            {
                var fctBody = fctBootstrap.Body;
                if (fctBody.Instructions.Any(i =>
                        i.OpCode == OpCodes.Call &&
                        i.Operand is MethodReference mr &&
                        mr.Name == "RequestTaskComplete"))
                {
                    var loadsTerminatorNpc = fctBody.Instructions.Any(i =>
                        i.OpCode == OpCodes.Ldfld &&
                        i.Operand is FieldReference fr &&
                        fr.Name == "m_iHTerminatorNPCID");
                    var usesNpcZeroRtc = fctBody.Instructions.Any(i =>
                        i.OpCode == OpCodes.Call &&
                        i.Operand is MethodReference mr &&
                        mr.Name == "RequestTaskComplete" &&
                        i.Previous?.OpCode == OpCodes.Ldc_I4_0 &&
                        i.Previous.Previous?.OpCode == OpCodes.Ldc_I4_0);
                    Console.WriteLine(loadsTerminatorNpc && !usesNpcZeroRtc
                        ? "verify-il: FCT -> terminator NPC RTC (doc504h)."
                        : "verify-il: FCT -> RequestTaskComplete (bootstrap).");
                    return true;
                }
            }
        }

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

        var callsRtc = method.Body.Instructions.Any(i =>
            (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete");

        var callsDocComplete = method.Body.Instructions.Any(i =>
            (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
            i.Operand is MethodReference mr &&
            mr.Name == "EmitDocCompletePacket");

        if (liteMode)
        {
            var fct = type.Methods.FirstOrDefault(m => m.Name == "ForceCompleteCurrentTask");
            var fctCallsEmit = fct?.Body?.Instructions.Any(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "EmitDocCompletePacket") == true;
            if (fctCallsEmit)
            {
                Console.WriteLine("verify-il: FCT -> EmitDocCompletePacket (bootstrap).");
                return true;
            }

            if (callsDocComplete && !callsTaskEnd)
            {
                Console.WriteLine("verify-il: TryForceCompleteChainTask uses doc-complete / emit helper.");
                return true;
            }

            var fctCallsChain = fct?.Body?.Instructions.Any(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "TryForceCompleteChainTask") == true;
            if (fctCallsChain && callsDocComplete)
            {
                Console.WriteLine("verify-il: FCT -> TryForceCompleteChainTask -> doc-complete.");
                return true;
            }

            Console.Error.WriteLine("verify-il: lite path must send from FCT or TryForceComplete.");
            return false;
        }

        if (!callsDocComplete)
        {
            Console.Error.WriteLine("verify-il: client path must call EmitDocCompletePacket.");
            return false;
        }

        if (callsTaskEnd)
        {
            Console.Error.WriteLine("verify-il: client path must NOT call RequestForceCompleteTaskEnd.");
            return false;
        }

        Console.WriteLine("verify-il: TryForceCompleteChainTask uses doc-complete helper (client).");
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

        var callsRtc = method.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete");
        var callsEmit = method.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "EmitDocCompletePacket");
        var callsChain = method.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "TryForceCompleteChainTask");

        if (!callsRtc && !callsEmit && !callsChain)
        {
            Console.Error.WriteLine("verify-il: ForceCompleteCurrentTask must call RequestTaskComplete, EmitDocCompletePacket, or TryForceCompleteChainTask.");
            return false;
        }

        Console.WriteLine(callsRtc
            ? "verify-il: ForceCompleteCurrentTask uses bootstrap direct send."
            : callsEmit
                ? "verify-il: ForceCompleteCurrentTask uses bootstrap emit entry."
                : "verify-il: ForceCompleteCurrentTask uses client chain entry.");
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

    public static bool VerifyBootstrapPath(string patchedPath)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        var bytes = File.ReadAllBytes(patchedPath);
        if (!HasDoc504BuildMarker(bytes))
        {
            Console.Error.WriteLine("verify-bootstrap: FCT doc504 build marker missing.");
            return false;
        }

        using var asm = AssemblyDefinition.ReadAssembly(patchedPath);
        var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
        var rtc = type.Methods.FirstOrDefault(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        if (rtc?.Body == null || !PatchV6.HasChainDepthBypass(rtc.Body))
        {
            Console.Error.WriteLine("verify-bootstrap: RequestTaskComplete chain bypass missing.");
            return false;
        }

        var fct = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        if (!fct.Body.Instructions.Any(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "RequestTaskComplete"))
        {
            Console.Error.WriteLine("verify-bootstrap: ForceCompleteCurrentTask must call RequestTaskComplete.");
            return false;
        }

        if (!CheckRequestTaskCompleteBoolArg(type))
        {
            return false;
        }

        var slotBytes = 1520640;
        var size = bytes.Length;
        if (size != slotBytes)
        {
            Console.Error.WriteLine($"verify-bootstrap: DLL size {size} != client slot {slotBytes}.");
            return false;
        }

        Console.WriteLine("verify-bootstrap: bootstrap path + slot size OK.");
        return true;
    }

    public static bool VerifyInstanceCompletePath(string patchedPath)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        var bytes = File.ReadAllBytes(patchedPath);

        if (!HasDoc504BuildMarker(bytes))
        {
            Console.Error.WriteLine("verify-safe-plus: FCT doc504 build marker missing.");
            return false;
        }

        using var asm = AssemblyDefinition.ReadAssembly(patchedPath);
        var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");

        var startSuccBody = type.Methods.FirstOrDefault(m => m.Name == "ProcessStartSucc")?.Body;
        var hasStartSuccBlock = startSuccBody?.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "IsExistTaskInActiveMission") == true
            && startSuccBody.Instructions.Any(i =>
                i.OpCode == OpCodes.Ldc_I4 &&
                i.Operand is int n &&
                n == 466);

        var hasEndSuccHook = CheckProcessEndSuccInlineChain(type);
        if (!hasEndSuccHook)
        {
            Console.WriteLine("verify-safe-plus: FCT-only tier (no EndSucc auto-advance; avoids instance 463 overworld emit).");
        }

        var rtc = type.Methods.FirstOrDefault(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        if (rtc?.Body == null || !PatchV6.HasChainDepthBypass(rtc.Body))
        {
            Console.Error.WriteLine("verify-safe-plus: RequestTaskComplete surgical bypass missing.");
            return false;
        }

        Console.WriteLine("verify-safe-plus: RequestTaskComplete uses surgical chain bypass.");

        if (!VerifyDirectSendPath(patchedPath))
        {
            return false;
        }

        if (!CheckRequestTaskCompleteBoolArg(type))
        {
            return false;
        }

        if (hasStartSuccBlock == true)
        {
            Console.WriteLine("verify-safe-plus: ProcessStartSucc stale-466 block present.");
        }

        var hasDeferred = !PatchMarkers.Contains(bytes, "instance start fail complete task ");
        var tier = hasEndSuccHook ? "FCT+RTC+EndSucc" : hasStartSuccBlock == true ? "FCT+RTC+StartSucc" : "FCT+RTC hotkey";
        Console.WriteLine(hasDeferred
            ? $"verify-safe-plus: mission-minimal tier OK ({tier}; Start*/EndFail deferred)."
            : "verify-safe-plus: full instance + chain handlers present.");
        return true;
    }

    private static bool CheckProcessEndSuccInlineChain(TypeDefinition type)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == "ProcessEndSucc");
        if (method?.Body == null)
        {
            return false;
        }

        var body = method.Body;
        var hasLoopGate = body.Instructions.Any(i =>
            i.OpCode == OpCodes.Ldfld &&
            i.Operand is FieldReference fr &&
            fr.Name == "m_iloopTemp");
        var hasNext = body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "GetNextTaskNode");
        var hasEmitRtc = body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete" &&
            mr.Parameters.Count == 3);

        return hasLoopGate && hasNext && hasEmitRtc;
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
        using var asm = AssemblyDefinition.ReadAssembly(patchedPath);
        var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
        var fct = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        var fctCallsRtc = fct.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "RequestTaskComplete");
        var fctCallsChain = fct.Body.Instructions.Any(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "TryForceCompleteChainTask");

        if (fctCallsChain)
        {
            var emitDoc = type.Methods.FirstOrDefault(m => m.Name == "EmitDocCompletePacket");
            if (emitDoc?.Body == null)
            {
                Console.Error.WriteLine("verify-direct-send: EmitDocCompletePacket missing.");
                return false;
            }

            if (!emitDoc.Body.Instructions.Any(i =>
                    i.OpCode == OpCodes.Call &&
                    i.Operand is MethodReference mr &&
                    mr.Name == "RequestTaskComplete"))
            {
                Console.Error.WriteLine("verify-direct-send: EmitDocCompletePacket must call RequestTaskComplete.");
                return false;
            }

            Console.WriteLine("verify-direct-send: FCT -> chain -> emit helper.");
            return true;
        }

        if (fctCallsRtc)
        {
            var endSuccHook = CheckProcessEndSuccInlineChain(type);
            Console.WriteLine(endSuccHook
                ? "verify-direct-send: FCT -> RTC + ProcessEndSucc inline emit."
                : "verify-direct-send: FCT -> RTC (per-task doc emit on hotkey).");
            return true;
        }

        Console.Error.WriteLine("verify-direct-send: FCT must call RequestTaskComplete or TryForceCompleteChainTask.");
        return false;
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
