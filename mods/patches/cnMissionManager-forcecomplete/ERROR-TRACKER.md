# Error tracker — mission autocomplete

Living log. Update on every staging run, deploy attempt, or client log analysis.

**Process:** Order of work and focus rules → [`../../CONTINUOUS-ERROR-TRACKING.md`](../../CONTINUOUS-ERROR-TRACKING.md)  
**Active queue:** [`../../ACTIVE-ERROR-QUEUE.md`](../../ACTIVE-ERROR-QUEUE.md)

**Client requirements (unchanged):**
1. Timer missions — autocomplete must not fail with “return to NPC” / error 1
2. Fusion Lair / Infected Zone — must not loop until player enters zone
3. Game must load (no black screen)

**Test mission:** 504 → tasks **466 → 468 → 463**

---

## Status dashboard

| Requirement | Status | Blocker |
|-------------|--------|---------|
| Game loads | **OK** (SAFE fixed DonorCompile ref) | — |
| Hotkey / chain | **OK** | `ForceCompleteV2: start chain task` in 06:32 log |
| Fusion Lair 463 | **PENDING VERIFY** | SAFE+ adds `complete after start 463` + `instance zone complete` |
| Timer retry | **PENDING VERIFY** | SAFE+ `ForceCompleteOnEndFail` on err 1 |
| 466↔463 loop | **FIX PENDING** | Stale `m_iForceCompletePendingTaskId` (468) blocked `ForceCompleteOnStartSucc(463)` |

**Current live bundle:** tier **SAFE++++++** (`hash=f4f5f4c0…`, size=7,100,580, build `2026-06-07-fct-rtc2`).

**Launcher manifest:** must be `file:///D:/work/roberto/6877b37c-e9cd-4826-b82c-5e8d3d5db744/main.unity3d` — **NOT** CDN. Run `verify-deploy-manifest.bat` before every test.

**Rollback:** `tools/FFPatch/restore-client.bat`

---

## Log evidence — 2026-06-07 05:32 (client backup, tier 0)

```
ProcessStartSucc : 463
ProcessEndFail : 463 Error Code : 1
Fail Outgoing Task : 466          ← vanilla restart causes loop
ProcessStartSucc : 466
ProcessStartFail tasknumber : 463
```

**Diagnosis:** Vanilla autocomplete sends complete without `bError:true` for instance task 463. Server rejects (error 1). Vanilla fail-outgoing restarts 466 → loop.

**Fix deployed:** Tier 2 — LITE helpers + surgical `ProcessEndFail` hook.

---

## Log evidence — 2026-06-07 06:32 (SAFE tier, pre-SAFE+)

Patch active; fail-outgoing guard worked (no `Fail Outgoing Task : 466`). Still failing:
- `ProcessEndFail : 463 Error Code : 1` — no retry
- `ProcessStartSucc : 466` — server/client restarts 466 without `instance zone complete task 463`
- Never saw `complete after start 463`

**SAFE+ fix:** `ForceCompleteOnEndFail` + `ForceCompleteOnStartSucc` surgical hooks.

---

## Log evidence — 2026-06-07 06:39 (SAFE+ live)

```
ForceCompleteV2: start chain task 468
ForceCompleteV2: complete without npc gate task 468
ProcessStartFail tasknum : 468
Send Start Mission : 463
ProcessStartSucc : 463
ProcessStartSucc
TaskNode.GetMe().m_iSTGrantWayPoint = false
ProcessStartSucc : 466          ← 14ms later, spurious fail-outgoing restart
ProcessStartFail tasknumber : 463
```

**Missing:** `ForceCompleteV2: complete after start 463`, `instance zone complete task 463`.

**Root cause:** `ForceCompleteOnStartSucc` returned early because `m_iForceCompletePendingTaskId` was still **468** (stale from prior chain step). Guard `pending != iTaskNum` blocked instance complete for 463.

**SAFE++ fix:**
1. Clear stale pending in `ForceCompleteOnStartSucc` instead of returning.
2. `PatchProcessStartSuccBlock` — block `ProcessStartSucc : 466` when `bForceCompleteChain && m_iForceCompleteChainDepth > 0`.
3. Move chain hook anchor to after `Logger.Log("ProcessStartSucc ")`.

---

## Log evidence — 2026-06-07 06:49 (SAFE++ live)

```
ForceCompleteV2: complete without npc gate task 468
ProcessStartFail tasknum : 468          ← ProcessEndSucc (468 end OK)
Send Start Mission : 463                ← vanilla ProcessEndSucc outgoing start
ProcessStartFail tasknumber : 463       ← instance start fail, no handler
```

