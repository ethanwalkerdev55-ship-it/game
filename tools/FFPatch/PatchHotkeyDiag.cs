using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>Ensure F11 / Right Ctrl calls public ForceCompleteCurrentTask (private handlers fail silently from cnAvatarAttack).</summary>
internal static class PatchHotkeyDiag
{
    internal static void Apply(AssemblyDefinition asm)
    {
        var attackType = asm.MainModule.Types.FirstOrDefault(t => t.Name == "cnAvatarAttack")
            ?? throw new InvalidOperationException("cnAvatarAttack type not found.");

        var mgrType = asm.MainModule.Types.FirstOrDefault(t => t.Name == "cnMissionManager")
            ?? throw new InvalidOperationException("cnMissionManager type not found.");

        var fct = mgrType.Methods.FirstOrDefault(m => m.Name == "ForceCompleteCurrentTask" && m.IsPublic && !m.HasGenericParameters)
            ?? throw new InvalidOperationException("public ForceCompleteCurrentTask missing.");

        var fctRef = asm.MainModule.ImportReference(fct);

        var update = attackType.Methods.FirstOrDefault(m => m.Name == "Update" && m.HasBody)
            ?? throw new InvalidOperationException("cnAvatarAttack.Update not found.");

        var body = update.Body;

        var hotkeyCall = FindHotkeyCall(body)
            ?? throw new InvalidOperationException("cnAvatarAttack.Update: mission hotkey call not found.");

        if (hotkeyCall.Operand is MethodReference mr && mr.Name == "ForceCompleteCurrentTask")
        {
            Console.WriteLine("cnAvatarAttack: hotkey already routes to ForceCompleteCurrentTask (skip retarget)");
        }
        else
        {
            hotkeyCall.OpCode = OpCodes.Callvirt;
            hotkeyCall.Operand = fctRef;
            Console.WriteLine("cnAvatarAttack: hotkey retargeted to public ForceCompleteCurrentTask");
        }

        IlStackHelper.RefreshMaxStack(body);
    }

    private static Instruction? FindHotkeyCall(MethodBody body)
    {
        for (var i = 0; i < body.Instructions.Count; i++)
        {
            var ins = body.Instructions[i];
            if (ins.OpCode != OpCodes.Callvirt && ins.OpCode != OpCodes.Call)
            {
                continue;
            }

            if (ins.Operand is not MethodReference mr)
            {
                continue;
            }

            if (mr.Name is "ForceCompleteCurrentTask" or "ForceCompleteHotkeyHandler")
            {
                return ins;
            }
        }

        return null;
    }
}
