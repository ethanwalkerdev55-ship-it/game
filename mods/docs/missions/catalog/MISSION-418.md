# Mission 418 — Double Fusion Trouble (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 418 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1042 | Talk | — | — | 1043 | — | 1192 | 718 |
| 1043 | UseItems | — | — | 1044 | — | — | 1460 |
| 1044 | Delivery | — | — | — | — | — | 1192 |

## Chain edges (from table)

- Success: **1042** → **1043**
- Success: **1043** → **1044**

## Task 1042

**Tags:** chains-to=1043

### How this task is received

- Player accepts from NPC table ID **1192** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **417**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **1192** (journal accept or auto-chain).
- Complete at terminator NPC table ID **718** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1042
- `iNPC_ID` = runtime ID from grant NPC 1192
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1042
- `iNPC_ID` = runtime ID from terminator NPC 718
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get an item from Coop.

## Task 1043

**Tags:** chains-to=1044

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **417**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1460** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1043
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1043
- `iNPC_ID` = runtime ID from terminator NPC 1460
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get an item from Coop.

## Task 1044

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **417**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1192** (proximity/talk).
- Collect items: item 263 x1
- Reward table ID **242** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1044
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1044
- `iNPC_ID` = runtime ID from terminator NPC 1192
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get an item from Coop.
