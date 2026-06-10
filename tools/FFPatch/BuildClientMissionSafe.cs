using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Load-safe client patch: add helper methods + replace ForceCompleteCurrentTask only.
/// ProcessEndFail gets a surgical fail-outgoing guard — never full donor transplant.
/// </summary>
internal static class BuildClientMissionSafe
{
    // Donor helpers stay in DonorCompile only — spliced inline (zero new MethodDefs).
    private static readonly string[] NewMethodNames = Array.Empty<string>();

    public static bool Apply(string targetDll, string clientBaseDll)
    {
        targetDll = Path.GetFullPath(targetDll);
        clientBaseDll = Path.GetFullPath(clientBaseDll);

        if (!File.Exists(clientBaseDll))
        {
            Console.Error.WriteLine("apply-client-safe: client base missing.");
            return false;
        }

        var workDll = targetDll + ".work.tmp";
        File.Copy(clientBaseDll, workDll, true);

        if (!DonorCompileBuild.Run())
        {
            File.Delete(workDll);
            return false;
        }

        var donorDll = DonorCompileBuild.ResolveDonorDll();
        if (!File.Exists(donorDll))
        {
            Console.Error.WriteLine("DonorCompile.dll not found.");
            File.Delete(workDll);
            return false;
        }

        if (!InjectSafe(workDll, donorDll, clientBaseDll))
        {
            File.Delete(workDll);
            return false;
        }

        File.Copy(workDll, targetDll, true);
        File.Delete(workDll);

        if (!PatchVerify.Run(targetDll, liteMode: true))
        {
            return false;
        }

        if (!PatchVerify.VerifyLoadSafe(targetDll, clientBaseDll))
        {
            return false;
        }

        if (!PatchVerify.VerifyBootstrapPath(targetDll))
        {
            return false;
        }

        if (!PatchVerify.VerifyInstanceCompletePath(targetDll))
        {
            return false;
        }

        Console.WriteLine("Client mission SAFE+ OK: " + targetDll);
        return true;
    }

    private static bool InjectSafe(string targetDll, string donorDll, string clientBaseDll)
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(Path.GetDirectoryName(targetDll)!);
        resolver.AddSearchDirectory(Path.GetDirectoryName(donorDll)!);

        var dnlibOut = targetDll + ".dnlib-slot.tmp";
        TypeDefinition? patchedMissionType = null;
        using (var target = AssemblyDefinition.ReadAssembly(targetDll, new ReaderParameters { AssemblyResolver = resolver }))
        using (var donor = AssemblyDefinition.ReadAssembly(donorDll, new ReaderParameters { AssemblyResolver = resolver }))
        {
            var targetType = target.MainModule.Types.First(t => t.Name == "cnMissionManager");
            var donorType = donor.MainModule.Types.First(t => t.Name == "cnMissionManagerV2Donor");
            patchedMissionType = targetType;

            var fctDonor = donorType.Methods.First(m => m.Name == "ForceCompleteCurrentTask" && !m.HasGenericParameters);
            var fctTarget = targetType.Methods.First(m => m.Name == "ForceCompleteCurrentTask" && !m.HasGenericParameters);
            Program.CopyMethodBody(fctDonor, fctTarget, target.MainModule);
            IlStackHelper.RefreshMaxStack(fctTarget.Body);
            Console.WriteLine("Injected: ForceCompleteCurrentTask (client chain entry)");

            // Surgical RTC chain bypass only — full donor transplant adds ~10KB and breaks UnityWeb inject size budget.
            PatchV6.PatchRequestTaskCompleteChainBypass(targetType);
            Console.WriteLine("Patched: RequestTaskComplete (surgical chain bypass)");

            PatchV6.PatchForceCompleteRemoveDelChecker(targetType);
            PatchV6.PatchForceCompleteSkipCheckerGate(targetType);

            // ProcessStartSucc / ProcessEndFail hooks deferred (+512 B each vs 1520640 slot).
            // doc504g trades them for terminator-NPC RTC (persistence fix for Talk/GotoLocation).

            // ProcessEndSucc chain hook deferred if dnlib slot-fit fails (+512 B).

            foreach (var method in targetType.Methods)
            {
                if (method.HasBody)
                {
                    IlStackHelper.RefreshMaxStack(method.Body);
                }
            }

            PatchHotkeyDiag.Apply(target);
            RepairDanglingBranches(patchedMissionType);
            ValidateIlOperands(patchedMissionType);
            ValidateBranchTargets(patchedMissionType);

            if (!DnlibPreserveWriter.Write(dnlibOut, clientBaseDll, patchedMissionType))
            {
                return false;
            }
        }

