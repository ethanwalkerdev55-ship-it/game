using System;
using System.IO;
using System.Linq;

namespace FFPatch;

/// <summary>
/// Last-resort fit: repack Cecil output into the client PE section shell so bundle slot size stays stable.
/// </summary>
internal static class PeShellSlotFit
{
    public static long? TryFit(string patchedPath, string clientTemplatePath, int slotBytes)
    {
        patchedPath = Path.GetFullPath(patchedPath);
        clientTemplatePath = Path.GetFullPath(clientTemplatePath);

        var patched = File.ReadAllBytes(patchedPath);
        var client = File.ReadAllBytes(clientTemplatePath);
        if (patched.Length <= slotBytes)
        {
            return patched.Length;
        }

        var patchedSections = ReadSections(patched);
        var clientSections = ReadSections(client);
        if (patchedSections.Length == 0 || clientSections.Length == 0)
        {
            return null;
        }

        var patchedText = patchedSections.FirstOrDefault(s => s.Name == ".text");
        var clientText = clientSections.FirstOrDefault(s => s.Name == ".text");
        if (patchedText == null || clientText == null)
        {
            return null;
        }

        var patchedTextBytes = Slice(patched, patchedText);
        var copyLen = Math.Min(patchedTextBytes.Length, clientText.RawSize);
        if (patchedTextBytes.Length > clientText.RawSize)
        {
            Console.WriteLine(
                $"pe-shell-fit: truncating patched .text {patchedTextBytes.Length} -> client slot {clientText.RawSize}.");
        }

        var output = (byte[])client.Clone();
        Array.Copy(patchedTextBytes, 0, output, clientText.RawPtr, copyLen);
        if (copyLen < clientText.RawSize)
        {
            Array.Clear(output, clientText.RawPtr + copyLen, clientText.RawSize - copyLen);
        }

        var patchedRsrc = patchedSections.FirstOrDefault(s => s.Name == ".rsrc");
        var clientRsrc = clientSections.FirstOrDefault(s => s.Name == ".rsrc");
        if (patchedRsrc != null && clientRsrc != null)
        {
            var patchedRsrcBytes = Slice(patched, patchedRsrc);
            if (patchedRsrcBytes.Length <= clientRsrc.RawSize)
            {
                Array.Copy(patchedRsrcBytes, 0, output, clientRsrc.RawPtr, patchedRsrcBytes.Length);
                if (patchedRsrcBytes.Length < clientRsrc.RawSize)
                {
                    Array.Clear(output, clientRsrc.RawPtr + patchedRsrcBytes.Length, clientRsrc.RawSize - patchedRsrcBytes.Length);
                }
            }
        }

        var temp = patchedPath + ".pe-shell.tmp";
        File.WriteAllBytes(temp, output);
        if (!ValidateManagedAssembly(temp))
        {
            File.Delete(temp);
            Console.WriteLine("pe-shell-fit: truncated .text is not a loadable assembly.");
            return null;
        }

        File.Copy(temp, patchedPath, true);
        File.Delete(temp);
        Console.WriteLine($"pe-shell-fit: repacked into client PE shell ({output.Length} bytes).");
        return output.Length;
    }

    private static bool ValidateManagedAssembly(string path)
    {
        try
        {
            using var asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(path);
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            foreach (var method in type.Methods.Where(m => m.HasBody).Take(8))
            {
                _ = method.Body.Instructions.Count;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("pe-shell-fit: validate failed: " + ex.Message);
            return false;
        }
    }

    private static byte[] Slice(byte[] file, PeSection section)
    {
        var len = Math.Min(section.RawSize, file.Length - section.RawPtr);
        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        var slice = new byte[len];
        Array.Copy(file, section.RawPtr, slice, 0, len);
        return slice;
    }

    private static PeSection[] ReadSections(byte[] file)
    {
        if (file.Length < 0x40)
        {
            return Array.Empty<PeSection>();
        }

        var peOff = BitConverter.ToInt32(file, 0x3C);
        if (peOff < 0 || peOff + 24 > file.Length)
        {
            return Array.Empty<PeSection>();
        }

        var numSec = BitConverter.ToUInt16(file, peOff + 6);
        var optSize = BitConverter.ToUInt16(file, peOff + 20);
        var secStart = peOff + 24 + optSize;
        var sections = new PeSection[numSec];
        for (var i = 0; i < numSec; i++)
        {
            var o = secStart + (i * 40);
            if (o + 40 > file.Length)
            {
                break;
            }

            sections[i] = new PeSection
            {
                Name = System.Text.Encoding.ASCII.GetString(file, o, 8).TrimEnd('\0'),
                RawSize = BitConverter.ToInt32(file, o + 16),
                RawPtr = BitConverter.ToInt32(file, o + 20)
            };
        }

        return sections;
    }

    private sealed class PeSection
    {
        public string Name = "";
        public int RawSize;
        public int RawPtr;
    }
}
