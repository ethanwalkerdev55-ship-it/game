# FusionFall — Mission execution logic (complete reference)

**Source:** Decompiled `Assembly - CSharp.dll` from `main.unity3d` (`mods/decompiled/Assembly-CSharp/`).  
**Mission table schema:** `Assembly - CSharp - first pass.dll` → `MissionElement` (see `MISSION-ELEMENT-SCHEMA.md`).  
**Packets:** `MISSION-PACKET-PROTOCOL.md`.  
**Worked example (mission 504):** `missions/MISSION-504.md`.

---

## 1. Architecture overview

Missions are **data-driven**. Each row in **Mission Table** (index **7** in `TableContainer`) is a `MissionElement` describing one **task** (not one mission — a mission group contains multiple tasks).

```
xdtdatas*.asset  →  XdtTableScript.m_pMissionTable  →  MissionElement[]
                              ↓
                    cnMissionManager.MakeMissionGroup()
                              ↓
              m_MissionGroupList (tree: mission ID → ordered tasks)
                              ↓
         Runtime: m_ActivateMissionList / m_CompletedMissionList
                              ↓
              Client ↔ Server via TASK_START / TASK_END packets
```

| Component | File | Role |
|-----------|------|------|
| `cnMissionManager` | `cnMissionManager.cs` | Central state machine: start, complete, chain, timer, instance |
| `cnMissionNode` | `cnMissionNode.cs` | Per-task runtime node (timers, kill/item counters) |
| `cnMissionJournal` | `cnMissionJournal.cs` | UI: accept, abandon, reward complete |
| `NpcIconMode` | `NpcIconMode.cs` | NPC interaction: grant / terminate tasks |
| `cnMissionTaskCheckerNode` | `cnMissionTaskCheckerNode.cs` | 10s anti-spam lock per task during in-flight packets |
| `cnNPCAndMissionRelationNode` | `cnNPCAndMissionRelationNode.cs` | NPC → grant/terminate task lists |
| `GameFrame` | `GameFrame.cs` | TCP packet receive → `cnEvent(12, 21..24)` → manager |
| `TableContainer` | `TableContainer.cs` | Loads `xdtdatas*.asset`; mission table = object index **7** |
| `InstanceTableScript` | first pass DLL | Instance zone / warp metadata (table index **5**) |

There are **no** hardcoded strings `"Fusion Lair"` or `"Infected Zone"` in mission logic. Instance missions are identified solely by `MissionElement.m_iRequireInstanceID > 0`. Timer missions use `m_iSTGrantTimer` and/or `m_iCSUCheckTimer`.

---

## 2. How the player receives a mission

### 2.1 Sources

| Source | Flow |
|--------|------|
| **Mission Journal (NPC offer)** | `cnMissionJournal` mode `eJM_Allow` → ACCEPT → `cnEvent(12, 5)` → `ReceiveRequestTaskStart` |
| **NPC icon mode** | `NpcIconMode.CheckQuest` → grant list → `CheckToStartTaskCondition == 0` → journal / start UI |
| **Email system** | `cnEmailSystemManager` offers missions from table scan on login |
| **Auto-chain** | `ProcessEndSucc` → `m_iSUOutgoingTask` → `RequestTaskStart(next, 0)` |
| **Fail rollback** | `ProcessEndFail` (error 1/12) → `m_iFOutgoingTask` → `RequestTaskStart(failTask, 0)` |
| **Login restore** | `SetMissionAndTaskFlags` from `ENTER_SUCC` → re-activate running tasks |

### 2.2 Accept button (journal)

```csharp
// cnMissionJournal — ACCEPT MISSION
cnEvent(12, 5)[0] = currentMission.m_iHTaskID;
cnEvent(12, 5)[1] = currentMission.m_iHNPCID;
→ ReceiveRequestTaskStart → RequestTaskStart(taskId, npcTableId)
```

`CheckMissionSlot` enforces slot limits: Guide (type 1), Nano (type 2), Normal (type 3).

### 2.3 Start preconditions (`CheckToStartTaskCondition`)

