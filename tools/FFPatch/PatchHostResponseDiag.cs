using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>
/// Log server task-end responses so we can see what the host returns after SendPacket.
/// </summary>
internal static class PatchHostResponseDiag
{
    private const string SuccPrefix = "ForceCompleteV2: host ProcessEndSucc task ";
    private const string FailPrefix = "ForceCompleteV2: host ProcessEndFail task ";

    internal static void Apply(TypeDefinition type)
    {
        PatchEndSucc(type);
        PatchEndFail(type);
    }

    private static void PatchEndSucc(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessEndSucc");
        var body = method.Body;
        if (body.Instructions.Any(i => i.OpCode == OpCodes.Ldstr && SuccPrefix.Equals(i.Operand as string)))
        {
            Console.WriteLine("ProcessEndSucc: host diag already present (skip)");
            return;
        }

        var il = body.GetILProcessor();
        var logger = FindLogger(body);
        var concat = FindConcat2(body);
        var toString = FindInt32ToString(body);

        var taskLocal = body.Variables.FirstOrDefault(v => v.VariableType.Name.Contains("REP_PC_TASK_END_SUCC"))
            ?? throw new InvalidOperationException("ProcessEndSucc: struct local not found.");
        var taskNumField = type.Module.ImportReference(
            taskLocal.VariableType.Resolve().Fields.First(f => f.Name == "iTaskNum"));

        var firstLog = body.Instructions.First(i =>
            i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.Name == "Log");

        var block = new[]
        {
            il.Create(OpCodes.Ldstr, SuccPrefix),
            il.Create(OpCodes.Ldloca, taskLocal),
            il.Create(OpCodes.Ldfld, taskNumField),
            il.Create(OpCodes.Call, toString),
            il.Create(OpCodes.Call, concat),
            il.Create(OpCodes.Call, logger)
        };

        foreach (var ins in block)
        {
            il.InsertBefore(firstLog, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("ProcessEndSucc: host response diag injected");
    }

    private static void PatchEndFail(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "ProcessEndFail");
        var body = method.Body;
        if (body.Instructions.Any(i => i.OpCode == OpCodes.Ldstr && FailPrefix.Equals(i.Operand as string)))
        {
            Console.WriteLine("ProcessEndFail: host diag already present (skip)");
            return;
        }

        var il = body.GetILProcessor();
        var logger = FindLogger(body);
        var concat = FindConcat2(body);
        var toString = FindInt32ToString(body);

        var taskLocal = body.Variables.FirstOrDefault(v => v.VariableType.Name.Contains("REP_PC_TASK_END_FAIL"))
            ?? throw new InvalidOperationException("ProcessEndFail: struct local not found.");
        var taskNumField = type.Module.ImportReference(
            taskLocal.VariableType.Resolve().Fields.First(f => f.Name == "iTaskNum"));
        var errField = type.Module.ImportReference(
            taskLocal.VariableType.Resolve().Fields.First(f => f.Name == "iErrorCode"));

        var firstLog = body.Instructions.First(i =>
            i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.Name == "Log");

        var block = new[]
        {
            il.Create(OpCodes.Ldstr, FailPrefix),
            il.Create(OpCodes.Ldloca, taskLocal),
            il.Create(OpCodes.Ldfld, taskNumField),
            il.Create(OpCodes.Call, toString),
            il.Create(OpCodes.Call, concat),
            il.Create(OpCodes.Ldstr, " err "),
            il.Create(OpCodes.Ldloca, taskLocal),
            il.Create(OpCodes.Ldfld, errField),
            il.Create(OpCodes.Call, toString),
            il.Create(OpCodes.Call, concat),
            il.Create(OpCodes.Call, concat),
            il.Create(OpCodes.Call, logger)
        };

        foreach (var ins in block)
        {
            il.InsertBefore(firstLog, ins);
        }

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine("ProcessEndFail: host response diag injected");
    }

    private static MethodReference FindLogger(MethodBody body) =>
        body.Instructions.First(i => i.OpCode == OpCodes.Call && i.Operand is MethodReference mr && mr.Name == "Log")
            .Operand as MethodReference ?? throw new InvalidOperationException("Logger.Log not found.");

    private static MethodReference FindConcat2(MethodBody body) =>
        body.Instructions.First(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "Concat" &&
                mr.DeclaringType.FullName.Contains("System.String") &&
                mr.HasThis == false &&
                mr.Parameters.Count == 2)
            .Operand as MethodReference ?? throw new InvalidOperationException("String.Concat(string,string) not found.");

    private static MethodReference FindInt32ToString(MethodBody body) =>
        body.Instructions.First(i =>
                i.OpCode == OpCodes.Call &&
                i.Operand is MethodReference mr &&
                mr.Name == "ToString" &&
                mr.DeclaringType.FullName.Contains("Int32"))
            .Operand as MethodReference ?? throw new InvalidOperationException("Int32.ToString not found.");
}
