# ForceComplete V2 — root cause and fix (2026-06-07)

## Log evidence (`fusionfall_log.txt` ~23:56)

Fusion Lair chain **466 → 468 → 463** with F11:

| Symptom | Log line |
|---------|----------|
| Task 463 fails (not in instance) | `ProcessEndFail : 463 Error Code : 1` |
| Retries never escalate | `retry task 463 err 1 attempt 1` (always 1) |
| Spurious restart of 466 | `ForceCompleteV2: complete after start 466` during 463 handling |
| Infinite loop | 468 → 463 → fail → 466 → 468 … |

## Root causes

### 1. Fusion Lair / Infected Zone (task 463, `m_iRequireInstanceID > 0`)

Vanilla `CheckWarpAllMision` completes instance tasks with `RequestTaskComplete(taskId, 0, **bError: true**)` when warping into the zone. Force-complete used `bError: false`, so the server rejects with error **1** (“return to NPC”) when the player is outside the instance.

**Fix:** `RequestForceCompleteTaskEnd` — if `m_iRequireInstanceID > 0`, call `RequestTaskComplete(id, 0, true)` (same as warp path).

### 2. Infinite loop / spurious task 466

`ProcessStartSucc` hook used:

```csharp
if (chain && (pending == task || chainDepth > 0))
```

When 463 failed, vanilla/server restarted fail-outgoing task **466**. Because `chainDepth > 0`, force-complete ran on **466** (`complete after start 466`), advancing the chain again → loop.

**Fix:** Only hook when `m_iForceCompletePendingTaskId == iTaskNum`.

### 3. Timer missions / retries stuck at attempt 1

Before every `RequestForceCompleteTaskEnd` in `ProcessStartSucc`, the patch reset `m_iForceCompleteRetryCount = 0`, so `ProcessEndFail` retries never reached tiers 2–3 (`retry without npc`, `retry with error flag`).

**Fix:** Remove retry reset in `ProcessStartSucc`. Optional: defer timer tasks via `Update()` (see step1/step2/step3/step5).

## What was deployed

Automated **Cecil V2 hotfix** on golden DLL (`FFPatch patch-v2`):

- Instance zone `bError` path in `RequestForceCompleteTaskEnd`
- `ProcessStartSucc` pending-only guard + retry reset removed

Deploy:

```bat
D:\work\roberto\tools\FFPatch\apply-mission-patch-v2.bat
```

Or: `patch-v2` golden, then `apply-mission-patch.bat`.

## Full timer defer (optional dnSpy pass)

For best timer behaviour, re-edit golden with updated step files:

- `step1-Update.cs` — pending timer includes `m_iCSUCheckTimer`
- `step2-ProcessStartSucc.cs` — wait timer / pending defer
- `step3-ProcessStartFail.cs`, `step5-ProcessEndFail.cs` — timer retry defer
- `add-class-members-compile-paste.txt` — `RequestForceCompleteTaskEnd` instance path (already in Cecil patch)

Save golden → `apply-mission-patch.bat`.

## Expected log after fix

- `ForceCompleteV2: instance zone complete task 463`
- No `complete after start 466` while handling 463
- Retries: `attempt 2`, `attempt 3`, … and `retry without npc` / `retry with error flag`
