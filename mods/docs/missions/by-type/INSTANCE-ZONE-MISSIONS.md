# Instance zone missions (Fusion Lair / Infected Zone)

**Customer requirement:** Autocomplete must not loop until player enters Fusion Lair or Infected Zone.

**Code reality:** No zone names in logic — only `MissionElement.m_iRequireInstanceID > 0` and server instance state.

---

## Identification

| Signal | Source |
|--------|--------|
| `m_iRequireInstanceID > 0` | Mission table row |
| `bInsMap == true` | `cnOwnAvatarStatus` after `INSTANCE_MAP_INFO` |
| `iInsMapNum` | Current instance map number from server |

**Client does NOT** compare `iInsMapNum` to `m_iRequireInstanceID` during normal start/complete.

---

## Entering instance

**Packet:** `sP_FE2CL_INSTANCE_MAP_INFO` (822083738)

```csharp
bInsMap = true;
iInsMapNum = struct.iInstanceMapNum;
iEp_ID = struct.iEP_ID;
GameFrameEpContainer.InitRings(rect, epSwitches);
```

Player is now “in instance” from client perspective.

---

## Starting instance task outside zone

`RequestTaskStart` has **no** instance check.

Server may:

- Accept → `ProcessStartSucc : {task}` (observed for task **463** in mission 504)
- Reject → `ProcessStartFail tasknumber : {task}`

---

## Completing instance task

### Success path (in instance)

```
RequestTaskComplete(taskId, npcId, bError: false)
→ TASK_END
→ ProcessEndSucc
→ chain m_iSUOutgoingTask
```

### Failure outside instance (autocomplete / premature complete)

```
RequestTaskComplete(taskId, npcId, bError: false)   // vanilla hotkey
→ ProcessEndFail : {task} Error Code : 1
→ ProcessEndFail handler:
     RequestTaskStart(m_iFOutgoingTask)             // e.g. 466
→ LOG: Fail Outgoing Task : 466
```

This is the **infinite loop** customer describes.

### Abort on warp exit (only vanilla bError=true path)

When player warps out while `bInsMap`:

```csharp
CheckWarpAllMision():
  if (m_iRequireInstanceID > 0)
    RequestTaskComplete(taskId, 0, bError: true);
// iEscortNPC_ID forced to -1
```

Triggered from `GameFrame` on `WARP_USE_NPC_SUCC` if was in instance.

---

## World / map references (non-mission)

| File | Reference |
|------|-------------|
| `WorldMapMode.cs` | `FusionLairRect` world region; blocks map when `bInsMap && iEp_ID <= 0` |
| `cntutorialscript.cs` | BGM `"InfectedZone_Sting"` |
| `CnGuiChat.cs` | XCOM zone vs `iInsMapNum` |

These are UX/world — not mission completion logic.

---

## Instance table (metadata)

`InstanceTableScript` (TableContainer index **5**):

```csharp
InstanceElement: m_iZoneX, m_iZoneY, m_iInstanceNameID, m_iIsEP, ...
WarpElement: warp NPC ↔ instance mapping
```

Used for warps and display — mission manager reads only `m_iRequireInstanceID` from mission row.

---

## Patch strategy (ForceCompleteV2)

1. Detect instance task: `m_iRequireInstanceID > 0`
2. If outside instance: `RequestTaskStart` first, wait `ProcessStartSucc`, then complete
3. Send complete with **`bError=true`** when server expects abort-style complete outside zone OR after start-in-place hack
4. Block `ProcessEndFail` from firing `m_iFOutgoingTask` while `bForceCompleteChain` active
5. Log: `ForceCompleteV2: instance zone complete task N`

Customer workaround (from `client_requirement.txt`): warp NPC substitution — maps task → lair ID via game files.

---

## Packet formats

### Start (outside or inside instance)

```
TASK_START: iTaskNum, iNPC_ID, iEscortNPC_ID
```

### Complete (success, in instance)

```
TASK_END: iTaskNum, iNPC_ID, iEscortNPC_ID>=0, boxes=0
```

### Complete (abort / patch outside instance)

```
TASK_END: iTaskNum, iNPC_ID, iEscortNPC_ID=-1  (bError=true in code)
```

---

## Example: task 463 (mission 504)

See [../MISSION-504.md](../MISSION-504.md).

- `ProcessStartSucc : 463` — tutorial `CheckCondition(18)`
- `ProcessEndFail : 463 Error Code : 1` — outside instance complete
- `Fail Outgoing Task : 466` — fail chain
