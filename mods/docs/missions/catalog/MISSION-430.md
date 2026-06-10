# Mission 430 — The Littlest Fusion (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 430 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1087 | Delivery | — | — | 1088 | — | 705 | 704 |
| 1088 | Delivery | — | — | 1089 | — | — | 702 |
| 1089 | Delivery | — | — | — | — | — | 703 |

## Chain edges (from table)

- Success: **1087** → **1088**
- Success: **1088** → **1089**

## Task 1087

**Tags:** item-quota, chains-to=1088

### How this task is received

- Player accepts from NPC table ID **705** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **429**.

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **705** (journal accept or auto-chain).
- Complete at terminator NPC table ID **704** (proximity/talk).
- Collect items: item 271 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1087
- `iNPC_ID` = runtime ID from grant NPC 705
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1087
- `iNPC_ID` = runtime ID from terminator NPC 704
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the weird pickle.

## Task 1088

**Tags:** item-quota, chains-to=1089

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **429**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **702** (proximity/talk).
- Collect items: item 271 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1088
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1088
- `iNPC_ID` = runtime ID from terminator NPC 702
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the weird pickle.

## Task 1089

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **429**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **703** (proximity/talk).
- Collect items: item 271 x1
- Reward table ID **253** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1089
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1089
- `iNPC_ID` = runtime ID from terminator NPC 703
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the weird pickle.
