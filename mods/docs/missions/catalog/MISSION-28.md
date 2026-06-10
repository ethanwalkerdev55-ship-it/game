# Mission 28 — Follow Your Node

| Field | Value |
|-------|-------|
| Mission ID | 28 |
| Mission type | Guide |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 115 | UseItems | — | — | 116 | — | 704 | 833 |
| 116 | UseItems | — | — | 117 | — | — | 834 |
| 117 | UseItems | — | — | 118 | — | — | 835 |
| 118 | Talk | — | — | — | — | — | 704 |

## Chain edges (from table)

- Success: **115** → **116**
- Success: **116** → **117**
- Success: **117** → **118**

## Task 115

**Tags:** chains-to=116

### How this task is received

- Player accepts from NPC table ID **704** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **27**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **704** (journal accept or auto-chain).
- Complete at terminator NPC table ID **833** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 115
- `iNPC_ID` = runtime ID from grant NPC 704
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 115
- `iNPC_ID` = runtime ID from terminator NPC 833
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three triangulation nodes for Numbuh Two's LOCATOR device.

## Task 116

**Tags:** chains-to=117

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **27**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **834** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 116
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 116
- `iNPC_ID` = runtime ID from terminator NPC 834
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three triangulation nodes for Numbuh Two's LOCATOR device.

## Task 117

**Tags:** chains-to=118

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **27**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **835** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 117
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 117
- `iNPC_ID` = runtime ID from terminator NPC 835
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three triangulation nodes for Numbuh Two's LOCATOR device.

## Task 118

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **27**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **704** (proximity/talk).
- Reward table ID **28** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 118
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 118
- `iNPC_ID` = runtime ID from terminator NPC 704
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place three triangulation nodes for Numbuh Two's LOCATOR device.