Returns `eMISSION_CHECK_RESULT_PROPERTY`:

| Code | Meaning |
|------|---------|
| `0` | **CanStart** |
| `1` | CannotStart (prereq, level, nano, guide, items, trigger task, repeat) |
| `2` | AlreadyStart (same mission group already active) |
| `5` | AlreadyComplete |

Checks (in order):

1. No pending start in same mission group (`arrRequestStartTask`)
2. Repeat / completed flags (`m_iRepeatflag`, `m_iRepeatQuestFlag`)
3. Mission not already active
4. Prerequisite missions `m_iCSTReqMission[0..1]`
5. Level: `m_iKorStReqLvlMin` or `m_iCTRReqLvMin` / `m_iCTRReqLvMax`
6. Required nanos `m_iCSTRReqNano[0..4]` in nano bank
7. Guide match `m_iCSTReqGuide` vs `ownstatus.iGuide`
8. Start items `m_iCSTItemID` / `m_iCSTItemNumNeeded`
9. Trigger task active `m_iCSTTrigger`

**Instance zones are NOT checked client-side at start.** Server validates via `TASK_START_FAIL`.

---

## 3. How a task is started (client → server)

### 3.1 `RequestTaskStart(int iTaskID, int iNPCID)`

Builds `sP_CL2FE_REQ_PC_TASK_START` (12 bytes, opcode `318767115`):

| Field | Source |
|-------|--------|
| `iTaskNum` | `MissionElement.m_iHTaskID` |
| `iNPC_ID` | Runtime `Status.iID` from `m_iHNPCID` table lookup (if > 0). **Note:** method parameter `iNPCID` is unused. |
| `iEscortNPC_ID` | For task type 6 (EscortDefence): nearby `m_iCSUDEFNPCID` runtime ID |

Before send:

- `task.InitializeBeforeStart()` — resets kill/item counters
- `AddMissionTaskChecker(taskId)` — 10s debounce
- `arrRequestStartTask.Add(taskId)`

Log: `Send Start Mission : {taskId}`

### 3.2 Server response → `ProcessStartSucc`

Opcode `822083612` → `GameFrame` → `cnEvent(12, 21)` → `ProcessStartSucc`.

| Action | Detail |
|--------|--------|
| Clear checker / pending start | `DelMissionTaskChecker`, remove from `arrRequestStartTask` |
| **Timer** | If `m_iSTGrantTimer > 0`: `SetRemainTime(iRemainTime)` from server packet |
| Activate | `InsertActivateTask`, `SetMissionState(1)` |
| UI | Bubble chat, mission messages, waypoint `m_iSTGrantWayPoint` |
| Escort | `NPC_GROUP_INVITE` for escort NPC |
| Tutorial hooks | Task IDs 463, 488, 548, 572 (first-use conditions only) |

**Does not** auto-complete or auto-chain on start (chain happens on **end success**).

### 3.3 `ProcessStartFail`

Logs `ProcessStartFail tasknumber : {id}`, clears checker. Common when starting instance task outside zone (server error).

---

## 4. How a task is completed (client → server)

All normal completions use **`TASK_END`** (`sP_CL2FE_REQ_PC_TASK_END`, 16 bytes, opcode `318767116`).  
**Not** `TASK_COMPLETE` (opcode `318767223`) — that opcode is GM/debug only (`CnGuiChat`).

### 4.1 `RequestTaskComplete(int iTaskID, int iNPCID, bool bError)`

| Field | Normal (`bError=false`) | Error (`bError=true`) |
|-------|-------------------------|------------------------|
| `iTaskNum` | Task ID | Task ID |
| `iNPC_ID` | Runtime ID from terminator NPC table ID (if `iNPCID != 0` and NPC found) | Same if found |
| `iBox1Choice` / `iBox2Choice` | 0 (journal reward UI sets bit flags) | 0 |
| `iEscortNPC_ID` | Escort runtime ID (type 6) | **Forced `-1`** |

Guards:

- Returns if task checker active (10s debounce)
- Returns if `iNPCID != 0`, NPC not found, and `bError == false`

