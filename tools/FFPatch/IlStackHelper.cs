using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class IlStackHelper
{
    internal static void RefreshMaxStack(MethodBody body)
    {
        // DonorCompile often emits MaxStack=4 for String.Concat(string[]) paths; Unity Mono
        // JIT-validates the whole method — undersized stack makes Cecil-added helpers fail on invoke.
        if (body.MaxStackSize < 16)
        {
            body.MaxStackSize = 16;
        }
    }
}
