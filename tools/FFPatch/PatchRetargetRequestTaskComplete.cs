using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Helper methods cloned before RTC transplant may call a stale MethodReference.
/// Retarget every RequestTaskComplete(3) call to the live method on cnMissionManager.
/// </summary>
internal static class PatchRetargetRequestTaskCompleteCalls
{
    internal static void Apply(TypeDefinition type)
    {
        var rtc = type.Methods.FirstOrDefault(m =>
            m.Name == "RequestTaskComplete" && m.Parameters.Count == 3 && !m.HasGenericParameters);
        if (rtc == null)
        {
            throw new System.InvalidOperationException("RequestTaskComplete(3) missing for retarget.");
        }

        var rtcRef = type.Module.ImportReference(rtc);
        var fixedCount = 0;

        foreach (var method in type.Methods)
        {
            if (method.Body == null)
            {
                continue;
            }

            foreach (var ins in method.Body.Instructions)
            {
                if (ins.OpCode != OpCodes.Call && ins.OpCode != OpCodes.Callvirt)
                {
                    continue;
                }

                if (ins.Operand is not MethodReference mr || mr.Name != "RequestTaskComplete")
                {
                    continue;
                }

                if (mr.Parameters.Count != 3)
                {
                    continue;
                }

                if (ReferenceEquals(mr, rtcRef) || ReferenceEquals(mr.Resolve(), rtc))
                {
                    continue;
                }

                ins.OpCode = OpCodes.Call;
                ins.Operand = rtcRef;
                fixedCount++;
            }
        }

        Console.WriteLine($"RequestTaskComplete: retargeted {fixedCount} helper call site(s).");
    }
}
