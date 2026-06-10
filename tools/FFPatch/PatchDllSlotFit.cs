using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Shrink patched DLL to fit the client UnityWeb slot (1520640) so preserve-inject keeps u= offset map stable.
/// </summary>
internal static class PatchDllSlotFit
{
    private static readonly string[] KeepStringPrefixes =
    {
        "ForceCompleteV2: patch build 2026-06-08-doc504",
        "ForceCompleteV2: patch build 2026-06-09-doc504b",
        "ForceCompleteV2: patch build 2026-06-09-doc504d",
        "ForceCompleteV2: patch build 2026-06-09-doc504e",
        "ForceCompleteV2: patch build 2026-06-09-doc504g",
        "ForceCompleteV2: patch build 2026-06-09-doc504h",
    };

    private static readonly string[] PatchMethodNames =
    {
        "ForceCompleteCurrentTask",
        "GetForceCompleteTarget",
        "ClearForceCompleteChain",
        "EmitDocCompletePacket",
        "RequestForceCompleteTaskEnd",
        "TryForceCompleteChainTask",
        "ForceCompleteChainHook",
        "RequestTaskComplete"
    };

    public static bool Fit(string dllPath, int slotBytes, string? clientTemplatePath = null)
    {
        dllPath = Path.GetFullPath(dllPath);
        var before = new FileInfo(dllPath).Length;
        if (before <= slotBytes)
        {
            PadFile(dllPath, slotBytes);
            Console.WriteLine($"dll-slot-fit: {before} bytes fits slot {slotBytes} (padded).");
            return true;
        }

        CompactLogStrings(dllPath);
        StripOptionalLoggerCalls(dllPath);
        RewriteWithoutDebugMetadata(dllPath);

        var afterCompact = new FileInfo(dllPath).Length;
        if (afterCompact > slotBytes && !string.IsNullOrEmpty(clientTemplatePath))
        {
            afterCompact = PeShellSlotFit.TryFit(dllPath, clientTemplatePath, slotBytes) ?? afterCompact;
        }

        if (afterCompact > slotBytes)
        {
            Console.Error.WriteLine(
                $"dll-slot-fit: FAIL {afterCompact} bytes still exceeds client slot {slotBytes} (was {before}, need -{afterCompact - slotBytes}).");
            return false;
        }

        PadFile(dllPath, slotBytes);
        Console.WriteLine($"dll-slot-fit: OK {before} -> {slotBytes} bytes (compact {before - afterCompact}, pad {slotBytes - afterCompact}).");
        return true;
    }

    private static void CompactLogStrings(string dllPath)
    {
        var temp = dllPath + ".compact.tmp";
        using (var asm = AssemblyDefinition.ReadAssembly(dllPath))
        {
            var type = asm.MainModule.Types.FirstOrDefault(t => t.Name == "cnMissionManager");
            if (type == null)
            {
                return;
            }

            var changed = 0;
            foreach (var method in type.Methods.Where(m => m.Body != null))
            {
                foreach (var ins in method.Body.Instructions)
                {
                    if (ins.OpCode != OpCodes.Ldstr || ins.Operand is not string text)
                    {
                        continue;
                    }

                    if (ShouldKeepString(text))
                    {
                        continue;
                    }

                    if (!text.StartsWith("ForceCompleteV2:", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    ins.Operand = "FC";
                    changed++;
                }
            }

            asm.Write(temp);
            Console.WriteLine($"dll-slot-fit: compacted {changed} log strings to FC.");
        }

        File.Copy(temp, dllPath, true);
        File.Delete(temp);
    }

    private static void StripOptionalLoggerCalls(string dllPath)
    {
        var temp = dllPath + ".strip.tmp";
        using (var asm = AssemblyDefinition.ReadAssembly(dllPath))
        {
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            var removed = 0;

            foreach (var method in type.Methods.Where(m => m.Body != null && PatchMethodNames.Any(n => n == m.Name)))
            {
                removed += StripLoggerCallsInMethod(method);
                IlStackHelper.RefreshMaxStack(method.Body);
            }

            asm.Write(temp);
            Console.WriteLine($"dll-slot-fit: stripped {removed} optional Logger.Log calls.");
        }

        File.Copy(temp, dllPath, true);
        File.Delete(temp);
    }

    private static int StripLoggerCallsInMethod(MethodDefinition method)
    {
        var body = method.Body;
        var il = body.GetILProcessor();
        var removed = 0;

        for (var i = 0; i < body.Instructions.Count; i++)
        {
            var ins = body.Instructions[i];
            if (ins.OpCode != OpCodes.Call && ins.OpCode != OpCodes.Callvirt)
            {
                continue;
            }

            if (ins.Operand is not MethodReference mr || mr.Name != "Log" || mr.Parameters.Count != 1)
            {
                continue;
            }

            if (!IsLoggerReference(mr))
            {
                continue;
            }

            var ldstrIndex = FindLeadingLdstrIndex(body, i);
            if (ldstrIndex < 0)
            {
                continue;
            }

            var text = body.Instructions[ldstrIndex].Operand as string;
            if (text != null && ShouldKeepString(text))
            {
                continue;
            }

            if (text == "FC")
            {
                il.Remove(ins);
                il.Remove(body.Instructions[ldstrIndex]);
                removed++;
                i = Math.Max(0, ldstrIndex - 1);
            }
        }

        return removed;
    }

    private static int FindLeadingLdstrIndex(MethodBody body, int logCallIndex)
    {
        for (var j = logCallIndex - 1; j >= 0 && j >= logCallIndex - 6; j--)
        {
            if (body.Instructions[j].OpCode == OpCodes.Ldstr)
            {
                return j;
            }

            if (body.Instructions[j].OpCode == OpCodes.Call ||
                body.Instructions[j].OpCode == OpCodes.Callvirt)
            {
                break;
            }
        }

        return -1;
    }

    private static bool IsLoggerReference(MethodReference mr) =>
        mr.DeclaringType.Name == "Logger";

    private static bool ShouldKeepString(string text)
    {
        foreach (var prefix in KeepStringPrefixes)
        {
            if (text.StartsWith(prefix, StringComparison.Ordinal) || text == prefix)
            {
                return true;
            }
        }

        return false;
    }

    private static void RewriteWithoutDebugMetadata(string dllPath)
    {
        var temp = dllPath + ".rewrite.tmp";
        using (var asm = AssemblyDefinition.ReadAssembly(dllPath))
        {
            foreach (var type in asm.MainModule.Types)
            {
                foreach (var method in type.Methods)
                {
                    method.DebugInformation.Scope = null;
                }
            }

            asm.Write(temp, new WriterParameters { WriteSymbols = false });
        }

        File.Copy(temp, dllPath, true);
        File.Delete(temp);
    }

    private static void PadFile(string path, int slotBytes)
    {
        var bytes = File.ReadAllBytes(path);
        if (bytes.Length == slotBytes)
        {
            return;
        }

        if (bytes.Length > slotBytes)
        {
            throw new InvalidOperationException($"Cannot pad {bytes.Length} to slot {slotBytes}");
        }

        Array.Resize(ref bytes, slotBytes);
        File.WriteAllBytes(path, bytes);
    }
}
