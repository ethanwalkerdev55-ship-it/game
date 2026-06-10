# ForceCompleteV2 — apply progress log



Search keywords: `ForceCompleteV2`, `mission autocomplete`, `cnMissionManager`, `golden DLL`, `Add Class Members`, `Edit Method`



---



## 2026-06-06 — Step 2 DONE ✓



**What:** Added fields + 7 helper methods to `cnMissionManager` via dnSpy.



**dnSpy action:** Right-click `cnMissionManager` → **Add Class Members (C#)...**



**Paste file:** `add-class-members-compile-paste.txt` (lines 7–end only)



**Members added:**

- Fields: `bForceCompleteChain`, `m_iForceCompleteChainDepth`, `m_iForceCompletePendingTaskId`, `m_iForceCompleteRetryCount`

- Methods: `GetForceCompleteTarget`, `PrepareTaskForForceComplete`, `NeedsForceCompleteTaskStart`, `ResolveForceCompleteTerminatorNpcId`, `ClearForceCompleteChain`, `RequestForceCompleteTaskEnd`, `TryForceCompleteChainTask`



---



## 2026-06-06 — Step 3 DONE ✓ (all Edit Method patches)



**dnSpy action:** Right-click each **method** → **Edit Method (C#)...** → paste → Compile.



| Method | Paste file | Result |

|--------|------------|--------|

| `Update` | `step1-Update.cs` | ✓ |

| `ProcessStartSucc` | `step2-ProcessStartSucc.cs` | ✓ |

| `ProcessStartFail` | `step3-ProcessStartFail.cs` | ✓ |

| `ProcessEndSucc` | `step4-ProcessEndSucc.cs` | ✓ |

| `ProcessEndFail` | `step5-ProcessEndFail.cs` | ✓ |

| `ForceCompleteCurrentTask` | `step6-ForceComplete-block.cs` **lines 3–19 only** | ✓ |



**Notes:**

- **ProcessEndFail:** use `SendSystemMessageBox(null, 12, ...)` not `base.gameObject` (dnSpy partial class has no `: MonoBehaviour`).

- **ForceCompleteCurrentTask:** paste **only** the public method — helpers already exist from step 2 (pasting full step6 → CS0111 duplicate).



---



## 2026-06-07 — V3 hotfix DONE ✓

**Problem:** F11 logged `request end 466` but no `ProcessEndSucc/Fail` — `RequestTaskComplete` returned silently (NPC lookup / task checker).

**Fix (V3):**
- First force-complete attempt: `RequestTaskComplete(taskId, 0, false)` — no NPC gate
- `RequestTaskComplete`: log when blocked (checker / missing task / NPC) or when packet sent
- Files: `step8-RequestTaskComplete.cs`, `add-class-members` (RequestForceCompleteTaskEnd), `PatchV3.cs`

**Deploy:** `tools\FFPatch\apply-mission-patch-v3.bat`

**Guide:** `ITERATION-PLAYBOOK.md`

**Verify log:**
- `ForceCompleteV2: complete without npc gate task …`
- `ForceCompleteV2: sent complete packet task …`
- Then `ProcessEndSucc` or `ProcessEndFail`

---

## Next (in-game test)

1. Restart game after V3 deploy
2. Single F11 on mission 504 / task 466 chain
3. Confirm log lines in § above; chain should reach 468 → 463



| # | Status | Action |
|---|--------|--------|
| 4 | DONE ✓ | Golden → `tools/FFPatch/golden/Assembly-CSharp.forcecomplete.dll` |
| 5 | DONE ✓ | `run-verify-golden.bat` (checks `GetForceCompleteTarget` / `bForceCompleteChain`) |
| 6 | DONE ✓ | `apply-mission-patch.bat` — patched DLL + bundle-inject (2026-06-06) |
| 7 | **TODO** | Restart game → test **F11** / Right Ctrl |
| 8 | TODO | Check `fusionfall_log.txt` for `ForceCompleteV2:` lines |



---



## Full guides



- `BUILD-GOLDEN.txt` — golden DLL workflow

- `tools/FFPatch/apply-mission-patch.bat` — automated apply (after golden saved)



## Avoid



- **Edit Class** on `cnMissionManager` (coroutine CS1525 / load freeze)

- `cnMissionManager-ONE-PASTE-EditClass.cs`

- Pasting full `step6-ForceComplete-block.cs` into Edit Method (CS0111 — helpers already added)



## Verify in-game



- Log: `ForceCompleteV2: start chain task` in `fusionfall_log.txt` after hotkey (F11 / key 305)



## 2026-06-09 — doc504 staging DONE ✓



**What:** Doc-driven mission completion (`EmitDocCompletePacket`) per [`MISSION-PACKET-COMPLETION.md`](../../docs/MISSION-PACKET-COMPLETION.md).



**Build stamp:** `ForceCompleteV2: patch build 2026-06-08-doc504`



**Staging:** `apply-mission-patch-staging.bat` + `run-verify-staging.bat` — all gates pass.



**Deployed 2026-06-09:** `apply-mission-patch-client-deploy.bat` — bundle **7126766** hash `394b4ded…` (+1661 B vs vanilla). `launch-patched-client.bat` run; manifest+ffcache pinned.

**Test:** Quit/reopen launcher → Connect → **Right Ctrl** (key 305) with mission selected → log `ForceCompleteV2: patch build 2026-06-08-doc504`.


