using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

internal static class PatchSlashPanel
{
    private const string PanelTypeName = "ClientModSlashPanel";
    private const string TryToggleName = "TryToggleFromKeys";
    private const string DrawName = "Draw";

    internal static bool Apply(string targetDll)
    {
        targetDll = System.IO.Path.GetFullPath(targetDll);
        if (!SlashPanelDonorBuild.Run())
        {
            return false;
        }

        var donorDll = SlashPanelDonorBuild.ResolveDonorDll();
        if (!System.IO.File.Exists(donorDll))
        {
            Console.Error.WriteLine("apply-slash-panel: DonorCompile.dll missing.");
            return false;
        }

        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(System.IO.Path.GetDirectoryName(targetDll)!);
        resolver.AddSearchDirectory(System.IO.Path.GetDirectoryName(donorDll)!);

        var stagingDir = System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(targetDll)!,
            "..", "..", "..", "tools", "FFPatch", "staging");
        stagingDir = System.IO.Path.GetFullPath(stagingDir);
        System.IO.Directory.CreateDirectory(stagingDir);
        var tempPath = System.IO.Path.Combine(stagingDir, "Assembly-CSharp.slash-panel.dll");
        using (var target = AssemblyDefinition.ReadAssembly(targetDll, new ReaderParameters { AssemblyResolver = resolver }))
        using (var donor = AssemblyDefinition.ReadAssembly(donorDll, new ReaderParameters { AssemblyResolver = resolver }))
        {
            var donorType = donor.MainModule.Types.FirstOrDefault(t => t.Name == PanelTypeName)
                ?? throw new InvalidOperationException(PanelTypeName + " missing in donor.");

            var existing = target.MainModule.Types.FirstOrDefault(t => t.Name == PanelTypeName);
            if (existing != null)
            {
                target.MainModule.Types.Remove(existing);
                Console.WriteLine("apply-slash-panel: replacing existing " + PanelTypeName);
            }

            var gameFrame = target.MainModule.Types.First(t => t.Name == "GameFrame");
            var avatarMove = target.MainModule.Types.First(t => t.Name == "cnAvatarThirdPersonMove");
            RemoveStaticCalls(gameFrame);
            RemoveStaticCalls(avatarMove);

            var importedType = ImportType(donorType, target.MainModule);

            var tryToggle = target.MainModule.ImportReference(
                importedType.Methods.First(m => m.Name == TryToggleName && m.IsStatic));
            var draw = target.MainModule.ImportReference(
                importedType.Methods.First(m => m.Name == DrawName && m.IsStatic));

            InjectCallBeforeNoclipKey(avatarMove, tryToggle);
            InjectStaticCallAtStart(gameFrame, "OnGUI", draw);

            target.Write(tempPath);
        }

