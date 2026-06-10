# Mission 305 — Mayor's Assistant (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 305 |
| Mission type | Normal |
| Task count | 8 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1254 | GotoLocation | — | — | 1256 | — | 722 | 1649 |
| 1255 | GotoLocation | — | — | 1256 | — | — | 1649 |
| 1256 | UseItems | — | grant:150, gate:150 | 1257 | 1255 | — | 1678 |
| 1257 | UseItems | — | grant:100, gate:100 | 1258 | 1255 | — | 1679 |
| 1258 | UseItems | — | grant:100, gate:100 | 1259 | 1255 | — | 1680 |
| 1259 | UseItems | — | grant:100, gate:100 | 1260 | 1255 | — | 1681 |
| 1260 | UseItems | — | grant:100, gate:100 | 1261 | 1255 | — | 1682 |
| 1261 | Delivery | — | — | — | — | — | 722 |

## Chain edges (from table)

- Success: **1254** → **1256**
- Success: **1255** → **1256**
- Success: **1256** → **1257**
- Fail (err 1/12): **1256** → **1255**
- Success: **1257** → **1258**
- Fail (err 1/12): **1257** → **1255**
- Success: **1258** → **1259**
- Fail (err 1/12): **1258** → **1255**
- Success: **1259** → **1260**
- Fail (err 1/12): **1259** → **1255**
- Success: **1260** → **1261**
- Fail (err 1/12): **1260** → **1255**

## Task 1254

**Tags:** chains-to=1256

### How this task is received

- Player accepts from NPC table ID **722** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **722** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1649** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1254
- `iNPC_ID` = runtime ID from grant NPC 722
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1254
- `iNPC_ID` = runtime ID from terminator NPC 1649
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1255

**Tags:** chains-to=1256

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1649** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1255
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1255
- `iNPC_ID` = runtime ID from terminator NPC 1649
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1256

**Tags:** grant-timer=150s, complete-timer-gate=150s, chains-to=1257, fail-restart=1255

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1678** (proximity/talk).
- Grant timer **150**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **150**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1256
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1256
- `iNPC_ID` = runtime ID from terminator NPC 1678
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1257

**Tags:** grant-timer=100s, complete-timer-gate=100s, chains-to=1258, fail-restart=1255

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1679** (proximity/talk).
- Grant timer **100**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **100**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1257
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1257
- `iNPC_ID` = runtime ID from terminator NPC 1679
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1258

**Tags:** grant-timer=100s, complete-timer-gate=100s, chains-to=1259, fail-restart=1255

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1680** (proximity/talk).
- Grant timer **100**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **100**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1258
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1258
- `iNPC_ID` = runtime ID from terminator NPC 1680
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1259

**Tags:** grant-timer=100s, complete-timer-gate=100s, chains-to=1260, fail-restart=1255

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1681** (proximity/talk).
- Grant timer **100**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **100**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1259
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1259
- `iNPC_ID` = runtime ID from terminator NPC 1681
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1260

**Tags:** grant-timer=100s, complete-timer-gate=100s, chains-to=1261, fail-restart=1255

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1682** (proximity/talk).
- Grant timer **100**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **100**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1260
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1260
- `iNPC_ID` = runtime ID from terminator NPC 1682
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.

## Task 1261

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **304**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **722** (proximity/talk).
- Collect items: item 326 x5
- Reward table ID **280** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1261
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1261
- `iNPC_ID` = runtime ID from terminator NPC 722
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Run through the "Keys to the City" challenge.
