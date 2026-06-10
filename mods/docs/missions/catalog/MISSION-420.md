# Mission 420 — Double Fusion Trouble (Part 4 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 420 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1051 | Talk | — | — | 1052 | — | 1192 | 701 |
| 1052 | UseItems | — | — | 1053 | — | — | 1462 |
| 1053 | Delivery | — | — | — | — | — | 1192 |

## Chain edges (from table)

- Success: **1051** → **1052**
- Success: **1052** → **1053**

## Task 1051

**Tags:** chains-to=1052

### How this task is received

- Player accepts from NPC table ID **1192** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **419**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **1192** (journal accept or auto-chain).
- Complete at terminator NPC table ID **701** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1051
- `iNPC_ID` = runtime ID from grant NPC 1192
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1051
- `iNPC_ID` = runtime ID from terminator NPC 701
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a Dexlabs Power Cell.

## Task 1052

**Tags:** chains-to=1053

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **419**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1462** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1052
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1052
- `iNPC_ID` = runtime ID from terminator NPC 1462
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a Dexlabs Power Cell.

## Task 1053

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **419**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1192** (proximity/talk).
- Collect items: item 265 x1
- Reward table ID **244** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1053
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1053
- `iNPC_ID` = runtime ID from terminator NPC 1192
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a Dexlabs Power Cell.
