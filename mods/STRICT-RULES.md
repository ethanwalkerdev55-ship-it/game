# Strict rules — deploy, bundle inject, and error tracking

**Authority:** These rules override ad-hoc fixes. When a rule is wrong, **amend this file first** (see §1), then change tooling. Do not repeat a failed deploy path because “verify gates passed.”

**Related docs:**

| Doc | Role |
|-----|------|
| [`CONTINUOUS-ERROR-TRACKING.md`](CONTINUOUS-ERROR-TRACKING.md) | Focus, magazine FIFO, verification gates |
| [`ACTIVE-ERROR-QUEUE.md`](ACTIVE-ERROR-QUEUE.md) | Live `ERR-###` cards |
| [`patches/cnMissionManager-forcecomplete/OPERATING-RULES.md`](patches/cnMissionManager-forcecomplete/OPERATING-RULES.md) | Staging tiers, golden lineage |
| [`patches/cnMissionManager-forcecomplete/PATCH-ERROR-LOG.md`](patches/cnMissionManager-forcecomplete/PATCH-ERROR-LOG.md) | Permanent mistake record |

---

## 1. How to correct these rules (mandatory process)

When a deploy or test produces a **new failure mode** (symptom or root cause not already covered):

| Step | Action | Owner |
|------|--------|-------|
| 1 | **Do not redeploy** the same bundle path until the card is updated | Agent |
| 2 | Append row to [`PATCH-ERROR-LOG.md`](patches/cnMissionManager-forcecomplete/PATCH-ERROR-LOG.md) — symptom, root cause, prevention | Agent |
| 3 | Update focused `ERR-###` in [`ACTIVE-ERROR-QUEUE.md`](ACTIVE-ERROR-QUEUE.md) — fix attempt row + result | Agent |
| 4 | **Amend this file** — add or tighten one rule in §2–§5; cite `ERR-###` and date in the new bullet | Agent |
| 5 | If the failure is a **hard stop** (§2.4), promote the blocking `ERR-###` to `IN_PROGRESS` per [`CONTINUOUS-ERROR-TRACKING.md` §5.2](CONTINUOUS-ERROR-TRACKING.md) | Agent |
| 6 | Only after steps 1–5: implement tooling change + staging test | Agent |

**Forbidden:** Closing an error, changing `apply-mission-patch-client-deploy.bat`, or asking the user to Connect with a new bundle hash **without** updating steps 2–4.

**Regression:** If a `VERIFIED_FIXED` item fails again on the same test plan, reopen as `REGRESSED` `ERR-###` and add a **REGRESSION** bullet under the relevant §2 rule.

---

## 2. Bundle and deploy rules (ERR-002)

### 2.1 Format

