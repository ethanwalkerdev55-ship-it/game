# Mission 518 — Jack and the Giant Robot

| Field | Value |
|-------|-------|
| Mission ID | 518 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 587 | GotoLocation | — | — | 588 | — | 776 | 1085 |
| 588 | GotoLocation | — | — | 589 | — | — | 1086 |
| 589 | Talk | — | — | — | — | — | 776 |

## Chain edges (from table)

- Success: **587** → **588**
- Success: **588** → **589**

## Task 587

**Tags:** chains-to=588

### How this task is received

- Player accepts from NPC table ID **776** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **776** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1085** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 587
- `iNPC_ID` = runtime ID from grant NPC 776
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 587
- `iNPC_ID` = runtime ID from terminator NPC 1085
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Samurai Jack in the junk yard.

## Task 588

**Tags:** chains-to=589

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1086** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 588
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 588
- `iNPC_ID` = runtime ID from terminator NPC 1086
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Samurai Jack in the junk yard.

## Task 589

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **776** (proximity/talk).
- Reward table ID **134** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 589
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 589
- `iNPC_ID` = runtime ID from terminator NPC 776
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Samurai Jack in the junk yard.
