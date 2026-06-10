# Mission 371 — Mystery of the Meteor (Part 4 of 6)

| Field | Value |
|-------|-------|
| Mission ID | 371 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1999 | GotoLocation | — | — | 2000 | — | 738 | 2268 |
| 2000 | UseItems | — | — | 2001 | — | — | 2313 |
| 2001 | Talk | — | — | — | — | — | 738 |

## Chain edges (from table)

- Success: **1999** → **2000**
- Success: **2000** → **2001**

## Task 1999

**Tags:** chains-to=2000

### How this task is received

- Player accepts from NPC table ID **738** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **370**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **738** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2268** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1999
- `iNPC_ID` = runtime ID from grant NPC 738
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1999
- `iNPC_ID` = runtime ID from terminator NPC 2268
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Dispose of the decoy meteor.

## Task 2000

**Tags:** chains-to=2001

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **370**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2313** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2000
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2000
- `iNPC_ID` = runtime ID from terminator NPC 2313
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Dispose of the decoy meteor.

## Task 2001

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **370**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **738** (proximity/talk).
- Reward table ID **457** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2001
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2001
- `iNPC_ID` = runtime ID from terminator NPC 738
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Dispose of the decoy meteor.
