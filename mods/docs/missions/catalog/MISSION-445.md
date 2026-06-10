# Mission 445 — Water You Kidding? (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 445 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1370 | Talk | — | grant:240, gate:240 | 1372 | 1371 | 727 | 726 |
| 1371 | Talk | — | — | 1370 | — | — | 727 |
| 1372 | Delivery | — | — | 1373 | — | — | 727 |
| 1373 | Talk | — | — | — | — | — | 726 |

## Chain edges (from table)

- Success: **1370** → **1372**
- Fail (err 1/12): **1370** → **1371**
- Success: **1371** → **1370**
- Success: **1372** → **1373**

## Task 1370

**Tags:** grant-timer=240s, complete-timer-gate=240s, chains-to=1372, fail-restart=1371

### How this task is received

- Player accepts from NPC table ID **727** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **444**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **727** (journal accept or auto-chain).
- Complete at terminator NPC table ID **726** (proximity/talk).
- Grant timer **240**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **240**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1370
- `iNPC_ID` = runtime ID from grant NPC 727
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1370
- `iNPC_ID` = runtime ID from terminator NPC 726
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Don't let Bloo eat the fried dough.

## Task 1371

**Tags:** chains-to=1370

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **444**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **727** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1371
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1371
- `iNPC_ID` = runtime ID from terminator NPC 727
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Don't let Bloo eat the fried dough.

## Task 1372

**Tags:** item-quota, chains-to=1373

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **444**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **727** (proximity/talk).
- Collect items: item 348 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1372
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1372
- `iNPC_ID` = runtime ID from terminator NPC 727
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Don't let Bloo eat the fried dough.

## Task 1373

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **444**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **726** (proximity/talk).
- Reward table ID **310** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1373
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1373
- `iNPC_ID` = runtime ID from terminator NPC 726
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Don't let Bloo eat the fried dough.
