using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class MethodSizeReport
{
    public static bool Run(string dllPath, params string[] methodNames)
    {
        dllPath = Path.GetFullPath(dllPath);
        using var asm = AssemblyDefinition.ReadAssembly(dllPath);
        var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
        foreach (var name in methodNames)
        {
            var method = type.Methods.First(m => m.Name == name && m.HasBody);
            var body = method.Body;
            var rtc = body.Instructions.Count(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "RequestTaskComplete");
            var loop = body.Instructions.Count(i =>
                i.OpCode == OpCodes.Ldfld &&
                i.Operand is FieldReference fr &&
                fr.Name == "m_iloopTemp");
            var next = body.Instructions.Count(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "GetNextTaskNode");
            Console.WriteLine(
                $"{name}: RVA=0x{method.RVA:X} ins={body.Instructions.Count} codeSize={body.CodeSize} maxStack={body.MaxStackSize} rtc={rtc} loop={loop} next={next}");
        }

        return true;
    }
}
