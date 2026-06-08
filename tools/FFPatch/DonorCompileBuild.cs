using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FFPatch;

/// <summary>Build DonorCompile.dll from mods/patches step sources.</summary>
internal static class DonorCompileBuild
{
    internal static string ResolveDonorDll()
    {
        var candidates = new[]
        {
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "DonorCompile", "bin", "Release", "net48", "DonorCompile.dll")),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "DonorCompile", "bin", "Release", "DonorCompile.dll"))
        };

        return candidates.FirstOrDefault(File.Exists) ?? candidates[0];
    }

    internal static bool Run()
    {
        var script = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "DonorCompile", "build-donor.ps1"));

        if (File.Exists(script))
        {
            Console.WriteLine("Regenerating PatchedDonor.g.cs...");
            var gen = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-NoProfile -ExecutionPolicy Bypass -File \"" + script + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var genProcess = Process.Start(gen);
            if (genProcess != null)
            {
                var genOut = genProcess.StandardOutput.ReadToEnd();
                var genErr = genProcess.StandardError.ReadToEnd();
                genProcess.WaitForExit();
                if (!string.IsNullOrWhiteSpace(genOut))
                {
                    Console.WriteLine(genOut.TrimEnd());
                }

                if (genProcess.ExitCode != 0)
                {
                    Console.Error.WriteLine(genErr.TrimEnd());
                    return false;
                }
            }
        }

        var project = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "DonorCompile", "DonorCompile.csproj"));

        Console.WriteLine("Building DonorCompile...");
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "build \"" + project + "\" -c Release -v q",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start(psi);
        if (process == null)
        {
            return false;
        }

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (!string.IsNullOrWhiteSpace(stdout))
        {
            Console.WriteLine(stdout.TrimEnd());
        }

        if (process.ExitCode != 0)
        {
            Console.Error.WriteLine(stderr.TrimEnd());
            return false;
        }

        return true;
    }
}
