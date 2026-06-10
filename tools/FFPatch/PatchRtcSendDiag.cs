using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FFPatch;

/// <summary>Log every TASK_END packet actually sent from RequestTaskComplete.</summary>
internal static class PatchRtcSendDiag
{
    internal static void Apply(TypeDefinition type)
    {
        var method = type.Methods.First(m => m.Name == "RequestTaskComplete" && m.Parameters.Count == 3);
        var body = method.Body;
        var sendPacket = body.Instructions.FirstOrDefault(i =>
            i.OpCode == OpCodes.Call &&
            i.Operand is MethodReference mr &&
            mr.Name == "SendPacket");
        if (sendPacket == null)
        {
            Console.WriteLine("RTC send-diag: SendPacket call not found (skip)");
            return;
        }

        if (body.Instructions.Take(body.Instructions.IndexOf(sendPacket)).Any(i =>
                i.OpCode == OpCodes.Ldstr &&
                i.Operand as string == "ForceCompleteV2: send task "))
        {
            Console.WriteLine("RTC send-diag: already present (skip)");
            return;
        }

        var fct = type.Methods.First(m => m.Name == "ForceCompleteCurrentTask");
        MethodReference logMethod = IlLookup.FindLog(fct.Body!);
        var toStringInt = type.Module.ImportReference(typeof(int).GetMethod("ToString", Type.EmptyTypes)!);
        var toStringBool = type.Module.ImportReference(typeof(bool).GetMethod("ToString", Type.EmptyTypes)!);
        var concat = type.Module.ImportReference(
            typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) })!);

        var il = body.GetILProcessor();
        var at = sendPacket;

        il.InsertBefore(at, il.Create(OpCodes.Ldstr, "ForceCompleteV2: send task "));
        il.InsertBefore(at, il.Create(OpCodes.Ldarg_1));
        il.InsertBefore(at, il.Create(OpCodes.Call, toStringInt));
        il.InsertBefore(at, il.Create(OpCodes.Call, concat));

        il.InsertBefore(at, il.Create(OpCodes.Ldstr, " npcTbl "));
        il.InsertBefore(at, il.Create(OpCodes.Ldarg_2));
        il.InsertBefore(at, il.Create(OpCodes.Call, toStringInt));
        il.InsertBefore(at, il.Create(OpCodes.Call, concat));

        il.InsertBefore(at, il.Create(OpCodes.Ldstr, " err "));
        il.InsertBefore(at, il.Create(OpCodes.Ldarg_3));
        il.InsertBefore(at, il.Create(OpCodes.Call, toStringBool));
        il.InsertBefore(at, il.Create(OpCodes.Call, concat));

        il.InsertBefore(at, il.Create(OpCodes.Call, logMethod));

        IlStackHelper.RefreshMaxStack(body);
        Console.WriteLine($"RTC send-diag: log before SendPacket (MaxStack={body.MaxStackSize}).");
    }
}
