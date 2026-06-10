using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class IlLookup
{
    internal static MethodReference FindLog(MethodBody body) =>
        body.Instructions
            .Where(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference { Name: "Log" })
            .Select(i => (MethodReference)i.Operand!)
            .First();

    internal static FieldReference FindField(MethodBody body, string fieldName) =>
        FindField(body, body.Method.Module, fieldName);

    internal static FieldReference FindField(MethodBody? body, ModuleDefinition module, string fieldName)
    {
        if (body != null)
        {
            var fromBody = body.Instructions
                .Select(i => i.Operand as FieldReference)
                .FirstOrDefault(fr => fr?.Name == fieldName);
            if (fromBody != null)
            {
                return fromBody;
            }
        }

        foreach (var type in module.Types)
        {
            foreach (var method in type.Methods.Where(m => m.HasBody))
            {
                var fromMethod = method.Body!.Instructions
                    .Select(i => i.Operand as FieldReference)
                    .FirstOrDefault(fr => fr?.Name == fieldName);
                if (fromMethod != null)
                {
                    return fromMethod;
                }
            }
        }

        throw new System.InvalidOperationException("IlLookup: field " + fieldName + " not found.");
    }

    internal static MethodReference FindGetMe(ModuleDefinition module) =>
        module.Types.First(t => t.Methods.Any(m => m.Name == "GetMe"))
            .Methods.First(m => m.Name == "GetMe" && m.Parameters.Count == 0);
}
