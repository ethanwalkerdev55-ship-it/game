# Mission 502 — Tech Support

| Field | Value |
|-------|-------|
| Mission ID | 502 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 452 | Talk | — | — | 455 | — | 769 | 771 |
| 453 | UseItems | — | — | 454 | — | — | 870 |
| 454 | Delivery | — | — | 455 | — | — | 771 |
| 455 | Delivery | — | — | — | — | — | 769 |

## Chain edges (from table)

- Success: **452** → **455**
- Success: **453** → **454**
- Success: **454** → **455**

## Task 452

**Tags:** chains-to=455

### How this task is received

- Player accepts from NPC table ID **769** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **503**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **769** (journal accept or auto-chain).
- Complete at terminator NPC table ID **771** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 452
- `iNPC_ID` = runtime ID from grant NPC 769
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 452
- `iNPC_ID` = runtime ID from terminator NPC 771
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get a spare Gravity Decelerator.

## Task 453

**Tags:** chains-to=454

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **503**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **870** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 453
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 453
- `iNPC_ID` = runtime ID from terminator NPC 870
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get a spare Gravity Decelerator.

## Task 454

**Tags:** item-quota, chains-to=455

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **503**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **771** (proximity/talk).
- Collect items: item 130 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 454
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 454
- `iNPC_ID` = runtime ID from terminator NPC 771
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get a spare Gravity Decelerator.

## Task 455

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **503**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **769** (proximity/talk).
- Collect items: item 131 x1
- Reward table ID **107** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 455
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 455
- `iNPC_ID` = runtime ID from terminator NPC 769
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get a spare Gravity Decelerator.