After send: `AddMissionTaskChecker(iTaskID)`.

### 4.2 Completion preconditions (`CheckToCompleteTaskCondition`)

| Code | Meaning |
|------|---------|
| `3` | **CanComplete** |
| `4` | CannotComplete (not active, enemies remain, items remain) |
| `6` | **Fail** — `m_iCSUCheckTimer > 0` and timer expired |

Checks:

1. Task in `m_ActivateMissionList`
2. **Completion timer gate:** `m_iCSUCheckTimer > 0` && `m_fRemainTime <= 0` → fail (6)
3. Kill quotas: `m_aiCurrentRemainEnemyNum[i]` for each `m_iCSUEnemyID[i]`
4. Item quotas: `m_aiCurrentRemainItemNum[j]` for each `m_iCSUItemID[j]`

**Instance location is NOT checked client-side** before send.

### 4.3 Automatic completion triggers (`Update`, 1 Hz)

For each active task:

| Condition | Action |
|-----------|--------|
| `m_iSTGrantTimer > 0` && timer expired | `RequestTaskComplete(taskId, 0, false)` — log: `Send Time mission.` |
| Type 6 escort | Invite escort NPC to group if not grouped |
| Type 2 or 6 + terminator NPC in range | If `CheckToCompleteTaskCondition == 3`: complete at NPC or open reward UI |
| Kill quota met (no terminator) | Complete or reward UI |
| Item quota met | Complete |

### 4.4 Manual completion paths

| Path | `bError` |
|------|----------|
| NPC terminate (`ReceiveRequestTaskEnd`) | `false` |
| Journal reward (`CheckComplete`) | `false` (sets box choice bits) |
| Kill-all-enemies auto | `false` |
| Timer expiry | `false` |
| Escort NPC death (`CheckEscortQuest`) | **`true`** |
| Warp out of instance (`CheckWarpAllMision`) | **`true`** |
| **Right Ctrl cheat** (`ForceCompleteCurrentTask`) | **`false`** (vanilla) |

### 4.5 Server response → end handlers

**`ProcessEndSucc`** (opcode `822083614`):

- State → completed; remove from active list
- If final task or no `m_iSUOutgoingTask`: insert completed, mission-complete sound
- Else: task-complete sound, bubble success
- Escort: `NPC_GROUP_KICK`
- **Chain:** if `m_iSUOutgoingTask > 0` → `RequestTaskStart(next, 0)`

**`ProcessEndFail`** (opcode `822083615`):

| `iErrorCode` | Client behavior |
|--------------|-----------------|
| **1**, **12** | Fail path: escort kick, state 0, eliminate active, fail bubble/message, **`RequestTaskStart(m_iFOutgoingTask)`** — log: `Fail Outgoing Task : {id}` |
| **13** | System message box only |
| Other | **Return early** (no handling) |

Error **1** is the common server rejection (“return to NPC”, wrong zone, timer not satisfied).

---

## 5. Timer missions (detailed)

### 5.1 Two timer fields

| Field | Role |
|-------|------|
| `m_iSTGrantTimer` | **Grant timer** — server sends `iRemainTime` on start success; client counts down |
| `m_iCSUCheckTimer` | **Completion gate** — task cannot complete until timer elapsed; expired → `CheckToCompleteTaskCondition` returns **6 (Fail)** |

### 5.2 Runtime timer state (`cnMissionNode`)

```
SetRemainTime(serverSeconds):
  m_fLifeTime = m_fRemainTime = serverSeconds
  m_lStartedSystemTime = DateTime.Now.Ticks

UpdateRemainTime() [called 1/sec from cnMissionManager.Update]:
  m_fRemainTime = m_fLifeTime - (Now.Ticks - m_lStartedSystemTime) * 1e-7
```

### 5.3 Timer lifecycle

