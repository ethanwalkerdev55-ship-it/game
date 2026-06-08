using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace FFPatch;

internal static class Program
{
    internal static readonly string[] ClientTransplantMethodNames =
    {
        "ForceCompleteCurrentTask",
        "GetForceCompleteTarget",
        "PrepareTaskForForceComplete",
        "ResolveForceCompleteTerminatorNpcId",
        "ClearForceCompleteChain",
        "TryForceCompleteChainTask"
    };

    internal static readonly string[] ClientFieldNames =
    {
        "bForceCompleteChain",
        "m_iForceCompleteChainDepth"
    };

    static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            PrintUsage();
            return 1;
        }

        var command = args[0].ToLowerInvariant();
        if (command is "help" or "-h" or "--help")
        {
            PrintUsage();
            return 0;
        }

        return command switch
        {
            "apply-client-safe" => RequireArgs(args, 3, "apply-client-safe requires target.dll and client-base.dll paths.")
                ? (BuildClientMissionSafe.Apply(args[1], args[2]) ? 0 : 1)
                : 1,
            "verify" => RequireArgs(args, 2, "verify requires dll path.")
                ? (VerifyMarkers(args[1]) ? 0 : 1)
                : 1,
            "verify-il" => RequireArgs(args, 2, "verify-il requires dll path.")
                ? (PatchVerify.Run(args[1], liteMode: args.Any(a => a.Equals("--lite", StringComparison.OrdinalIgnoreCase))) ? 0 : 1)
                : 1,
            "verify-load-safe" => RequireArgs(args, 3, "verify-load-safe requires patched.dll and client-base.dll paths.")
                ? (PatchVerify.VerifyLoadSafe(args[1], args[2]) ? 0 : 1)
                : 1,
            "verify-no-donor-refs" => RequireArgs(args, 2, "verify-no-donor-refs requires patched.dll path.")
                ? (PatchVerify.VerifyNoDonorRefs(args[1]) ? 0 : 1)
                : 1,
            "verify-safe-plus" => RequireArgs(args, 2, "verify-safe-plus requires patched.dll path.")
                ? (PatchVerify.VerifyInstanceCompletePath(args[1]) ? 0 : 1)
                : 1,
            "verify-direct-send" => RequireArgs(args, 2, "verify-direct-send requires patched.dll path.")
                ? (PatchVerify.VerifyDirectSendPath(args[1]) ? 0 : 1)
                : 1,
            "patch-safe-client" => RequireArgs(args, 2, "patch-safe-client requires dll path.")
                ? (PatchSafeClient.Apply(args[1]) ? 0 : 1)
                : 1,
            _ => UnknownCommand(command)
        };
    }

    private static bool RequireArgs(string[] args, int count, string message)
    {
        if (args.Length >= count)
        {
            return true;
        }

        Console.Error.WriteLine(message);
        return false;
    }

    private static int UnknownCommand(string command)
    {
        Console.Error.WriteLine("Unknown command: " + command);
        PrintUsage();
        return 1;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("FusionFall mission patch — SAFE client pipeline");
        Console.WriteLine();
        Console.WriteLine("  apply-client-safe <target.dll> <client-base.dll>");
        Console.WriteLine("  verify <dll>");
        Console.WriteLine("  verify-il <dll> [--lite]");
        Console.WriteLine("  verify-load-safe <patched.dll> <client-base.dll>");
        Console.WriteLine("  verify-no-donor-refs <patched.dll>");
        Console.WriteLine("  verify-safe-plus <patched.dll>");
        Console.WriteLine("  verify-direct-send <patched.dll>");
        Console.WriteLine("  patch-safe-client <dll>");
    }

    internal static bool VerifyMarkers(string dllPath) => Verify(dllPath);

    internal static void CopyMethodBody(MethodDefinition source, MethodDefinition target, ModuleDefinition module)
    {
        IlTransplant.CopyMethodBody(source, target, module);
    }

    private static bool Verify(string dllPath)
    {
        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            Console.Error.WriteLine("Not found: " + dllPath);
            return false;
        }

        var bytes = File.ReadAllBytes(dllPath);
        if (PatchMarkers.Contains(bytes, "ForceCompleteV2") ||
            PatchMarkers.Contains(bytes, "GetForceCompleteTarget") ||
            PatchMarkers.Contains(bytes, "bForceCompleteChain"))
        {
            Console.WriteLine("Patch markers found in " + Path.GetFileName(dllPath));
            return true;
        }

        Console.Error.WriteLine("Patch markers not found in " + dllPath);
        return false;
    }
}
