# Mod changelog

## 2026-06-07 — Tier 2 staging workflow + deploy (LITE + ProcessEndFail)

**Workflow:** Staging-only patch (`apply-mission-patch-staging.bat`), `verify-load-safe` (Update IL unchanged), then `apply-mission-patch-client-deploy.bat --tier2`.

**Fixes:** Fusion Lair `bError:true` via `RequestForceCompleteTaskEnd`; timer/instance retry in `ProcessEndFail`; blocks vanilla `Fail Outgoing Task` during force-complete chain.

**Docs:** `OPERATING-RULES.md`, `ERROR-TRACKER.md`

**Rollback:** `tools/FFPatch/restore-client.bat`

---

## 2026-06-07 — ForceComplete V2 on client bundle (timer + Fusion Lair/IZ)

**Base:** `ClientFile/main.unity3d` → `_inspect_bundle/client/` (preserves UdpLogger in first pass DLL)

**Fixes:** Instance tasks use `bError:true`; ProcessEndFail retry escalation; ProcessStartSucc pending-only guard; no instance-zone wait loop; timer retry calls `RequestForceCompleteTaskEnd` when task active.

**Deploy:** `tools/FFPatch/apply-mission-patch-client.bat`

**Log markers:** `ForceCompleteV2: start chain task`, `instance zone complete`, `retry task … attempt N`, `advance to task`

---

## 2026-06-07 — ForceComplete V3 (NPC gate + diagnostics)

**Problem:** F11 logged `request end` but no server response — `RequestTaskComplete` exited silently (NPC lookup / pending checker).

**Fix:** First complete uses `npc 0`; log blocked/sent paths in `RequestTaskComplete`.

**Deploy:** `tools/FFPatch/apply-mission-patch-v3.bat`

**Docs:** `ITERATION-PLAYBOOK.md`, `ROOT-CAUSE-V2.md`, `step8-RequestTaskComplete.cs`

**Log markers:** `complete without npc gate`, `sent complete packet`, `complete blocked …`

---

## 2026-06-07 — ForceComplete V2 (instance zone + chain guard)

**Fix:** Instance tasks use `bError: true`; ProcessStartSucc pending-only guard; retry count no longer reset on start.

**Cecil hotfix:** `tools/FFPatch/PatchV2.cs`

---

## 2026-06-06 — Mission autocomplete (ForceCompleteV2)

**Goal:** Fix mission hotkey autocomplete for timer missions and Fusion Lair / Infected Zone tasks.

**Changed class:** `cnMissionManager` in `Assembly - CSharp.dll`

**Cursor source:** `mods/decompiled/Assembly-CSharp/cnMissionManager.cs`

**dnSpy apply file:** `mods/patches/cnMissionManager-forcecomplete/` (steps 0–6)

**Recommended workflow:** See `mods/patches/cnMissionManager-forcecomplete/WORK-METHODS.md` (full methods log). Deploy: `tools/FFPatch/apply-mission-patch.bat`

**DO NOT USE:** `cnMissionManager-ONE-PASTE-EditClass.cs` (compiles but freezes game at load)

**Status:** Patch applied to game bundle (2026-06-06). See `WORK-METHODS.md` + `PROGRESS-LOG.md`.

**Verify:** `ForceCompleteV2` strings in DLL; log lines after hotkey (F11 / key 305).

**Backups:** `_inspect_bundle/main.bak/Assembly - CSharp.dll`, `6877b37c-.../main.unity3d.bak`