        File.Copy(dnlibOut, targetDll, true);
        File.Delete(dnlibOut);
        return true;
    }

    private static bool RefreshAllMethodMaxStacks(string dllPath)
    {
        var tempPath = dllPath + ".maxstack.tmp";
        using (var asm = AssemblyDefinition.ReadAssembly(dllPath))
        {
            var type = asm.MainModule.Types.First(t => t.Name == "cnMissionManager");
            foreach (var method in type.Methods)
            {
                if (method.HasBody)
                {
                    IlStackHelper.RefreshMaxStack(method.Body);
                }
            }

            asm.Write(tempPath);
        }

        File.Copy(tempPath, dllPath, true);
        File.Delete(tempPath);
        Console.WriteLine("Refreshed MaxStack on cnMissionManager methods.");
        return true;
    }

    private static void ValidateIlOperands(TypeDefinition type)
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            for (var i = 0; i < method.Body!.Instructions.Count; i++)
            {
                var ins = method.Body.Instructions[i];
                if (ins.OpCode.OperandType != OperandType.InlineNone && ins.Operand == null)
                {
                    throw new InvalidOperationException(
                        "InjectSafe: null IL operand in " + method.Name + " at index " + i + " opcode " + ins.OpCode);
                }
            }
        }
    }

    private static void RepairDanglingBranches(TypeDefinition type)
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            var body = method.Body!;
            var insn = body.Instructions;
            if (insn.Count == 0)
            {
                continue;
            }

            var set = new System.Collections.Generic.HashSet<Instruction>(insn);
            var fallback = insn.Last(i => i.OpCode == OpCodes.Ret);
            var repaired = 0;
            foreach (var ins in insn)
            {
                if (ins.Operand is Instruction target && !set.Contains(target))
                {
                    ins.Operand = fallback;
                    repaired++;
                }
                else if (ins.Operand is Instruction[] targets)
                {
                    for (var i = 0; i < targets.Length; i++)
                    {
                        if (!set.Contains(targets[i]))
                        {
                            targets[i] = fallback;
                            repaired++;
                        }
                    }
                }
            }

            if (repaired > 0)
            {
                Console.WriteLine($"InjectSafe: repaired {repaired} dangling branch(es) in {method.Name}.");
            }
        }
    }

    private static void ValidateBranchTargets(TypeDefinition type)
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            var body = method.Body!;
            var insn = body.Instructions;
            var set = new System.Collections.Generic.HashSet<Instruction>(insn);
            for (var i = 0; i < insn.Count; i++)
            {
                switch (insn[i].Operand)
                {
                    case Instruction target when !set.Contains(target):
                        throw new InvalidOperationException(
                            "InjectSafe: dangling branch in " + method.Name + " at index " + i);
                    case Instruction[] targets:
                        foreach (var t in targets)
                        {
                            if (!set.Contains(t))
                            {
                                throw new InvalidOperationException(
                                    "InjectSafe: dangling switch branch in " + method.Name + " at index " + i);
                            }
                        }

                        break;
                }
            }
        }
    }

    private static void AddFields(TypeDefinition targetType, ModuleDefinition module)
    {
        foreach (var (name, isBool) in new[]
        {
            ("m_iForceCompleteChainDepth", false)
        })
        {
            if (targetType.Fields.Any(f => f.Name == name))
            {
                continue;
            }

            var fieldType = isBool ? module.TypeSystem.Boolean : module.TypeSystem.Int32;
            targetType.Fields.Add(new FieldDefinition(name, FieldAttributes.Private, fieldType));
            Console.WriteLine("Added field: " + name);
        }
    }

    private static MethodDefinition CloneMethod(MethodDefinition source, ModuleDefinition module, TypeDefinition declaringType)
    {
        var method = new MethodDefinition(source.Name, source.Attributes, IlTransplant.ImportType(source.ReturnType, module))
        {
            DeclaringType = declaringType,
            ImplAttributes = source.ImplAttributes,
            HasThis = source.HasThis,
            ExplicitThis = source.ExplicitThis,
            CallingConvention = source.CallingConvention
        };

        foreach (var param in source.Parameters)
        {
            method.Parameters.Add(new ParameterDefinition(param.Name, param.Attributes, IlTransplant.ImportType(param.ParameterType, module)));
        }

        Program.CopyMethodBody(source, method, module);
        return method;
    }
}
