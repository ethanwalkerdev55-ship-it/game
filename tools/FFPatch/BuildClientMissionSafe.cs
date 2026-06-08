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
    private static readonly string[] NewMethodNames =
    {
        "GetForceCompleteTarget",
        "PrepareTaskForForceComplete",
        "NeedsForceCompleteTaskStart",
        "ResolveForceCompleteTerminatorNpcId",
        "ClearForceCompleteChain",
        "ForceCompleteEmitTaskEndPacket",
        "RequestForceCompleteTaskEnd",
        "TryForceCompleteChainTask",
        "ForceCompleteOnEndFail",
        "ForceCompleteOnEndSucc",
        "ForceCompleteOnStartFail",
        "ForceCompleteOnStartSucc"
    };

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

        if (!InjectSafe(workDll, donorDll))
        {
            File.Delete(workDll);
            return false;
        }

        File.Copy(workDll, targetDll, true);
        File.Delete(workDll);

        if (!PatchSafeClient.Apply(targetDll))
        {
            return false;
        }

        if (!RefreshAllMethodMaxStacks(targetDll))
        {
            return false;
        }

        if (!PatchVerify.Run(targetDll, liteMode: true))
        {
            return false;
        }

        if (!PatchVerify.VerifyLoadSafe(targetDll, clientBaseDll))
        {
            return false;
        }

        if (!PatchVerify.VerifyInstanceCompletePath(targetDll))
        {
            return false;
        }

        if (!PatchVerify.VerifyDirectSendPath(targetDll))
        {
            return false;
        }

        Console.WriteLine("Client mission SAFE+ OK: " + targetDll);
        return true;
    }

    private static bool InjectSafe(string targetDll, string donorDll)
    {
        var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(Path.GetDirectoryName(targetDll)!);
        resolver.AddSearchDirectory(Path.GetDirectoryName(donorDll)!);

        var tempPath = targetDll + ".client-safe.tmp";
        using (var target = AssemblyDefinition.ReadAssembly(targetDll, new ReaderParameters { AssemblyResolver = resolver }))
        using (var donor = AssemblyDefinition.ReadAssembly(donorDll, new ReaderParameters { AssemblyResolver = resolver }))
        {
            var targetType = target.MainModule.Types.First(t => t.Name == "cnMissionManager");
            var donorType = donor.MainModule.Types.First(t => t.Name == "cnMissionManagerV2Donor");

            AddFields(targetType, target.MainModule);

            foreach (var name in NewMethodNames)
            {
                var donorMethod = donorType.Methods.First(m => m.Name == name && !m.HasGenericParameters);
                var existing = targetType.Methods.FirstOrDefault(m => m.Name == name && !m.HasGenericParameters);
                if (existing != null)
                {
                    Program.CopyMethodBody(donorMethod, existing, target.MainModule);
                    IlStackHelper.RefreshMaxStack(existing.Body);
                    Console.WriteLine("Refreshed method: " + name);
                    continue;
                }

                var added = CloneMethod(donorMethod, target.MainModule, targetType);
                IlStackHelper.RefreshMaxStack(added.Body);
                targetType.Methods.Add(added);
                Console.WriteLine("Added method: " + name);
            }

            var fctDonor = donorType.Methods.First(m => m.Name == "ForceCompleteCurrentTask" && !m.HasGenericParameters);
            var fctTarget = targetType.Methods.First(m => m.Name == "ForceCompleteCurrentTask" && !m.HasGenericParameters);
            Program.CopyMethodBody(fctDonor, fctTarget, target.MainModule);
            IlStackHelper.RefreshMaxStack(fctTarget.Body);
            Console.WriteLine("Injected: ForceCompleteCurrentTask (client chain entry)");

            var rtcDonor = donorType.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
            var rtcTarget = targetType.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
            Program.CopyMethodBody(rtcDonor, rtcTarget, target.MainModule);
            IlStackHelper.RefreshMaxStack(rtcTarget.Body);
            Console.WriteLine("Injected: RequestTaskComplete (step8 checker bypass + diagnostics)");

            PatchRetargetRequestTaskCompleteCalls.Apply(targetType);
            PatchCustomerUpdate.InjectPendingTimerDefer(targetType);

            PatchV6.PatchForceCompleteSkipCheckerGate(targetType);
            PatchProcessEndFailGuard.Apply(targetType);
            PatchProcessEndFailChain.Apply(targetType);
            PatchProcessEndSuccChain.Apply(targetType);
            PatchProcessStartFailChain.Apply(targetType);
            PatchProcessStartSuccBlock.Apply(targetType);
            PatchProcessStartSuccChain.Apply(targetType);
            PatchHostResponseDiag.Apply(targetType);

            foreach (var method in targetType.Methods)
            {
                if (method.HasBody)
                {
                    IlStackHelper.RefreshMaxStack(method.Body);
                }
            }

            PatchHotkeyDiag.Apply(target);

            target.Write(tempPath);
        }

        File.Copy(tempPath, targetDll, true);
        File.Delete(tempPath);
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

    private static void AddFields(TypeDefinition targetType, ModuleDefinition module)
    {
        foreach (var (name, isBool) in new[]
        {
            ("bForceCompleteChain", true),
            ("m_iForceCompleteChainDepth", false),
            ("m_iForceCompletePendingTaskId", false),
            ("m_iForceCompleteRetryCount", false)
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