```
ProcessStartSucc → SetRemainTime(iRemainTime)     [if m_iSTGrantTimer > 0]
        ↓
Update (each second) → UpdateRemainTime()
        ↓
If m_fRemainTime <= 0:
  → RequestTaskComplete(taskId, 0, bError=false)   [auto submit — NOT an error packet]
        ↓
Server may accept OR reject with ProcessEndFail err 1
        ↓
If m_iCSUCheckTimer > 0 and player tries complete early:
  → CheckToCompleteTaskCondition returns 6 → blocked locally
```

### 5.4 Why autocomplete breaks on timer missions (vanilla)

`ForceCompleteCurrentTask` sends `TASK_END` with `bError=false` **without**:

- Zeroing / deferring the grant timer
- Waiting for `m_iCSUCheckTimer` gate

Server responds `ProcessEndFail : {task} Error Code : 1` → client may restart `m_iFOutgoingTask` → “return to NPC” loop.

**Patch project fix:** defer complete in `Update` until timer zero; retry on error 1 (`ForceCompleteV2` in patch sources).

See: `missions/by-type/TIMER-MISSIONS.md`

---

## 6. Instance zone / Fusion Lair / Infected Zone (detailed)

### 6.1 Identification

| Signal | Meaning |
|--------|---------|
| `MissionElement.m_iRequireInstanceID > 0` | Task requires instance map (Fusion Lair, Infected Zone, EP, etc.) |
| `cnOwnAvatarStatus.bInsMap` | Player currently inside an instance |
| `cnOwnAvatarStatus.iInsMapNum` | Current instance map number (from server) |

**Client never compares** `iInsMapNum` to `m_iRequireInstanceID` during normal start/complete. Validation is **server-side**.

### 6.2 Entering instance

Server packet `sP_FE2CL_INSTANCE_MAP_INFO` (opcode `822083738`, 60 bytes):

```
bInsMap = true
iInsMapNum = struct.iInstanceMapNum
iEp_ID = struct.iEP_ID
GameFrameEpContainer.InitRings(rect, epSwitchArray)
```

### 6.3 Leaving instance (warp)

On `sP_FE2CL_REP_PC_WARP_USE_NPC_SUCC` while `bInsMap`:

```
cnEvent(12, 29) → CheckWarpAllMision()
  for each active task with m_iRequireInstanceID > 0:
    RequestTaskComplete(taskId, 0, bError=true)   // iEscortNPC_ID = -1
bInsMap = false; iInsMapNum = 0
```

This is the **only vanilla client path** that completes instance tasks with `bError=true`.

### 6.4 Why autocomplete breaks on instance missions (vanilla)

`ForceCompleteCurrentTask` sends `TASK_END` with **`bError=false`** while player is outside instance.

Server: `ProcessEndFail : 463 Error Code : 1`  
Client: `Fail Outgoing Task : 466` → `RequestTaskStart(466)` → loop until player enters zone.

**Patch project fix:** send `bError=true` (or warp/instance map hack) for instance tasks; block fail-outgoing during force-complete chain.

See: `missions/by-type/INSTANCE-ZONE-MISSIONS.md`, `missions/MISSION-504.md`

### 6.5 Instance table (warp / zone metadata)

`InstanceTableScript` (table index 5):

- `InstanceElement`: zone coords, name ID, EP flag
- `WarpElement`: warp NPC ↔ instance mapping

Used by world map / warp NPCs — not directly by `cnMissionManager` except via server instance state.

---

## 7. Task types (`eTaskTypeProperty`)

| Value | Name | Typical completion |
|-------|------|-------------------|
| 1 | Talk | NPC terminate |
| 2 | GotoLocation | Proximity to `m_iHTerminatorNPCID` |
| 3 | UseItems | Item counts |
| 4 | Delivery | Item + NPC |
| 5 | Defeat | Kill `m_iCSUEnemyID` quotas |
| 6 | EscortDefence | Escort NPC to terminator; death → `bError` complete |

See: `missions/by-type/TASK-TYPE-REFERENCE.md`

---

## 8. Mission types (`eMissionTypeProperty`)

| Value | Name | Slot limit |
|-------|------|------------|
| 1 | Guide | 1 active |
| 2 | Nano | 1 active (slot 0 only on login) |
| 3 | Normal | Multiple |