        System.IO.File.Copy(tempPath, targetDll, true);
        System.IO.File.Delete(tempPath);
        Console.WriteLine("apply-slash-panel: injected " + PanelTypeName + " (ForceUpdate TryToggleFromKeys + GameFrame.OnGUI Draw).");
        return true;
    }

    private static TypeDefinition ImportType(TypeDefinition source, ModuleDefinition module)
    {
        var imported = new TypeDefinition(
            source.Namespace,
            source.Name,
            source.Attributes,
            module.ImportReference(source.BaseType));

        foreach (var field in source.Fields)
        {
            imported.Fields.Add(new FieldDefinition(
                field.Name,
                field.Attributes,
                module.ImportReference(field.FieldType)));
        }

        module.Types.Add(imported);

        foreach (var method in source.Methods)
        {
            if (!method.HasBody)
            {
                continue;
            }

            var added = new MethodDefinition(
                method.Name,
                method.Attributes,
                IlTransplant.ImportType(method.ReturnType, module))
            {
                DeclaringType = imported,
                ImplAttributes = method.ImplAttributes,
                HasThis = method.HasThis,
                ExplicitThis = method.ExplicitThis,
                CallingConvention = method.CallingConvention
            };

            foreach (var param in method.Parameters)
            {
                added.Parameters.Add(new ParameterDefinition(
                    param.Name,
                    param.Attributes,
                    IlTransplant.ImportType(param.ParameterType, module)));
            }

            imported.Methods.Add(added);
            Program.CopyMethodBody(method, added, module);
            IlStackHelper.RefreshMaxStack(added.Body);
        }

        return imported;
    }

    private static void RemoveStaticCalls(TypeDefinition gameFrame)
    {
        foreach (var methodName in new[] { "Update", "OnGUI", "ForceUpdate" })
        {
            var method = gameFrame.Methods.FirstOrDefault(m => m.Name == methodName && m.HasBody);
            if (method == null)
            {
                continue;
            }

            var body = method.Body;
            var toRemove = body.Instructions
                .Where(i =>
                    i.OpCode == OpCodes.Call &&
                    i.Operand is MethodReference mr &&
                    mr.DeclaringType.Name == PanelTypeName &&
                    (mr.Name == TryToggleName || mr.Name == DrawName))
                .ToList();

            if (toRemove.Count == 0)
            {
                continue;
            }

            var il = body.GetILProcessor();
            foreach (var ins in toRemove)
            {
                il.Remove(ins);
            }

            IlStackHelper.RefreshMaxStack(body);
            Console.WriteLine("Removed " + toRemove.Count + " old " + PanelTypeName + " call(s) from " + methodName);
        }
    }

    private static void InjectStaticCall(TypeDefinition type, string methodName, MethodReference callee)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == methodName && m.HasBody)
            ?? throw new InvalidOperationException(type.Name + "." + methodName + " not found.");

        var body = method.Body;
        var il = body.GetILProcessor();
        var ret = body.Instructions.Last(i => i.OpCode == OpCodes.Ret);
        var call = il.Create(OpCodes.Call, callee);
        il.InsertBefore(ret, call);
        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("Hooked: " + type.Name + "." + methodName + " -> " + callee.Name);
    }

    private static void InjectCallBeforeNoclipKey(TypeDefinition avatarMove, MethodReference callee)
    {
        var forceUpdate = avatarMove.Methods.FirstOrDefault(m => m.Name == "ForceUpdate" && m.HasBody)
            ?? throw new InvalidOperationException("cnAvatarThirdPersonMove.ForceUpdate not found.");

        var body = forceUpdate.Body;
        var noclipKey = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Ldc_I4 && i.Operand is int key && key == 307)
            ?? throw new InvalidOperationException("Noclip key (307) anchor not found in ForceUpdate.");

        var il = body.GetILProcessor();
        il.InsertBefore(noclipKey, il.Create(OpCodes.Call, callee));
        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("Hooked: cnAvatarThirdPersonMove.ForceUpdate -> " + callee.Name);
    }

    private static void InjectStaticCallAtStart(TypeDefinition type, string methodName, MethodReference callee)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == methodName && m.HasBody)
            ?? throw new InvalidOperationException(type.Name + "." + methodName + " not found.");

        var body = method.Body;
        var il = body.GetILProcessor();
        il.InsertBefore(body.Instructions[0], il.Create(OpCodes.Call, callee));
        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("Hooked: " + type.Name + "." + methodName + " (start) -> " + callee.Name);
    }

    private static void InjectStaticCallBefore(TypeDefinition type, string methodName, MethodReference callee, string anchorCallName)
    {
        var method = type.Methods.FirstOrDefault(m => m.Name == methodName && m.HasBody)
            ?? throw new InvalidOperationException(type.Name + "." + methodName + " not found.");

        var body = method.Body;
        var anchor = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == anchorCallName)
            ?? body.Instructions.Last(i => i.OpCode == OpCodes.Ret);

        var il = body.GetILProcessor();
        il.InsertBefore(anchor, il.Create(OpCodes.Call, callee));
        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("Hooked: " + type.Name + "." + methodName + " before " + anchorCallName + " -> " + callee.Name);
    }
}
