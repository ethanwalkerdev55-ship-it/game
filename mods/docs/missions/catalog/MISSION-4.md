# Mission 4 — Dee Dee's Dilemmas (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 4 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 44 | Delivery | — | — | 45 | — | 701 | 713 |
| 45 | Delivery | — | — | 46 | — | — | 707 |
| 46 | Delivery | — | — | — | — | — | 701 |

## Chain edges (from table)

- Success: **44** → **45**
- Success: **45** → **46**

## Task 44

**Tags:** item-quota, chains-to=45

### How this task is received

- Player accepts from NPC table ID **701** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **3**.

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **701** (journal accept or auto-chain).
- Complete at terminator NPC table ID **713** (proximity/talk).
- Collect items: item 34 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 44
- `iNPC_ID` = runtime ID from grant NPC 701
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 44
- `iNPC_ID` = runtime ID from terminator NPC 713
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Return Dee Dee's flowers to their proper owner.

## Task 45

**Tags:** item-quota, chains-to=46

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **3**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **707** (proximity/talk).
- Collect items: item 34 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 45
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 45
- `iNPC_ID` = runtime ID from terminator NPC 707
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Return Dee Dee's flowers to their proper owner.

## Task 46

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **3**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **701** (proximity/talk).
- Collect items: item 34 x1
- Reward table ID **14** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 46
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 46
- `iNPC_ID` = runtime ID from terminator NPC 701
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Return Dee Dee's flowers to their proper owner.
