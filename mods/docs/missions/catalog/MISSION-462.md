# Mission 462 — Vertical Limits (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 462 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1427 | GotoLocation | — | — | 1429 | — | 791 | 1799 |
| 1428 | GotoLocation | — | — | 1429 | — | — | 1799 |
| 1429 | UseItems | — | grant:200, gate:200 | — | — | — | 1800 |

## Chain edges (from table)

- Success: **1427** → **1429**
- Success: **1428** → **1429**

## Task 1427

**Tags:** chains-to=1429

### How this task is received

- Player accepts from NPC table ID **791** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **461**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **791** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1799** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1427
- `iNPC_ID` = runtime ID from grant NPC 791
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1427
- `iNPC_ID` = runtime ID from terminator NPC 1799
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get to the top of the tower in time.

## Task 1428

**Tags:** chains-to=1429

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **461**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1799** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1428
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1428
- `iNPC_ID` = runtime ID from terminator NPC 1799
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get to the top of the tower in time.

## Task 1429

**Tags:** grant-timer=200s, complete-timer-gate=200s

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **461**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1800** (proximity/talk).
- Grant timer **200**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **200**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **324** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1429
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1429
- `iNPC_ID` = runtime ID from terminator NPC 1800
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get to the top of the tower in time.