**Missing:** `ForceCompleteV2: advance to task 463`, `instance start fail complete task 463`, `instance zone complete task 463`.

**Root cause:** SAFE++ never hooked `ProcessEndSucc` or `ProcessStartFail`. Vanilla `RequestTaskStart(463)` fails outside Fusion Lair; chain handlers never run.

**SAFE+++ fix:** `ForceCompleteOnEndSucc` + `ForceCompleteOnStartFail` surgical hooks; `NeedsForceCompleteTaskStart` skips start when outside instance.

---

## Log evidence — 2026-06-07 07:06 (SAFE+++ live) — **V3 recurrence**

```
ForceCompleteV2: start chain task 466
ForceCompleteV2: request end 466
ForceCompleteV2: complete without npc gate task 466
(repeated F11 — no ProcessEndSucc/Fail, no advance)
```

**Missing (entire log file):** `ForceCompleteV2: sent complete packet task …`, `complete blocked …`

**Root cause:** SAFE tier never transplanted `step8-RequestTaskComplete.cs`. Vanilla `RequestTaskComplete` returns silently on `SearchMissionTaskChecker` — same incident as **V3 / V11** (`PROGRESS-LOG.md`, `ITERATION-PLAYBOOK.md`).

**SAFE++++ fix:** Transplant **only** `RequestTaskComplete` from donor (checker bypass + diagnostic logs). Not a full Process* transplant — load-safe.

---

## Log evidence — 2026-06-07 07:12 (SAFE++++ live) — **RTC still silent**

```
ForceCompleteV2: complete without npc gate task 468  (×4 F11)
```

**Still missing:** `RequestTaskComplete enter`, `sent complete packet`, any `complete blocked`.

**Root cause (staging audit):** `PatchSafeClient` ran `PatchRequestTaskCompleteChainBypass` **after** step8 transplant — IL patch corrupts RTC control flow. Strings present in PE (verify passed) but runtime never reaches `sent` log.

**SAFE+++++ fix:**
1. Skip IL chain bypass when step8 RTC string present in DLL.
2. step8: unconditional `sent complete packet` log + `RequestTaskComplete enter` + checker bypass log.
3. `verify-il`: accept step8 RTC OR IL depth bypass.

---

## Log evidence — 2026-06-07 07:18 — **manifest not loading local patch**

07:12–07:18 logs still show only `complete without npc gate` — no `RequestTaskComplete enter`, no `patch build`.

**Root cause (confirmed):** OpenFusionLauncher manifest was pointing at **CDN vanilla** `main.unity3d`:
- `main_file_url`: `http://cdn.ffretrobution.net/.../main.unity3d`
- hash `ba99b964…` size **6,993,722** (vanilla)

Local patched bundle was **7,115,310** bytes (`b064cbfd…`) — **never loaded by launcher**. Deploy script updated manifest, but launcher/sync reverted it to CDN.

**Fix:** `fix-launcher-manifest.bat` + `verify-deploy-manifest.bat` (now runs at end of deploy). Build stamp log: `ForceCompleteV2: patch build 2026-06-07-rtc-step8`.

---

## Log evidence — 2026-06-07 14:40 (`inline-send` — regression: no npc gate at all)

```
patch build 2026-06-07-inline-send
start chain task 468
request end 468
```

**Missing:** `complete without npc gate`, `sent complete packet` — worse than refresh-helpers build.

**Root cause (documented since 2026-06-06, PATCH-ERROR-LOG + PatchV12):** `TryForceCompleteChainTask` called `RequestForceCompleteTaskEnd(task)` after `PrepareTaskForForceComplete` / timer zeroing. RFTC re-checks `task.GetMe()` which is **null** → silent return. `request end` uses cached `me`; everything after the RFTC call never runs.

**Fix:** V12 pattern — inline `SendPacket` in `TryForceCompleteChainTask` using cached `me.m_iHTaskID`, never call RFTC from hotkey path. Build stamp: `2026-06-07-no-getme`. Hash: `abb18e5c…`.

---

## Log evidence — 2026-06-07 14:25 (`direct-send` — send path still silent)

```
ForceCompleteV2: patch build 2026-06-07-direct-send
ForceCompleteV2: start chain task 468
ForceCompleteV2: request end 468
ForceCompleteV2: complete without npc gate task 468
```

**Missing:** `ForceCompleteSendTaskEnd enter`, `sent complete packet`, any `ProcessEnd*`.

