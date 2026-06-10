# Mission 120 — Chicken Scrap

| Field | Value |
|-------|-------|
| Mission ID | 120 |
| Mission type | Guide |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1625 | Talk | — | — | 1627 | — | 747 | 749 |
| 1626 | Defeat | — | — | 1628 | — | — | — |
| 1627 | GotoLocation | — | — | 1626 | — | 749 | 2712 |
| 1628 | Delivery | — | — | — | — | — | 732 |

## Chain edges (from table)

- Success: **1625** → **1627**
- Success: **1626** → **1628**
- Success: **1627** → **1626**

## Task 1625

**Tags:** chains-to=1627

### How this task is received

- Player accepts from NPC table ID **747** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **116**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **747** (journal accept or auto-chain).
- Complete at terminator NPC table ID **749** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1625
- `iNPC_ID` = runtime ID from grant NPC 747
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1625
- `iNPC_ID` = runtime ID from terminator NPC 749
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Kevin in Nowhere.

## Task 1626

**Tags:** kill-quota, chains-to=1628

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **116**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 430 x7

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1626
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1626
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Kevin in Nowhere.

## Task 1627

**Tags:** chains-to=1626

### How this task is received

- Player accepts from NPC table ID **749** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **116**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **749** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2712** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1627
- `iNPC_ID` = runtime ID from grant NPC 749
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1627
- `iNPC_ID` = runtime ID from terminator NPC 2712
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Kevin in Nowhere.

## Task 1628

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **116**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **732** (proximity/talk).
- Collect items: item 419 x1
- Reward table ID **367** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1628
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1628
- `iNPC_ID` = runtime ID from terminator NPC 732
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Kevin in Nowhere.
