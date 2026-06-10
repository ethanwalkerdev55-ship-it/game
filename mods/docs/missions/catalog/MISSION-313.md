# Mission 313 — How to Date a Fusion (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 313 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1284 | GotoLocation | — | — | 1285 | — | 720 | 1656 |
| 1285 | UseItems | — | — | 1286 | — | — | 1684 |
| 1286 | UseItems | — | — | 1288 | — | — | 1685 |
| 1287 | UseItems | — | — | 1288 | — | — | 1685 |
| 1288 | UseItems | — | grant:120, gate:120 | — | 1287 | — | 1686 |

## Chain edges (from table)

- Success: **1284** → **1285**
- Success: **1285** → **1286**
- Success: **1286** → **1288**
- Success: **1287** → **1288**
- Fail (err 1/12): **1288** → **1287**

## Task 1284

**Tags:** chains-to=1285

### How this task is received

- Player accepts from NPC table ID **720** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **312**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **720** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1656** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1284
- `iNPC_ID` = runtime ID from grant NPC 720
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1284
- `iNPC_ID` = runtime ID from terminator NPC 1656
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Slow the Terrafusing at the Auditorium.

## Task 1285

**Tags:** chains-to=1286

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **312**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1684** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1285
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1285
- `iNPC_ID` = runtime ID from terminator NPC 1684
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Slow the Terrafusing at the Auditorium.

## Task 1286

**Tags:** chains-to=1288

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **312**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1685** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1286
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1286
- `iNPC_ID` = runtime ID from terminator NPC 1685
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Slow the Terrafusing at the Auditorium.

## Task 1287

**Tags:** chains-to=1288

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **312**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1685** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1287
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1287
- `iNPC_ID` = runtime ID from terminator NPC 1685
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Slow the Terrafusing at the Auditorium.

## Task 1288

**Tags:** grant-timer=120s, complete-timer-gate=120s, fail-restart=1287

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **312**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1686** (proximity/talk).
- Grant timer **120**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **120**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **288** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1288
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1288
- `iNPC_ID` = runtime ID from terminator NPC 1686
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Slow the Terrafusing at the Auditorium.
