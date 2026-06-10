# Active error queue (magazine)

**Rules:** [`CONTINUOUS-ERROR-TRACKING.md`](CONTINUOUS-ERROR-TRACKING.md)

**Mission docs:** When a failure involves a mission/task, consult [`docs/missions/catalog/MISSION-{id}.md`](docs/missions/catalog/) before hypothesizing. Index: [`docs/missions/MISSION-CATALOG.md`](docs/missions/MISSION-CATALOG.md). See CONTINUOUS-ERROR-TRACKING §2.5.

- Only **one** `IN_PROGRESS` error at a time.
- New errors during verify → append here as `QUEUED`; do not switch focus.
- When focused error → `VERIFIED_FIXED`, promote the **top** `QUEUED` item.

**Current bundle (on disk):** `main.unity3d` — size=**7111202** hash=`a61b966f…` (**doc504h** 2026-06-09; SearchTableID NPC fallback; prior `0b4ae1f7…` doc504g 7100788; `lastU=49172188`)

**Strict rules:** [`STRICT-RULES.md`](STRICT-RULES.md) — amend before repeating failed deploy paths.

**Log inaccuracy (confirmed):** `Requesting to assetInfo.php` is logged in `Assembly - CSharp - first pass.dll` when the local WWW request **starts**. Failures (missing `assetInfo.php` in Unity `dataPath`, NPAPI `UrlNotify … 1`, injected bundle runtime hang) produce **no** error line — the log looks "stuck at assetInfo" but that is a symptom, not the recorded failure. `udp_listener.py` now emits `BOOTSTRAP_STALL` diagnostics; run `bootstrap-snapshot.bat` before Connect.

---

## IN_PROGRESS

### ERR-001 — Fusion Lair task 463: ProcessEndFail err 1 → Fail Outgoing 466 loop

| Field | Value |
|-------|-------|
| State | IN_PROGRESS |
| Requirement | Fusion Lair / Infected Zone (client_requirement §2) |
| Discovered | 2026-06-08 |
| Discovered while | initial intake (client log 22:34) |
| Bundle | Bootstrap deployed `b787ec06…` 7126773; mission tier pending SAFE+ staging |
| Mission ID | **504** |
| Task ID | **463** (chain: 466→468→463) |
| Mission doc | [`docs/missions/catalog/MISSION-504.md`](docs/missions/catalog/MISSION-504.md) |

**Table fields in scope:** task 463 — `m_iRequireInstanceID=12`, kill enemy **2513 ×4**, `m_iFOutgoingTask=466`; tasks 466/468 — GotoLocation.

**Fix attempts:**

| # | Date | Change | Result |
|---|------|--------|--------|
| 1 | 2026-06-08 | `chain504` logic + LOOSE deploy | Bootstrap OK; **patch not loaded** |
| 2–9 | 2026-06-08 | bundle-inject / vanilla repros | See archived rows — ERR-002 blocked deploy |
| 10 | 2026-06-09 | `EmitDocCompletePacket` + doc504 staging | PASS staging (pre-SAFE+ hooks) |
| 11–12 | 2026-06-08–09 | Vanilla repros + doc504 deploy attempt | Bootstrap tier deployed; mission hooks pending |
| 13 | 2026-06-09 | 8 helper methods + dnlib transplant | **FAIL** — DLL 1530880 (+10240); null operand on ProcessStartSucc (fixed) |
| 14 | 2026-06-09 | Consolidate to 3 methods (`ForceCompleteChainHook`) + name-based dnlib resolve | **FAIL** — DLL 1530368 (+9728); null operand fixed |
| 15 | 2026-06-09 | Next: **0 new MethodDefs** — inline IL hooks or recycle existing method slots | **queued** |
| 16 | 2026-06-09 | `PatchProcessStartSuccBlock` stale-466 guard (doc504b) | PASS staging 1520640; hotkey silent-return reported |
| 17 | 2026-06-09 | `GetSelectedActiveMission` active-list fallback + doc504c stamp (`PatchForceCompleteHotkeyFix`) | **FAIL in-game** — char select never reaches `StartSelectMission` / `CreateGameMode:24` (loading overlay stall); bootstrap OK |
| 18 | 2026-06-09 | **doc504d** — revert `GetSelectedActiveMission` dnlib sync; stamp `doc504d`; keep doc504b `ProcessStartSuccBlock` | PASS — `StartSelectMission` + `CreateGameMode:24` restored (04:34 session) |
| 19 | 2026-06-09 | **doc504e** — compact FCT: entry log at hotkey + `m_ActivateMissionList` fallback (hotkey-only, no shared method patch) | PASS staging/deploy `b70602fa…` 7111291; simple missions OK; GotoLocation/Talk silent RTC (NPC gate) |
| 20 | 2026-06-09 | **doc504f** — npc=0 `RequestTaskComplete` (bypass NPC gate); `m_iloopTemp=1` not cleared on hotkey; EndSucc auto-chain **deferred** (+512 B over 1520640 slot); stamp `doc504f` | PASS staging/deploy `cbddcfd8…` 7125461; pending in-game |
| 21 | 2026-06-09 | **doc504g** — `RequestTaskComplete(task, m_iHTerminatorNPCID, err)` on hotkey; `hotkey target task` log; ProcessStartSucc/EndFail hooks **deferred** (+512 B slot); full `EmitDocCompletePacket` inline rejected (1521152) | PASS staging/deploy `0b4ae1f7…` 7100788; **FAIL in-game** — hotkey fires but RTC silent-return (NPC not in world) |
| 22 | 2026-06-09 | **doc504h** — use terminator NPC only when `pNpcContainer.SearchTableID` succeeds; else `npc=0` (matches vanilla emit gate) | PASS staging/deploy `a61b966f…` 7111202; pending in-game |

