# Mission packet completion — doc-driven patch spec

**Purpose:** Autocomplete must emit **`sP_CL2FE_REQ_PC_TASK_END`** (and when needed **`TASK_START`**) with every field the server expects for that task row — not generic “force complete” shortcuts.

**Authority:** Per-task fields live in [`missions/catalog/MISSION-{id}.md`](missions/catalog/MISSION-{id}.md). Generic flow: [`MISSION-EXECUTION-LOGIC.md`](MISSION-EXECUTION-LOGIC.md) §3–4. Instance rules: [`missions/by-type/INSTANCE-ZONE-MISSIONS.md`](missions/by-type/INSTANCE-ZONE-MISSIONS.md).

**Strict rules:** [`STRICT-RULES.md`](../STRICT-RULES.md) §3.

---

## 1. Why vanilla “loops” (not a load bug)

Observed on **vanilla** bundle (no `ForceCompleteV2:`), e.g. 2026-06-08 23:39–23:40:

```
ProcessStartFail : 468  →  Send Start Mission : 463  →  ProcessStartSucc : 463
→  ProcessEndFail : 463 Error Code : 1  →  Fail Outgoing Task : 466  →  (repeat)
```

This matches [`MISSION-504.md`](missions/catalog/MISSION-504.md) task **463**:

- Instance **12** required; player is **overworld** (`bInsMap` false).
- Client auto-starts 463; server accepts **start** but rejects **end** with `bError=false` (implicit) → error **1** → fail-outgoing **466**.

**Game load is normal.** Mission state machine is doing what the table says. The patch must change **packets and handler behavior**, not remove the mission.

---

## 2. Completion is not one function — it is a per-task matrix

Before any code change, fill this table from the mission catalog (example: mission **504** chain **466 → 468 → 463 → 667**):

| Task | Type | Terminator NPC (table) | Instance | Quotas | Success `TASK_END` | Outside-zone / fail strategy |
|------|------|------------------------|----------|--------|-------------------|------------------------------|
| **466** | GotoLocation | **1471** | — | — | `iTaskNum=466`, `iNPC_ID=runtime(1471)`, `iEscortNPC_ID≥0`, `bError=false` | N/A |
| **468** | GotoLocation | **1470** | — | — | `iTaskNum=468`, `iNPC_ID=runtime(1470)`, `bError=false` | N/A |
| **463** | Defeat | **0** | **12** | enemy **2513 ×4** | In instance: quotas cleared, `bError=false` | Outside: `bError=true`, `iEscortNPC_ID=-1`; must obtain **`ProcessEndSucc`** or doc-approved abort that advances chain — not vanilla `Fail Outgoing 466` during patch |
| **667** | Defeat | **0** | **12** | enemy **7 ×1** | Same pattern as 463 | Same |

**Rule:** `ForceCompleteEmitTaskEndPacket` must be driven by this row, not a single global `sendError` heuristic.

---

## 3. Required client preconditions (before `TASK_END`)

From [`MISSION-EXECUTION-LOGIC.md`](MISSION-EXECUTION-LOGIC.md) §4.2 — server still validates; client should prepare:

| Check | Defeat (463) | GotoLocation (466/468) | Timer tasks |
|-------|----------------|-------------------------|-------------|
| Task active (`m_ActivateMissionList`) | Yes | Yes | Yes |
| Kill quotas `m_aiCurrentRemainEnemyNum[*]=0` | **2513 ×4** | — | — |
| Item quotas | — | — | — |
| Grant timer `m_fRemainTime` | — | — | **0** before send |
| Completion timer gate `m_iCSUCheckTimer` | — | — | elapsed |
| Terminator NPC runtime ID resolved | 0 → skip NPC lookup | **1471 / 1470** via `SearchTableID` | per doc |
| Instance | Server validates; client may set `ownstatus.iInsMapNum` hack **only** as staging aid | — | — |

`PrepareTaskForForceComplete` must implement the **row-specific** prep, not only zero all quota slots.

---

## 4. Patch behavior (replaces chain504 heuristics)

### 4.1 Single entry: `EmitDocCompletePacket(int taskId)`

1. Load `MissionElement` for `taskId`.
2. Load catalog doc `MISSION-{missionGroup}.md` task section (504 for this test case).
3. Run row-specific prep (§3).
4. Build `sP_CL2FE_REQ_PC_TASK_END`:
   - `iTaskNum` = taskId
   - `iNPC_ID` = runtime from terminator table ID (or 0 if doc says 0)
   - `iEscortNPC_ID` = escort runtime, or **-1** when `bError=true`
   - `iBox1Choice` / `iBox2Choice` = 0 unless reward UI path
5. `cnEvent.SendPacket(..., 318767116, pkt)`
6. Log: `ForceCompleteV2: doc-complete task {id} npc {tableId}→{runtimeId} err {bool} quotas {snapshot}`

### 4.2 Chain driver (hotkey / auto-chain)

| Step | Action |
|------|--------|
| 1 | User hotkey → `ForceCompleteCurrentTask` → resolve chain head (504: start at active task or **466**) |
| 2 | For each task in doc order: `EmitDocCompletePacket` **or** defer `RequestTaskStart` if `NeedsForceCompleteTaskStart` |
| 3 | On **`ProcessEndSucc`**: advance to `m_iSUOutgoingTask` from catalog (466→468→463→667) |
| 4 | On **`ProcessEndFail` err 1/12**: if patch active, **do not** call vanilla `RequestTaskStart(m_iFOutgoingTask)`; retry with doc **outside-zone** row (usually `bError=true`) |
| 5 | Close chain only on **`ProcessEndSucc`** for final task or documented chain end |

### 4.3 What we stop doing

| Old (chain504) | New (doc-driven) |
|----------------|------------------|
| `sendError = instance \|\| retry>1` | `sendError` from catalog + attempt phase table |
| `npcId = 0` always in `RequestForceCompleteTaskEnd` | Terminator NPC from doc (1470, 1471, 0) |
| Success = packet sent | Success = **`ProcessEndSucc`** in log + next task started |
| Block only on hotkey path | Also hook vanilla auto-fail when `bForceCompleteChain` (optional tier) |

---

## 5. Verification (ERR-001 close criteria)

From [`CONTINUOUS-ERROR-TRACKING.md`](../CONTINUOUS-ERROR-TRACKING.md) §6 — mission 504:

- [ ] Log: `ForceCompleteV2: patch build …-doc504` (new build stamp)
- [ ] Hotkey on task **468** or **463**
- [ ] **`ProcessEndSucc : 463`** (or documented abort that chains to **667** without `Fail Outgoing Task : 466`)
- [ ] No `Fail Outgoing Task : 466` while patch chain active
- [ ] Optional: host packet dump shows `TASK_END` fields match §2 table

**Vanilla auto-chain loops do not close ERR-001** — only patched, doc-verified completion.

---

## 6. Implementation order (ERR-001 while ERR-002 blocks deploy)

1. ~~**Staging only** — rewrite `add-class-members-compile-paste.txt` → `EmitDocCompletePacket` + 504 matrix in comments~~ **DONE 2026-06-09**
2. ~~`apply-mission-patch-staging.bat` + `run-verify-staging.bat`~~ **PASS 2026-06-09**
3. When ERR-002 resolved — deploy once, verify §5 in-game
4. Generalize matrix generator from `export-mission-catalog.py` (future)
