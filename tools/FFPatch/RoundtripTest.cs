using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace FFPatch;

internal static class RoundtripTest
{
    public static bool Run(string clientPath)
    {
        clientPath = Path.GetFullPath(clientPath);
        var outPath = clientPath + ".roundtrip.dll";
        using (var asm = AssemblyDefinition.ReadAssembly(clientPath))
        {
            asm.Write(outPath, new WriterParameters { WriteSymbols = false });
        }

        var before = new FileInfo(clientPath).Length;
        var after = new FileInfo(outPath).Length;
        Console.WriteLine($"roundtrip: {before} -> {after} (delta {after - before})");
        Console.WriteLine($"client .text {GetTextRawSize(clientPath)} roundtrip .text {GetTextRawSize(outPath)}");
        return true;
    }

    private static int GetTextRawSize(string path)
    {
        var file = File.ReadAllBytes(path);
        var peOff = BitConverter.ToInt32(file, 0x3C);
        var optSize = BitConverter.ToInt16(file, peOff + 20);
        var secStart = peOff + 24 + optSize;
        for (var i = 0; i < BitConverter.ToUInt16(file, peOff + 6); i++)
        {
            var o = secStart + (i * 40);
            var name = System.Text.Encoding.ASCII.GetString(file, o, 8).TrimEnd('\0');
            if (name == ".text")
            {
                return BitConverter.ToInt32(file, o + 16);
            }
        }

        return -1;
    }
}