**Hypothesis:** Doc-driven `TASK_END` per [`MISSION-PACKET-COMPLETION.md`](docs/MISSION-PACKET-COMPLETION.md). Success = **`ProcessEndSucc : 463`**, not packet emit alone.

**Next fix (in-game):** Deploy SAFE+ after staging passes; verify `doc-complete task 463` + `ProcessEndSucc : 463`; no `Fail Outgoing Task : 466`.

**Verification:**

- [x] Log shows `Resources base url` after Connect — bootstrap `b41a56ee…` (03:20–03:24 sessions)
- [x] `CreateGameMode:2` on doc504c bundle (not ERR-002 regression)
- [ ] In-game stamp: `ForceCompleteV2: patch build 2026-06-09-doc504g` after **Right Ctrl** hotkey
- [ ] Log: `hotkey target task {id}` then server `ProcessStartFail tasknum : {id}` (= END_SUCC)
- [x] Staging: `run-verify-staging.bat` pass with SAFE+ (`verify-safe-plus`) on doc504c
- [ ] Hotkey chain 466→468→463→667
- [ ] No `Fail Outgoing Task : 466` during chain
- [ ] `ProcessEndSucc : 463`

---

## VERIFIED_FIXED

### ERR-002 — bundle-inject / patched bundle cannot load in game

| Field | Value |
|-------|-------|
| State | VERIFIED_FIXED |
| Requirement | Game must load (client_requirement implicit) |
| Discovered | 2026-06-07 |
| Discovered while | ERR-001 (during deploy verify) |
| Bundle | UnityWeb shrink `1f5c728b…` 7105691; UnityRaw `db13794a…` 49182491 **rejected** |
| Mission ID | — (load blocker) |
| Task ID | — |
| Mission doc | — |

**Root cause (confirmed):** Any DLL edit → `bundle-inject` LZMA recompress (~19 KB shrink). UnityWeb shrunk bundle: bootstrap stall. **UnityRaw: launcher `Invalid data file`** (2026-06-08 ~23:12). Static verify gates pass; runtime rejects.

**Fix attempts:**

| # | Date | Change | Result |
|---|------|--------|--------|
| 1 | 2026-06-07 | Size gate in deploy script | Blocks bad deploy; no inject fix |
| 2 | 2026-06-08 | 98% min-size gate | Deploy allowed; still runtime fail |
| 3 | 2026-06-08 | manifest + ffcache sync | CDN revert fixed; inject still stalls |
| 4 | 2026-06-08 | offset-map + LZMA-props in `disunity.jar` | **No output change** — same `1f5c728b…` |
| 5 | 2026-06-08 | `bundle-inject-raw` (UnityRaw) | **FAIL** — `Invalid data file` on ACCESSING |
| 6 | 2026-06-08 | `restore-client.bat` after raw deploy | Vanilla restored; bootstrap OK |
| 7 | 2026-06-09 | Compact patch + surgical RTC + `bundle-inject-preserve` tooling | Partial — tooling broken (javac/inject order) |
| 8 | 2026-06-09 | **dnlib `PreserveRids` write** (DLL exactly 1520640) + slot-pad + UnityRaw preserve-inject (`u=49172188`) | **FAIL** — `Invalid data file` on ACCESSING (UnityRaw rejected) |
| 9 | 2026-06-09 | Same slot-fit + **UnityWeb** save in `BundleInjectPreserve` (7126773, +1668 vs client) | **VERIFIED** — in-game load OK, `ForceCompleteV2: patch build 2026-06-08-doc504` |

**Verification:**

- [x] Patched bundle loads past ACCESSING (no `Invalid data file`)
- [x] `Resources base url` after Connect on **patched** hash
- [x] `CreateGameMode:2` on patched hash
- [x] `ForceCompleteV2:` visible after patch load

**Notes:** Closed 2026-06-09. Durable path: `bundle-inject-preserve` + dnlib `PreserveRids` at 1520640.

---

## QUEUED (magazine — work top to bottom)

### ERR-003 — Timer missions: autocomplete fails / return to NPC (error 1)

| Field | Value |
|-------|-------|
| State | QUEUED |
| Requirement | timer (client_requirement §1) |
| Discovered | initial intake |
| Discovered while | initial intake |
| Bundle | client `main 2.unity3d` |
| Mission ID | *(fill on repro — pick from timer list in MISSION-CATALOG)* |
| Task ID | *(fill on repro)* |
| Mission doc | [`docs/missions/MISSION-CATALOG.md`](docs/missions/MISSION-CATALOG.md) § Timer missions |

**Evidence:** Customer requirement; client `PrepareTaskForForcedComplete` sets `SetRemainTime(99999f)` — needs dedicated repro on timer task.

**Hypothesis:** Timer not zeroed/deferred before complete packet; server returns error 1.

**Verification:**

- [ ] Named timer mission ID tested; mission doc consulted (`m_iSTGrantTimer` / `m_iCSUCheckTimer`)
- [ ] No "return to NPC" / error 1 without retry
- [ ] Log shows timer defer or forced complete path

---

## PARKED

*(none)*

---

## VERIFIED_FIXED (recent)

Move cards here when closed; copy summary to `PATCH-ERROR-LOG.md` if process mistake; detail to `ERROR-TRACKER.md`.

### ERR-000 — Workspace / wrong bundle lineage (vanilla ClientFile vs client main 2)

| Closed | 2026-06-08 |
|--------|------------|
| Fix | `ingest-client-bundle.bat`; cleanup; decompile to `mods/decompiled`; client `ClientMod` + noclip confirmed in DLL |

---

## ID counter

Next ID: **ERR-004**
