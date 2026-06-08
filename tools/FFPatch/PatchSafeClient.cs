using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace FFPatch;

/// <summary>
/// Post-apply hotfixes for the client-mod donor path (main.current lineage).
/// Only chain-bypass in RequestTaskComplete — no timer/defer/legacy V9 stack.
/// </summary>
internal static class PatchSafeClient
{
    public static bool Apply(string dllPath)
    {
        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            Console.Error.WriteLine("DLL not found: " + dllPath);
            return false;
        }

        var tempPath = dllPath + ".client-safe.tmp";
        var bytes = File.ReadAllBytes(dllPath);
        var hasStep8Rtc = PatchMarkers.Contains(bytes, "ForceCompleteV2: sent complete packet task ");
        using (var asm = AssemblyDefinition.ReadAssembly(dllPath))
        {
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            PatchV6.PatchForceCompleteSkipCheckerGate(type);
            if (hasStep8Rtc)
            {
                Console.WriteLine("RequestTaskComplete: step8 transplant present — skip IL chain bypass (avoids corrupting RTC).");
            }
            else
            {
                PatchV6.PatchRequestTaskCompleteChainBypass(type);
            }

            asm.Write(tempPath);
        }

        File.Copy(tempPath, dllPath, true);
        File.Delete(tempPath);
        Console.WriteLine("Client safe hotfix OK (RequestTaskComplete chain bypass).");
        return true;
    }

    /// <summary>
    /// Post-inject fixes for full customer golden (ForceCompleteV2 stack).
    /// </summary>
    public static bool ApplyFull(string dllPath)
    {
        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            Console.Error.WriteLine("DLL not found: " + dllPath);
            return false;
        }

        var tempPath = dllPath + ".customer-full.tmp";
        using (var asm = AssemblyDefinition.ReadAssembly(dllPath))
        {
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            PatchV6.PatchForceCompleteRemoveDelChecker(type);
            PatchV6.PatchRequestTaskCompleteChainBypass(type);
            asm.Write(tempPath);
        }

        File.Copy(tempPath, dllPath, true);
        File.Delete(tempPath);
        Console.WriteLine("Customer full safe hotfix OK.");
        return true;
    }
}
