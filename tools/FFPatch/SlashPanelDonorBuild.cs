using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FFPatch;

internal static class SlashPanelDonorBuild
{
    internal static string ResolveDonorDll()
    {
        var candidates = new[]
        {
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "SlashPanelDonor", "bin", "Release", "net48", "SlashPanelDonor.dll")),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "SlashPanelDonor", "bin", "Release", "SlashPanelDonor.dll"))
        };

        return candidates.FirstOrDefault(File.Exists) ?? candidates[0];
    }

    internal static bool Run()
    {
        var project = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "SlashPanelDonor", "SlashPanelDonor.csproj"));

        Console.WriteLine("Building SlashPanelDonor...");
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
