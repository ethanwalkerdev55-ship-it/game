# Mission 455 — Plumber's Helper (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 455 |
| Mission type | Normal |
| Task count | 8 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1405 | UseItems | — | — | 1406 | — | 725 | 1814 |
| 1406 | UseItems | — | — | 1407 | — | — | 1815 |
| 1407 | UseItems | — | — | 1409 | — | — | 1816 |
| 1408 | UseItems | — | — | 1410 | — | — | 1814 |
| 1409 | UseItems | — | — | 1410 | — | — | 1814 |
| 1410 | UseItems | — | grant:180, gate:180 | 1411 | 1408 | — | 1815 |
| 1411 | UseItems | — | grant:70, gate:70 | 1412 | 1408 | — | 1816 |
| 1412 | Talk | — | — | — | — | — | 725 |

## Chain edges (from table)

- Success: **1405** → **1406**
- Success: **1406** → **1407**
- Success: **1407** → **1409**
- Success: **1408** → **1410**
- Success: **1409** → **1410**
- Success: **1410** → **1411**
- Fail (err 1/12): **1410** → **1408**
- Success: **1411** → **1412**
- Fail (err 1/12): **1411** → **1408**

## Task 1405

**Tags:** chains-to=1406

### How this task is received

- Player accepts from NPC table ID **725** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **725** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1814** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1405
- `iNPC_ID` = runtime ID from grant NPC 725
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1405
- `iNPC_ID` = runtime ID from terminator NPC 1814
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1406

**Tags:** chains-to=1407

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1815** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1406
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1406
- `iNPC_ID` = runtime ID from terminator NPC 1815
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1407

**Tags:** chains-to=1409

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1816** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1407
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1407
- `iNPC_ID` = runtime ID from terminator NPC 1816
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1408

**Tags:** chains-to=1410

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1814** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1408
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1408
- `iNPC_ID` = runtime ID from terminator NPC 1814
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1409

**Tags:** chains-to=1410

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1814** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1409
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1409
- `iNPC_ID` = runtime ID from terminator NPC 1814
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1410

**Tags:** grant-timer=180s, complete-timer-gate=180s, chains-to=1411, fail-restart=1408

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1815** (proximity/talk).
- Grant timer **180**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **180**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1410
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1410
- `iNPC_ID` = runtime ID from terminator NPC 1815
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1411

**Tags:** grant-timer=70s, complete-timer-gate=70s, chains-to=1412, fail-restart=1408

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1816** (proximity/talk).
- Grant timer **70**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **70**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1411
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1411
- `iNPC_ID` = runtime ID from terminator NPC 1816
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.

## Task 1412

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **454**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **725** (proximity/talk).
- Reward table ID **318** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1412
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1412
- `iNPC_ID` = runtime ID from terminator NPC 725
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three Seismic Emitters in the Fissure.
