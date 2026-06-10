# Timer missions — logic and completion rules

**Customer requirement:** Autocomplete must not fail with “return to NPC” / server **error 1** on timed tasks.

---

## Identification (MissionElement)

A task is timer-related if **either**:

| Field | Meaning |
|-------|---------|
| `m_iSTGrantTimer > 0` | Grant timer — countdown from task start |
| `m_iCSUCheckTimer > 0` | Completion gate — cannot complete until elapsed |

Both can be set on the same task.

---

## Grant timer (`m_iSTGrantTimer`)

### Start

`ProcessStartSucc` receives `iRemainTime` from server:

```csharp
if (0 < task.GetMe().m_iSTGrantTimer)
    task.SetRemainTime((float)sP_FE2CL_REP_PC_TASK_START_SUCC.iRemainTime);
```

### Countdown

`cnMissionManager.Update` (1 Hz):

```csharp
cnMissionNode.UpdateRemainTime();
if (0f >= cnMissionNode.m_fRemainTime) {
    Logger.Log("Send Time mission.");
    RequestTaskComplete(taskId, 0, false);  // NOT bError
}
```

### UI

`cnMissionJournal` / `cnGUINanocom` show `Remaining time: N` when `m_iSTGrantTimer > 0`.

---

## Completion gate (`m_iCSUCheckTimer`)

`CheckToCompleteTaskCondition`:

```csharp
if (0 < task.GetMe().m_iCSUCheckTimer && 0f >= task.m_fRemainTime)
    return 6;  // Fail — blocks local complete
```

Player **cannot** manually complete until timer reaches zero (unless autocomplete bypasses check).

---

## Server interaction

| Event | Typical server response |
|-------|-------------------------|
| Auto complete at timer zero | `ProcessEndSucc` OR `ProcessEndFail err 1` if server disagrees |
| Early complete (cheat/hotkey) | `ProcessEndFail : {task} Error Code : 1` |
| Fail | `ProcessEndFail` → may `RequestTaskStart(m_iFOutgoingTask)` |

Error **1** manifests as “return to NPC” in player-facing text.

---

## Why vanilla autocomplete fails

`ForceCompleteCurrentTask`:

1. Does not call `SetRemainTime(0)` or wait for expiry
2. Sends `TASK_END` immediately with `bError=false`
3. Skips `CheckToCompleteTaskCondition` gate
4. Server rejects → `ProcessEndFail err 1` → fail-outgoing loop

---

## Patch strategy (ForceCompleteV2)

From `mods/patches/cnMissionManager-forcecomplete/`:

1. **Defer** complete in `Update` while `m_fRemainTime > 0` for pending force-complete tasks
2. **Zero timer** before packet (`SetRemainTime(99999f)` or server-safe value) in `PrepareTaskForForcedComplete`
3. **Retry** on `ProcessEndFail err 1` with escalation (max attempts)
4. Log: `ForceCompleteV2: retry task N err 1 attempt M`

---

## Packet format (timer complete at expiry)

```
sP_CL2FE_REQ_PC_TASK_END
  iTaskNum = {taskId}
  iNPC_ID = 0 or terminator
  iEscortNPC_ID = 0
  iBox1Choice = 0, iBox2Choice = 0
  (bError path NOT used for natural timer expiry)
```

---

## Documenting a specific timer mission

For mission doc template, record:

- `m_iSTGrantTimer` value (seconds from server on start)
- `m_iCSUCheckTimer` value
- `m_iFOutgoingTask` (fail restart)
- Log lines: `Send Time mission.`, `ProcessEndFail … Error Code : 1`