---

## 9. Task chaining fields

| Field | On success (`ProcessEndSucc`) | On fail err 1/12 (`ProcessEndFail`) |
|-------|------------------------------|-------------------------------------|
| `m_iSUOutgoingTask` | Auto `RequestTaskStart(next)` | — |
| `m_iSUOutgoingMission` | Start task in other mission (bug: uses `m_iSUOutgoingTask` lookup) | — |
| `m_iFOutgoingTask` | — | Auto `RequestTaskStart(failTask)` |

---

## 10. Cheat / autocomplete hotkey

```csharp
// cnAvatarAttack.cs — key 305 (Right Ctrl)
Input.GetKeyDown(305) → cnMissionManager.ForceCompleteCurrentTask()
```

Vanilla `ForceCompleteCurrentTask`:

1. Sets local state completed **before** server ack (optimistic)
2. Builds `TASK_END` with terminator NPC if `m_iHTerminatorNPCID` set
3. **Never sets `bError=true`** for instance tasks
4. Advances `SelectMissionTask` to next sibling in tree (local only)
5. Does **not** wait for `ProcessEndSucc` before advancing selection

Log markers (patched client): `ForceCompleteV2: …` (not in vanilla DLL).

---

## 11. Debounce (`cnMissionTaskCheckerNode`)

- 10 second lock per task ID after start or end packet sent
- `RequestTaskStart` / `RequestTaskComplete` no-op if checker exists
- Cleared on `ProcessStartSucc/Fail`, `ProcessEndSucc/Fail`

Prevents packet spam; can block rapid autocomplete retries.

---

## 12. Per-mission documentation

Mission **definitions** are extracted from `TableData.resourceFile` → `xdtdatas` → `m_pMissionTable` (**2866** tasks, **747** mission groups). Regenerate with `python tools/export-mission-catalog.py`.

| Doc | Content |
|-----|---------|
| `missions/MISSION-CATALOG.md` | Master index — every mission with links |
| `missions/catalog/MISSION-{id}.md` | Per-mission execution doc (receive, complete, packets, chain) |
| `missions/catalog/mission-table-full.csv` | Full table export |
| `MISSION-ELEMENT-SCHEMA.md` | Every `MissionElement` field |
| `missions/MISSION-504.md` | Log-backed analysis of test mission 466→468→463 |
| `missions/by-type/TIMER-MISSIONS.md` | Timer rules (73 mission groups) |
| `missions/by-type/INSTANCE-ZONE-MISSIONS.md` | Instance rules (99 mission groups) |

---

## 13. Key source file index

| Topic | File:method |
|-------|-------------|
| Build mission tree | `cnMissionManager.MakeMissionGroup` |
| NPC binding | `cnMissionManager.TaskBindWithNPC` |
| Start packet | `cnMissionManager.RequestTaskStart` |
| End packet | `cnMissionManager.RequestTaskComplete` |
| Start checks | `cnMissionManager.CheckToStartTaskCondition` |
| Complete checks | `cnMissionManager.CheckToCompleteTaskCondition` |
| Timer tick | `cnMissionManager.Update` |
| Instance warp fail | `cnMissionManager.CheckWarpAllMision` |
| Start response | `cnMissionManager.ProcessStartSucc` / `ProcessStartFail` |
| End response | `cnMissionManager.ProcessEndSucc` / `ProcessEndFail` |
| Login restore | `cnMissionManager.SetMissionAndTaskFlags` |
| Autocomplete | `cnMissionManager.ForceCompleteCurrentTask` |
| Journal accept | `cnMissionJournal` ACCEPT button → `cnEvent(12,5)` |
| Journal complete | `cnMissionJournal.CheckComplete` |
| Packet routing | `GameFrame.ReceivePacket` cases `822083612`–`615`, `738`, warp `727` |
| Table load | `TableContainer.LoadAssets` → `GetTable(7)` |

---

*Generated from decompiled client — 2026-06-08. Bundle: `6877b37c-.../main.unity3d`.*
