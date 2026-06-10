using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class IlTransplant
{
    internal static TypeReference ImportType(TypeReference type, ModuleDefinition module) => ResolveType(type, module);

    public static void CopyMethodBody(MethodDefinition source, MethodDefinition target, ModuleDefinition module)
    {
        target.Body = new MethodBody(target);
        var body = target.Body;
        var il = body.GetILProcessor();

        var signatureMap = BuildSignatureMap(source, target, module);

        foreach (var variable in source.Body.Variables)
        {
            body.Variables.Add(new VariableDefinition(ResolveType(variable.VariableType, module)));
        }

        var instructionMap = new Dictionary<Instruction, Instruction>();
        foreach (var instr in source.Body.Instructions)
        {
            instructionMap[instr] = il.Create(OpCodes.Nop);
        }

        foreach (var pair in instructionMap)
        {
            var newInstr = pair.Value;
            var oldInstr = pair.Key;
            newInstr.OpCode = oldInstr.OpCode;

            if (oldInstr.Operand == null)
            {
            }
            else if (oldInstr.Operand is Instruction targetInstr)
            {
                newInstr.Operand = instructionMap[targetInstr];
            }
            else if (oldInstr.Operand is Instruction[] targetInstrs)
            {
                newInstr.Operand = targetInstrs.Select(i => instructionMap[i]).ToArray();
            }
            else if (oldInstr.Operand is ParameterDefinition param)
            {
                newInstr.Operand = target.Parameters[param.Index];
            }
            else if (oldInstr.Operand is VariableDefinition variable)
            {
                newInstr.Operand = body.Variables[variable.Index];
            }
            else if (oldInstr.Operand is FieldReference field)
            {
                newInstr.Operand = MapField(field, signatureMap, module);
            }
            else if (oldInstr.Operand is MethodReference method)
            {
                newInstr.Operand = MapMethod(method, signatureMap, module);
            }
            else if (oldInstr.Operand is TypeReference type)
            {
                newInstr.Operand = ResolveType(type, module);
            }
            else if (oldInstr.Operand is string s)
            {
                newInstr.Operand = s;
            }
            else if (oldInstr.Operand is sbyte or byte or int or long or float or double)
            {
                newInstr.Operand = oldInstr.Operand;
            }
            else
            {
                throw new NotSupportedException("Unsupported operand: " + oldInstr.Operand.GetType().Name + " in " + source.Name);
            }

            il.Append(newInstr);
        }

        foreach (var handler in source.Body.ExceptionHandlers)
        {
            body.ExceptionHandlers.Add(new ExceptionHandler(handler.HandlerType)
            {
                CatchType = handler.CatchType != null ? ResolveType(handler.CatchType, module) : null,
                TryStart = instructionMap[handler.TryStart],
                TryEnd = instructionMap[handler.TryEnd],
                HandlerStart = instructionMap[handler.HandlerStart],
                HandlerEnd = instructionMap[handler.HandlerEnd]
            });
        }

        body.InitLocals = source.Body.InitLocals;
        body.MaxStackSize = source.Body.MaxStackSize;
    }

    private static Dictionary<string, MemberReference> BuildSignatureMap(MethodDefinition source, MethodDefinition target, ModuleDefinition module)
    {
        var map = new Dictionary<string, MemberReference>();
        var sourceType = source.DeclaringType;
        var targetType = target.DeclaringType;

        foreach (var field in sourceType.Fields)
        {
            var match = targetType.Fields.FirstOrDefault(f => f.Name == field.Name);
            if (match != null)
            {
                map[FieldKey(field)] = match;
            }
        }

        foreach (var method in sourceType.Methods)
        {
            var match = targetType.Methods.FirstOrDefault(m => MethodMatches(m, method));
            if (match != null)
            {
                map[MethodKey(method)] = match;
            }
        }

        return map;
    }

    private static FieldReference MapField(FieldReference field, Dictionary<string, MemberReference> map, ModuleDefinition module)
    {
        if (map.TryGetValue(FieldKey(field), out var mapped))
        {
            return module.ImportReference((FieldReference)mapped);
        }

        var declName = field.DeclaringType.Name;
        foreach (var type in EnumerateTypes(module))
        {
            if (type.Name != declName)
            {
                continue;
            }

            var match = type.Fields.FirstOrDefault(f => f.Name == field.Name);
            if (match != null)
            {
                return module.ImportReference(match);
            }
        }

        var borrowed = BorrowFieldReference(module, declName, field.Name);
        if (borrowed != null)
        {
            return module.ImportReference(borrowed);
        }

        var fromRefs = ResolveFieldFromReferences(module, declName, field.Name);
        if (fromRefs != null)
        {
            return fromRefs;
        }

        if (IsAllowedExternalReference(field, module))
        {
            return module.ImportReference(field);
        }

        throw new InvalidOperationException($"IlTransplant: unresolved field {declName}.{field.Name}");
    }

    private static MethodReference MapMethod(MethodReference method, Dictionary<string, MemberReference> map, ModuleDefinition module)
    {
        if (map.TryGetValue(MethodKey(method), out var mapped))
        {
            return module.ImportReference((MethodReference)mapped);
        }

        var declName = method.DeclaringType.Name;
        MethodDefinition? best = null;
        foreach (var type in EnumerateTypes(module))
        {
            if (type.Name != declName)
            {
                continue;
            }

            foreach (var candidate in type.Methods)
            {
                if (!MethodMatches(candidate, method))
                {
                    continue;
                }

                best = candidate;
                if (ParametersMatchByName(candidate, method))
                {
                    return module.ImportReference(candidate);
                }
            }
        }

        if (best != null)
        {
            return module.ImportReference(best);
        }

        var borrowed = BorrowMethodReference(module, declName, method);
        if (borrowed != null)
        {
            return module.ImportReference(borrowed);
        }

        borrowed = BorrowAnyMethodReference(module, method);
        if (borrowed != null)
        {
            return module.ImportReference(borrowed);
        }

        var fromRefs = ResolveMethodFromReferences(module, declName, method);
        if (fromRefs != null)
        {
            return fromRefs;
        }

        if (IsAllowedExternalReference(method, module))
        {
            return module.ImportReference(method);
        }

        throw new InvalidOperationException(
            $"IlTransplant: unresolved method {declName}.{method.Name} ({method.Parameters.Count} params)");
    }

    private static TypeReference? BorrowTypeReference(ModuleDefinition module, string typeName)
    {
        foreach (var type in EnumerateTypes(module))
        {
            foreach (var method in type.Methods)
            {
                if (method.Body == null)
                {
                    continue;
                }

                foreach (var variable in method.Body.Variables)
                {
                    if (variable.VariableType.Name == typeName)
                    {
                        return variable.VariableType;
                    }
                }

                foreach (var ins in method.Body.Instructions)
                {
                    if (ins.Operand is MethodReference mr && mr.ReturnType.Name == typeName)
                    {
                        return mr.ReturnType;
                    }

                    if (ins.Operand is FieldReference fr && fr.DeclaringType.Name == typeName)
                    {
                        return fr.DeclaringType;
                    }
                }
            }
        }

        return null;
    }

    private static FieldReference? BorrowFieldReference(ModuleDefinition module, string typeName, string fieldName)
    {
        foreach (var type in EnumerateTypes(module))
        {
            foreach (var method in type.Methods)
            {
                if (method.Body == null)
                {
                    continue;
                }

                foreach (var ins in method.Body.Instructions)
                {
                    if (ins.Operand is FieldReference fr &&
                        fr.DeclaringType.Name == typeName &&
                        fr.Name == fieldName)
                    {
                        return fr;
                    }
                }
            }
        }

        return null;
    }

    private static MethodReference? BorrowMethodReference(ModuleDefinition module, string typeName, MethodReference source)
    {
        foreach (var type in EnumerateTypes(module))
        {
            foreach (var method in type.Methods)
            {
                if (method.Body == null)
                {
                    continue;
                }

                foreach (var ins in method.Body.Instructions)
                {
                    if (ins.Operand is MethodReference mr &&
                        mr.DeclaringType.Name == typeName &&
                        mr.Name == source.Name &&
                        mr.Parameters.Count == source.Parameters.Count)
                    {
                        return mr;
                    }
                }
            }
        }

        foreach (var type in EnumerateTypes(module))
        {
            if (type.Name != typeName)
            {
                continue;
            }

            var match = type.Methods.FirstOrDefault(m => MethodMatches(m, source));
            if (match != null)
            {
                return match;
            }
        }

        return null;
    }

    private static MethodReference? BorrowAnyMethodReference(ModuleDefinition module, MethodReference source)
    {
        foreach (var type in EnumerateTypes(module))
        {
            foreach (var method in type.Methods)
            {
                if (method.Body == null)
                {
                    continue;
                }

                foreach (var ins in method.Body.Instructions)
                {
                    if (ins.Operand is MethodReference mr &&
                        mr.Name == source.Name &&
                        mr.HasThis == source.HasThis &&
                        mr.Parameters.Count == source.Parameters.Count)
                    {
                        return mr;
                    }
                }
            }
        }

        return null;
    }

    private static bool IsAllowedExternalReference(MemberReference member, ModuleDefinition module)
    {
        var scope = member.DeclaringType?.Scope as AssemblyNameReference;
        if (scope == null)
        {
            return false;
        }

        if (scope.Name.IndexOf("DonorCompile", StringComparison.OrdinalIgnoreCase) >= 0 ||
            scope.Name.IndexOf("SlashPanelDonor", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return false;
        }

        return module.AssemblyReferences.Any(r => r.Name == scope.Name);
    }

    private static TypeReference ResolveType(TypeReference type, ModuleDefinition module)
    {
        if (type.IsArray)
        {
            return new ArrayType(ResolveType(type.GetElementType(), module));
        }

        if (type.IsGenericInstance)
        {
            var generic = (GenericInstanceType)type;
            var element = (TypeReference)EnumerateTypes(module).FirstOrDefault(t => t.Name == generic.ElementType.Name)
                ?? module.ImportReference(generic.ElementType);
            var instance = new GenericInstanceType(element);
            foreach (var arg in generic.GenericArguments)
            {
                instance.GenericArguments.Add(ResolveType(arg, module));
            }

            return instance;
        }

        var found = EnumerateTypes(module).FirstOrDefault(t => t.Name == type.Name);
        if (found != null)
        {
            return module.ImportReference(found);
        }

        var borrowed = BorrowTypeReference(module, type.Name);
        if (borrowed != null)
        {
            return module.ImportReference(borrowed);
        }

        if (type.Scope is AssemblyNameReference scope)
        {
            if (scope.Name.IndexOf("DonorCompile", StringComparison.OrdinalIgnoreCase) >= 0 ||
                scope.Name.IndexOf("SlashPanelDonor", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var refType = ResolveTypeFromReferences(module, type.Name);
                if (refType != null)
                {
                    return refType;
                }

                throw new InvalidOperationException($"IlTransplant: donor type {type.FullName}");
            }

            if (module.AssemblyReferences.Any(r => r.Name == scope.Name))
            {
                return module.ImportReference(type);
            }
        }

        throw new InvalidOperationException($"IlTransplant: unresolved type {type.FullName}");
    }

    private static TypeReference? ResolveTypeFromReferences(ModuleDefinition module, string typeName)
    {
        foreach (var type in EnumerateReferencedTypes(module))
        {
            if (type.Name == typeName)
            {
                return module.ImportReference(type);
            }
        }

        return null;
    }

    private static FieldReference? ResolveFieldFromReferences(ModuleDefinition module, string typeName, string fieldName)
    {
        foreach (var type in EnumerateReferencedTypes(module))
        {
            if (type.Name != typeName)
            {
                continue;
            }

            var field = type.Fields.FirstOrDefault(f => f.Name == fieldName);
            if (field != null)
            {
                return module.ImportReference(field);
            }
        }

        return null;
    }

    private static MethodReference? ResolveMethodFromReferences(ModuleDefinition module, string typeName, MethodReference source)
    {
        foreach (var type in EnumerateReferencedTypes(module))
        {
            if (type.Name != typeName)
            {
                continue;
            }

            foreach (var candidate in type.Methods)
            {
                if (MethodMatches(candidate, source))
                {
                    return module.ImportReference(candidate);
                }
            }
        }

        return null;
    }

    private static IEnumerable<TypeDefinition> EnumerateReferencedTypes(ModuleDefinition module)
    {
        var resolver = module.AssemblyResolver;
        if (resolver == null)
        {
            yield break;
        }

        foreach (var asmRef in module.AssemblyReferences)
        {
            AssemblyDefinition asm;
            try
            {
                asm = resolver.Resolve(asmRef);
            }
            catch
            {
                continue;
            }

            foreach (var type in asm.MainModule.Types)
            {
                yield return type;
                foreach (var nested in EnumerateNested(type))
                {
                    yield return nested;
                }
            }
        }
    }

    private static IEnumerable<TypeDefinition> EnumerateTypes(ModuleDefinition module)
    {
        foreach (var type in module.Types)
        {
            yield return type;
            foreach (var nested in EnumerateNested(type))
            {
                yield return nested;
            }
        }
    }

    private static IEnumerable<TypeDefinition> EnumerateNested(TypeDefinition type)
    {
        foreach (var nested in type.NestedTypes)
        {
            yield return nested;
            foreach (var child in EnumerateNested(nested))
            {
                yield return child;
            }
        }
    }

    private static bool MethodMatches(MethodDefinition a, MethodReference b) =>
        a.Name == b.Name &&
        a.HasThis == b.HasThis &&
        a.Parameters.Count == b.Parameters.Count &&
        a.GenericParameters.Count == b.GenericParameters.Count;

    private static bool ParametersMatchByName(MethodDefinition a, MethodReference b)
    {
        for (var i = 0; i < a.Parameters.Count; i++)
        {
            if (a.Parameters[i].ParameterType.Name != b.Parameters[i].ParameterType.Name)
            {
                return false;
            }
        }

        return true;
    }

    private static string FieldKey(FieldReference field) => "F:" + field.DeclaringType.Name + "." + field.Name;

    private static string MethodKey(MethodReference method) =>
        "M:" + method.DeclaringType.Name + "." + method.Name + "(" + method.Parameters.Count + ")";
}
