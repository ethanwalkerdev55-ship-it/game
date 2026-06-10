# Mission 576 — The Thromnambular Saga (Part 7 of 9)

| Field | Value |
|-------|-------|
| Mission ID | 576 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2295 | Talk | — | — | 2296 | — | 2807 | 746 |
| 2296 | Defeat | — | — | 2298 | — | — | — |
| 2297 | GotoLocation | — | — | 2296 | — | — | 1824 |
| 2298 | Talk | — | grant:250, gate:250 | — | 2297 | — | 2807 |

## Chain edges (from table)

- Success: **2295** → **2296**
- Success: **2296** → **2298**
- Success: **2297** → **2296**
- Fail (err 1/12): **2298** → **2297**

## Task 2295

**Tags:** chains-to=2296

### How this task is received

- Player accepts from NPC table ID **2807** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **468**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **2807** (journal accept or auto-chain).
- Complete at terminator NPC table ID **746** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2295
- `iNPC_ID` = runtime ID from grant NPC 2807
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2295
- `iNPC_ID` = runtime ID from terminator NPC 746
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 2296

**Tags:** kill-quota, item-quota, chains-to=2298

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **468**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 218 x0
- Collect items: item 543 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2296
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2296
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 2297

**Tags:** chains-to=2296

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **468**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1824** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2297
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2297
- `iNPC_ID` = runtime ID from terminator NPC 1824
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 2298

**Tags:** grant-timer=250s, complete-timer-gate=250s, fail-restart=2297

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **468**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2807** (proximity/talk).
- Grant timer **250**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **250**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **546** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2298
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2298
- `iNPC_ID` = runtime ID from terminator NPC 2807
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.
