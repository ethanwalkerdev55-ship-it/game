# Mission 765 — All the World's a Stage 
(Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 765 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2675 | GotoLocation | — | — | 2676 | — | 751 | 2021 |
| 2676 | UseItems | — | — | 2678 | — | — | 3244 |
| 2677 | GotoLocation | — | — | 2676 | — | — | 2021 |
| 2678 | Talk | — | grant:150, gate:150 | — | 2677 | — | 751 |

## Chain edges (from table)

- Success: **2675** → **2676**
- Success: **2676** → **2678**
- Success: **2677** → **2676**
- Fail (err 1/12): **2678** → **2677**

## Task 2675

**Tags:** chains-to=2676

### How this task is received

- Player accepts from NPC table ID **751** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **764**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **751** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2021** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2675
- `iNPC_ID` = runtime ID from grant NPC 751
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2675
- `iNPC_ID` = runtime ID from terminator NPC 2021
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Light the flame within the Hibachi.

## Task 2676

**Tags:** chains-to=2678

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **764**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3244** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2676
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2676
- `iNPC_ID` = runtime ID from terminator NPC 3244
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Light the flame within the Hibachi.

## Task 2677

**Tags:** chains-to=2676

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **764**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2021** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2677
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2677
- `iNPC_ID` = runtime ID from terminator NPC 2021
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Light the flame within the Hibachi.

## Task 2678

**Tags:** grant-timer=150s, complete-timer-gate=150s, fail-restart=2677

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **764**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **751** (proximity/talk).
- Grant timer **150**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **150**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **677** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2678
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2678
- `iNPC_ID` = runtime ID from terminator NPC 751
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Light the flame within the Hibachi.
