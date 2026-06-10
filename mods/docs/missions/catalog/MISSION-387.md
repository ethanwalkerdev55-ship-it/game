# Mission 387 — Kevin Help Us (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 387 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2035 | Talk | — | — | 2036 | — | 749 | 716 |
| 2036 | Talk | — | — | 2037 | — | — | 715 |
| 2037 | UseItems | — | — | 2038 | — | — | 1464 |
| 2038 | Talk | — | — | — | — | — | 749 |

## Chain edges (from table)

- Success: **2035** → **2036**
- Success: **2036** → **2037**
- Success: **2037** → **2038**

## Task 2035

**Tags:** chains-to=2036

### How this task is received

- Player accepts from NPC table ID **749** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **386**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **749** (journal accept or auto-chain).
- Complete at terminator NPC table ID **716** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2035
- `iNPC_ID` = runtime ID from grant NPC 749
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2035
- `iNPC_ID` = runtime ID from terminator NPC 716
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Use Imaginary Energy against Fuse.

## Task 2036

**Tags:** chains-to=2037

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **386**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **715** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2036
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2036
- `iNPC_ID` = runtime ID from terminator NPC 715
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Use Imaginary Energy against Fuse.

## Task 2037

**Tags:** chains-to=2038

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **386**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1464** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2037
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2037
- `iNPC_ID` = runtime ID from terminator NPC 1464
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Use Imaginary Energy against Fuse.

## Task 2038

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **386**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **749** (proximity/talk).
- Reward table ID **467** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2038
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2038
- `iNPC_ID` = runtime ID from terminator NPC 749
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Use Imaginary Energy against Fuse.
