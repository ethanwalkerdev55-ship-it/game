# Mission 439 — The Thromnambular Saga (Part 2 of 9)

| Field | Value |
|-------|-------|
| Mission ID | 439 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1121 | GotoLocation | — | — | 1123 | — | 1487 | 1468 |
| 1122 | GotoLocation | — | — | 1123 | — | — | 1468 |
| 1123 | GotoLocation | — | grant:120, gate:120 | 1124 | 1122 | — | 1377 |
| 1124 | Talk | — | — | — | — | — | 1487 |

## Chain edges (from table)

- Success: **1121** → **1123**
- Success: **1122** → **1123**
- Success: **1123** → **1124**
- Fail (err 1/12): **1123** → **1122**

## Task 1121

**Tags:** chains-to=1123

### How this task is received

- Player accepts from NPC table ID **1487** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **438**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **1487** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1468** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1121
- `iNPC_ID` = runtime ID from grant NPC 1487
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1121
- `iNPC_ID` = runtime ID from terminator NPC 1468
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 1122

**Tags:** chains-to=1123

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **438**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1468** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1122
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1122
- `iNPC_ID` = runtime ID from terminator NPC 1468
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 1123

**Tags:** grant-timer=120s, complete-timer-gate=120s, chains-to=1124, fail-restart=1122

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **438**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1377** (proximity/talk).
- Grant timer **120**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **120**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1123
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1123
- `iNPC_ID` = runtime ID from terminator NPC 1377
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 1124

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **438**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1487** (proximity/talk).
- Reward table ID **262** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1124
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1124
- `iNPC_ID` = runtime ID from terminator NPC 1487
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.