- FusionFall accepts **UnityWeb only** (LZMA-compressed `main.unity3d`).
- **Forbidden:** `bundle-inject-raw` / `UnityRaw` / uncompressed bundles — launcher shows **`Invalid data file`** on ACCESSING (confirmed 2026-06-08, `ERR-002` attempt #7).

### 2.2 Inject behavior (bisected)

| Operation | Size vs client (7125105) | Game result |
|-----------|--------------------------|-------------|
| `bundle-extract` → `bundle-inject` **no DLL change** | **Bit-identical** | Load OK |
| **Any** DLL byte change → `bundle-inject` | **Shrinks** ~19–24 KB (`1f5c728b…` / `7105691`) | Bootstrap stall or hang |
| `verify-il` / `verify-bundle-patch` / `debug-deserializer` on shrunk bundle | Pass | **Not sufficient** — runtime still fails |

**Rule:** Static DLL gates **do not** authorize client deploy. In-game bootstrap is the gate.

### 2.3 Pre-client deploy checklist (agent runs all)

Before `apply-mission-patch-client-deploy.bat` touches the game cache:

1. [ ] Staging passes `run-verify-staging.bat`
2. [ ] **Round-trip test** on copy of client bundle: inject with **no** DLL change → size/hash unchanged
3. [ ] **Inject test** on copy: staged DLL → record size delta; if shrink &gt; 2%, treat as **ERR-002** (do not deploy)
4. [ ] `verify-bundle-layout.bat` reports `lastU=49172188` (matches client)
5. [ ] After deploy: `bootstrap-preflight.bat` + `launch-patched-client.bat` + `bootstrap-snapshot.bat`

### 2.4 Hard stop — game must load before mission work

If **any** of these occur on a patched bundle:

- `Invalid data file` (ACCESSING)
- No `Resources base url` within ~60s of `Requesting to assetInfo.php`
- `CreateGameMode:1` stuck (no `CreateGameMode:2` / in-world)

Then:

1. Run `restore-client.bat` immediately
2. Promote **`ERR-002`** (or new load `ERR-###`) to **`IN_PROGRESS`**
3. **Block** `ERR-001` mission verification until load error is `VERIFIED_FIXED` or `PARKED` with documented unblock (e.g. client-provided bundle)

### 2.5 Approved patch load paths (ordered)

| Priority | Path | Status |
|----------|------|--------|
| 1 | Client-provided pre-patched `main.unity3d` → `ingest-client-bundle.bat` | Not yet received |
| 2 | UnityWeb inject that passes **in-game** bootstrap (size/hash recorded) | **Superseded** by preserve-inject |
| 3 | **`bundle-inject-preserve`** — dnlib slot-fit DLL (1520640) + entry padding + **UnityWeb** LZMA save with `lastU` layout gate | **VERIFIED 2026-06-09** — bootstrap `7126773` `b787ec06…`, in-game load OK |
| **Forbidden** | LOOSE deploy for mission acceptance | Bootstrap OK; **patch never loads** |
| **Forbidden** | UnityRaw deploy | **Invalid data file** |
| **Forbidden** | Cecil `Write` on client DLL | Always **+4096** bytes — breaks slot |
| **Forbidden** | `dll-slot-fit` / PE truncation | Corrupts metadata |

---

## 3. Mission patch rules (ERR-001) — doc-driven packet completion

**Spec:** [`docs/MISSION-PACKET-COMPLETION.md`](docs/MISSION-PACKET-COMPLETION.md)

Autocomplete is **not** a single `RequestTaskComplete(id, 0, false)`. It is a **per-task packet matrix** from the mission catalog:

1. **Lookup first** — open `MISSION-{id}.md` for the failing task; record terminator NPC, instance ID, quotas, chain edges.
2. **Build `TASK_END` explicitly** — every field in the catalog § Server packets (`iTaskNum`, `iNPC_ID` runtime, `iEscortNPC_ID`, box bits, `bError`).
3. **Prep client state** — zero correct kill/item slots, timers, active flag; resolve terminator NPC table ID → runtime `Status.iID`.
4. **Success = server accept** — log must show **`ProcessEndSucc`** for the task (or doc-approved abort that advances chain), not merely “packet sent.”
5. **Block vanilla fail-outgoing** — while `bForceCompleteChain`, `ProcessEndFail` err 1/12 must **not** run `RequestTaskStart(m_iFOutgoingTask)` (e.g. 466); retry per doc outside-zone row (`bError=true`, `iEscortNPC_ID=-1`).
6. **Do not remove missions** — fulfill or correctly abort per table; server rejects incomplete packets with error 1.

**Mission 504 primary chain:** 466 → 468 → 463 → 667 ([`MISSION-504.md`](docs/missions/catalog/MISSION-504.md)).

| Log signal | Interpretation |
|------------|----------------|
| No `ForceCompleteV2:` | **Vanilla** — patch not loaded; auto-chain loops are expected ERR-001 repro |
| `ForceCompleteV2: patch build 2026-06-08-doc504` | Doc-driven completion build (ERR-001 fix attempt #10) |
| `ProcessEndFail : 463 Error Code : 1` + `Fail Outgoing Task : 466` | Server rejected end packet — wrong zone/quota/`bError`/NPC |
| `ProcessEndSucc : 463` | ERR-001 fix verified for instance step |

Vanilla auto-chain on login (23:39–23:40) is **valid repro**, not “working correctly.” Connect/load can be fine while mission logic loops.

---

## 4. Agent vs user responsibilities

| Task | Agent | User |
|------|-------|------|
| Staging, verify-il, inject tests on copies | **Yes** | No |
| `apply-mission-patch-client-deploy.bat` / `restore-client.bat` | **Yes** | No |
| `launch-patched-client.bat`, manifest/ffcache sync | **Yes** | No |
| Update `ACTIVE-ERROR-QUEUE.md`, `PATCH-ERROR-LOG.md`, this file | **Yes** | No |
| Quit/reopen launcher, Connect, hotkey, observe gameplay | No | **Yes** |
| Report screenshot / log symptom | No | **Yes** |

**Rule:** Never instruct the user to run deploy scripts the agent can run locally.

**Rule:** Agent output must not request anything from the user — no asks to run steps, confirm choices, paste logs, or approve next work. The user performs **verification only** (in-game Connect, hotkey, observe, report symptoms); all staging, deploy, tooling, and documentation updates are agent-owned.

---

## 5. Log interpretation (do not misread)

| Log line | Meaning |
|----------|---------|
| `Requesting to assetInfo.php` | WWW **start** only — not success or failure |
| `Resources base url` within ~60ms | Bootstrap **OK** |
| `Invalid data file` (launcher UI) | Bundle format rejected — **ERR-002** |
| No `ForceCompleteV2:` | Patch **not** in loaded DLL |
| `ProcessStartFail tasknum : 466` then `Send Start Mission : 468` | Client chain skip per MISSION-504 table |
| `ProcessEndFail : 463 Error Code : 1` + `Fail Outgoing Task : 466` | Instance gate fail — **ERR-001** repro |

---

## 6. Current focus (updated 2026-06-09)

| ID | State | Blocker |
|----|-------|---------|
| **ERR-002** | **VERIFIED_FIXED** | Bootstrap preserve-inject loads in game (`ForceCompleteV2: patch build 2026-06-08-doc504`) |
| **ERR-001** | **IN_PROGRESS** | SAFE+ staging must pass at **1520640** before mission-tier deploy |

**Next engineering action:** Finish SAFE+ slot-fit (`apply-mission-patch-staging.bat` + `run-verify-staging.bat`), deploy mission tier, verify Mission 504 chain in-game per §3.

---

## 7. Modification playbook — how to change the patch without breaking load

Use this section **before** every new fix attempt. The goal is to change mission logic only, not re-discover bootstrap/reflection failures.

### 7.1 Progress to date (2026-06-09)

| Milestone | Result |
|-----------|--------|
| Bootstrap stall root cause | Oversized DLL broke bundle `u=` layout; Cecil `Write` adds +4096 |
| Durable DLL write | `DnlibPreserveWriter` — Cecil inject in memory → dnlib `PreserveRids` at client slot |
| Durable bundle inject | `BundleInjectPreserve.java` — slot-pad entries, **UnityWeb** LZMA (not `saveRaw`) |
| Layout gate | `verify-bundle-layout.bat` → `lastU=49172188` |
| Bootstrap tier deployed | DLL **1520640**, bundle **7126773** (+1668 B), hash `b787ec06…`, in-game load **OK** |
| Mission tier (ERR-001) | Doc504 helpers + surgical `Process*` hooks; **blocked** until DLL stays 1520640 |

### 7.2 Golden assets (never substitute)

| Asset | Path | Constraint |
|-------|------|------------|
| Client bundle | `_inspect_bundle/client/main.unity3d` | 7125105, `lastU=49172188` |
| Client DLL slot | `_inspect_bundle/client/Assembly - CSharp.dll` | **Exactly 1520640** bytes |
| Staging output | `tools/FFPatch/staging/Assembly-CSharp.patched.dll` | All work lands here first |

### 7.3 Staging tiers and verify gates

| Tier | Build command | Required verifies |
|------|---------------|-------------------|
| Bootstrap | `apply-mission-patch-staging.bat` (FCT + RTC bypass only) | `verify-il`, `verify-load-safe`, `verify-bootstrap` |
| SAFE+ (mission) | Same script when donor includes chain helpers | Above + **`verify-safe-plus`** |

**Rule:** `run-verify-staging.bat` must pass **before** any client deploy. DLL size **must** equal 1520640 (`verify-bootstrap`).

### 7.4 Step-by-step — adding or changing mission logic

1. **Read spec first** — [`docs/MISSION-PACKET-COMPLETION.md`](docs/MISSION-PACKET-COMPLETION.md) + per-mission `MISSION-{id}.md`.
2. **Edit paste units only** — `mods/patches/cnMissionManager-forcecomplete/add-class-members-compile-paste.txt` and/or `step6-ForceComplete-block.cs`. These are **not** deploy artifacts.
3. **Slot budget (hard)** — The client DLL slot is **exactly 1520640** bytes. Bootstrap tier fits with **zero** new `MethodDef`s (FCT body replace + surgical `RequestTaskComplete` only). Each added helper method costs ~3 KB metadata — **3 helpers = +9728 bytes** (ERR-001 attempts #13–#14). Mission tier must use **0 new MethodDefs**: inline IL in `Process*` surgical hooks, or recycle bodies of existing unused `cnMissionManager` methods (do not add rows).
4. **Surgical hooks only** — Patch `ProcessEndFail` / `ProcessEndSucc` / `ProcessStartFail` / `ProcessStartSucc` via `PatchProcess*Chain.cs` (inject call + early `ret`). **Never** full-method transplant of `Process*` bodies.
5. **Never transplant** — `Update`, `CallGuideMessage`, `NanoTimeEffect`, full-class dnSpy paste.
6. **Chain flag** — Reuse existing field `m_iloopTemp`; do **not** add new fields (metadata growth).
7. **Build staging** — `tools/FFPatch/apply-mission-patch-staging.bat` (runs DonorCompile → Cecil inject → **dnlib preserve write**).
8. **Verify staging** — `tools/FFPatch/run-verify-staging.bat`.
9. **If dnlib fails** — Check `DnlibPreserveWriter` transplant: resolve methods/fields **by name on `cnMissionManager`**, not Cecil tokens. Sync **all** patched methods listed in `AlwaysSyncMethodNames`.
10. **If size ≠ 1520640** — Remove/consolidate methods; do **not** use Cecil `Write`, `dll-slot-fit`, or PE truncation.
11. **Deploy** — `apply-mission-patch-client-deploy.bat` → `bundle-inject-preserve.bat` → `verify-bundle-layout.bat` (`lastU=49172188`) → `launch-patched-client.bat`.
12. **In-game gates** — `Resources base url`, `CreateGameMode:2`, `ForceCompleteV2: patch build 2026-06-08-doc504`, then mission-specific logs (§3).

### 7.5 dnlib preserve rules (reflection / launch safety)

| Do | Don't |
|----|-------|
| Load **client base DLL** as dnlib module | Cecil `AssemblyDefinition.Write` on client DLL |
| `MetadataFlags.PreserveRids \| KeepOldMaxStack` | Full `Process*` body replace via Cecil write |
| Add methods then transplant IL from Cecil work copy | Import Cecil `MethodReference` tokens blindly |
| Resolve `IMethod` / `IField` by **name + param count** on `cnMissionManager` | Add `UnityEngine` types not already in module |
| Two-pass: add all methods, then transplant bodies | Transplant before callee methods exist (null operand) |

### 7.6 Bundle inject rules

| Do | Don't |
|----|-------|
| `bundle-inject-preserve.bat` (javac + UnityWeb LZMA) | `bundle-inject-raw` / UnityRaw |
| Confirm `lastU=49172188` after inject | Deploy when bundle shrinks &gt;2% vs client |
| Record hash + size on error card | `restore-client.bat` unless user requests rollback |

### 7.7 Build stamp and log markers

Update the build stamp string in `step6-ForceComplete-block.cs` when logic changes materially. In-game proof requires:

- `ForceCompleteV2: patch build 2026-06-08-doc504` — patch loaded
- `ForceCompleteV2: doc-complete task N` — packet emit path
- `ForceCompleteV2: retry task 463 err 1` — EndFail hook (no `Fail Outgoing Task : 466`)
- `ProcessEndSucc : 463` — server accepted (ERR-001 verified)

### 7.8 Documentation updates (mandatory per §1)

After each failed or successful fix attempt, update:

1. [`PATCH-ERROR-LOG.md`](patches/cnMissionManager-forcecomplete/PATCH-ERROR-LOG.md) — symptom + root cause
2. [`ACTIVE-ERROR-QUEUE.md`](ACTIVE-ERROR-QUEUE.md) — fix attempt row
3. This file §7.1 timeline if a new durable constraint is proven