**Analysis (not the same bug as manifest-only):** Entire `fusionfall_log.txt` history has **zero** `sent complete packet` lines — the send helper never logged at runtime despite `complete without npc gate` firing many times. Cecil dump of on-disk bundle **after** this test showed correct IL (`RequestForceCompleteTaskEnd` → `ForceCompleteSendTaskEnd`), so either (a) 14:25 runtime bundle was an intermediate build before direct-send path was fully wired, or (b) launcher manifest had reverted to CDN again between deploy and test (manifest was CDN when re-checked).

**Fix (2026-06-07 refresh-helpers):**
- Always **refresh** helper method bodies from donor each build (not skip-if-exists).
- `ForceCompleteSendTaskEnd enter task` log as first line in send helper.
- New `verify-direct-send` gate: RFTC must call FST, not RTC.
- Build stamp: `2026-06-07-refresh-helpers`. Deploy hash: `43fabde3…` size 7,117,748.
- Run `launch-patched-client.bat` before every test (manifest keeps reverting to CDN on launcher sync).

**Expected log on success (one F11 on 468):**
```
patch build 2026-06-07-refresh-helpers
start chain task 468
request end 468
complete without npc gate task 468
ForceCompleteSendTaskEnd enter task 468
sent complete packet task 468 npc 0 err False
ProcessEndSucc or ProcessEndFail …
```

---

## Log evidence — 2026-06-07 09:27 (manifest fixed — RTC call not reaching step8)

```
ForceCompleteV2: patch build 2026-06-07-rtc-step8
ForceCompleteV2: calling RequestTaskComplete task 468
```

**Missing:** `RequestTaskComplete enter task`, `sent complete packet`.

**Root cause:** Helpers cloned **before** RTC body transplant held stale `MethodReference` to pre-step8 `RequestTaskComplete` (vanilla silent return). `calling` log proved the call was issued but wrong target executed.

**Fix:** `PatchRetargetRequestTaskCompleteCalls` after RTC transplant; unconditional RTC enter log. Build stamp: `rtc-retarget`.

---

## Acceptance log (post rtc-retarget deploy)

| Check | Expected | Actual |
|-------|----------|--------|
| Game reaches MainGame | `CreateGameMode` / gameplay logs | — |
| Patch active | `ForceCompleteV2: start chain task` | — |
| Instance 463 | `instance zone complete task 463` | — |
| Error 1 retry | `retry task 463 err 1 attempt 2+` | — |
| No 466 loop | No `Fail Outgoing Task : 466` during chain | — |

---

## Incident history

| Date | Tier | Symptom | Root cause | Prevention |
|------|------|---------|------------|------------|
| 2026-06-06 | dnSpy full class | Load freeze | Coroutine recompile | Edit Method only |
| 2026-06-07 | Customer V2 | Black screen | Full Process* + RequestTaskComplete transplant | Tier table; staging |
| 2026-06-07 | apply-client-v2 on client base | Black screen | Same — verify-il insufficient | verify-load-safe |
| 2026-06-07 | Rollback into recompressed bundle | Character Creation stall | inject without main.bak restore | restore-client first |
| 2026-06-07 | LITE deployed | Unknown load | Not validated before V2 superseded | Staging-only workflow |
| 2026-06-07 | Agent conduct | Client asked to revert/test | Deploy without load proof | OPERATING-RULES §7 |
| 2026-06-07 | verify-load-safe false positive | Staging rejected | Branch operands compared by reference | Fixed instruction-index compare |
| 2026-06-07 | Tier 2 + SAFE deploy | **Black screen** | Patched DLL had `DonorCompile` assembly ref — Unity can't load it | Fixed `IlTransplant`; added `verify-no-donor-refs` gate |
| 2026-06-07 | SAFE / SAFE+++ | `complete without npc gate` × N, no server ack | `RequestTaskComplete` left vanilla — V3 fix not applied | Transplant step8 only; verify `sent complete packet` |
| 2026-06-07 | SAFE++++ | step8 in PE, still silent at runtime | `PatchSafeClient` IL bypass corrupted RTC after transplant | Skip IL bypass when step8 present |
| 2026-06-07 | All 07:06–07:18 tests | Same `complete without npc gate` loop | **Launcher manifest pointed at CDN vanilla**, not local patched bundle | `verify-deploy-manifest.bat` gate |

---

## Staging run log

