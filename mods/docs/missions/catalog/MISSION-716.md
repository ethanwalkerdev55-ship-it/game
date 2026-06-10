# Mission 716 — SACT Attack (Part 4 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 716 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2508 | GotoLocation | — | — | 2509 | — | 1908 | 2749 |
| 2509 | Defeat | — | grant:120, gate:120 | 2511 | 2510 | — | — |
| 2510 | GotoLocation | — | — | 2509 | — | — | 2749 |
| 2511 | Talk | — | — | — | — | — | 1908 |

## Chain edges (from table)

- Success: **2508** → **2509**
- Success: **2509** → **2511**
- Fail (err 1/12): **2509** → **2510**
- Success: **2510** → **2509**

## Task 2508

**Tags:** chains-to=2509

### How this task is received

- Player accepts from NPC table ID **1908** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **715**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **1908** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2749** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2508
- `iNPC_ID` = runtime ID from grant NPC 1908
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2508
- `iNPC_ID` = runtime ID from terminator NPC 2749
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the old mining camp.

## Task 2509

**Tags:** grant-timer=120s, complete-timer-gate=120s, kill-quota, chains-to=2511, fail-restart=2510

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **715**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Grant timer **120**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **120**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Kill quotas: enemy 184 x10

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2509
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2509
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the old mining camp.

## Task 2510

**Tags:** chains-to=2509

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **715**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2749** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2510
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2510
- `iNPC_ID` = runtime ID from terminator NPC 2749
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the old mining camp.

## Task 2511

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **715**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1908** (proximity/talk).
- Reward table ID **634** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2511
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2511
- `iNPC_ID` = runtime ID from terminator NPC 1908
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the old mining camp.