| Date | Script | verify-il | verify-load-safe | Deploy | Notes |
|------|--------|-----------|------------------|--------|-------|
| 2026-06-07 | `apply-mission-patch-staging.bat --tier1` | PASS | FAIL (fixed) | — | Branch compare bug |
| 2026-06-07 | `apply-mission-patch-staging.bat --tier2` | PASS | PASS | YES | ProcessEndFail hook present |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --tier2` | PASS | PASS | LIVE **BROKE LOAD** | Black screen — restored tier 0 |
| 2026-06-07 | `apply-mission-patch-staging.bat --safe` | PASS | PASS | staging | Surgical ProcessEndFail guard |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` | PASS | PASS | LIVE | Game loads; 463 still loops |
| 2026-06-07 | `apply-mission-patch-staging.bat --safe` (SAFE+) | PASS | PASS + no-donor | staging | Chain handler hooks |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` (SAFE+) | PASS | PASS | LIVE | manifest `ebbf0bdb…` |
| 2026-06-07 | `apply-mission-patch-staging.bat --safe` (SAFE++) | PASS | PASS + no-donor + safe-plus | staging | stale pending fix + 466 block |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` (SAFE++) | PASS | PASS | LIVE | manifest `4344d15a…` |
| 2026-06-07 | `apply-mission-patch-staging.bat --safe` (SAFE+++) | PASS | PASS + safe-plus | staging | EndSucc + StartFail chain hooks |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` (SAFE+++) | PASS | PASS | LIVE | manifest `57bddb8c…` |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` (SAFE++++) | PASS | PASS + sent-packet gate | LIVE | manifest `db796ffb…` — step8 RTC |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` (SAFE++++) | PASS | PASS + enter log | LIVE | manifest `b064cbfd…` — skip IL bypass |
| 2026-06-07 | `apply-mission-patch-client-deploy.bat --safe` (fct-rtc2) | PASS | PASS + host-diag + bool-arg | LIVE | manifest `f4f5f4c0…` — fix inline bool IL |

---

## Log evidence — 2026-06-07 14:55 (fct-rtc, broken IL)

```
ForceCompleteV2: patch build 2026-06-07-fct-rtc
ForceCompleteV2: start chain task 468
ForceCompleteV2: hotkey RequestTaskComplete task 468
(no RequestTaskComplete enter / no sent complete packet)
```

**Root cause:** DonorCompile emitted broken IL for `RequestTaskComplete(taskId, 0, 0 < me.m_iRequireInstanceID)` — `ldc.i4.0` pushed before `clt`, so the call threw `InvalidProgramException` silently.

**Fix (fct-rtc2):** Explicit `bool sendError = 0 < me.m_iRequireInstanceID` local in FCT + all chain paths. Verify gate `CheckRequestTaskCompleteBoolArg`. IL dump confirms `ldloc.3` as 3rd arg before `call RequestTaskComplete`.

**Host-response diag:** `PatchHostResponseDiag` injects `ForceCompleteV2: host ProcessEndSucc task N` and `ForceCompleteV2: host ProcessEndFail task N err E` at top of vanilla handlers — so we see server reply even when vanilla logs use confusing names.

---

## Next engineering actions

1. [x] Fix DonorCompile assembly ref (`verify-no-donor-refs`)
2. [x] Game loads on SAFE tier
3. [x] SAFE+ chain handlers deployed
4. [x] Diagnose stale pending blocking `ForceCompleteOnStartSucc(463)` (06:39 log)
5. [x] SAFE++ staging + verify gates + deploy (`hash=4344d15a…`)
6. [x] Diagnose missing ProcessEndSucc/ProcessStartFail hooks (06:49 log)
7. [x] SAFE+++ deploy (`hash=57bddb8c…`)
8. [x] V3 recurrence diagnosed (07:06 log — no `sent complete packet`)
9. [x] SAFE++++ deploy — step8 `RequestTaskComplete` transplant (`hash=db796ffb…`)
10. [x] 07:12 log — RTC strings in PE but silent at runtime (IL bypass corrupt)
11. [x] SAFE+++++ deploy — skip IL bypass on step8 RTC (`hash=b064cbfd…`)
12. [x] Diagnose fct-rtc silent RTC — inline bool arg broke IL stack
13. [x] fct-rtc2 deploy + host response logging (`hash=f4f5f4c0…`)
14. [ ] Log shows `RequestTaskComplete enter` + `sent complete packet` + `host ProcessEndSucc` or `host ProcessEndFail`
15. [ ] Log shows `advance to task 463` + `instance zone complete task 463`
16. [ ] Log shows `instance start fail complete task 463` (fallback path)
17. [ ] Log shows `retry task 463 err 1 attempt 2+` on error 1
18. [ ] No `ProcessStartSucc : 466` during active chain after 463 handled
