# Mission 788 — Welcome to Endsville (Part 6 of 6)

| Field | Value |
|-------|-------|
| Mission ID | 788 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5012 | GotoLocation | — | — | 5013 | — | 758 | 1364 |
| 5013 | Talk | — | — | 5014 | — | 1364 | 3268 |
| 5014 | Talk | — | — | — | — | — | 758 |

## Chain edges (from table)

- Success: **5012** → **5013**
- Success: **5013** → **5014**

## Task 5012

**Tags:** chains-to=5013

### How this task is received

- Player accepts from NPC table ID **758** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **758** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1364** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5012
- `iNPC_ID` = runtime ID from grant NPC 758
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5012
- `iNPC_ID` = runtime ID from terminator NPC 1364
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Go to Eternal Vistas

## Task 5013

**Tags:** chains-to=5014

### How this task is received

- Player accepts from NPC table ID **1364** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **1364** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3268** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5013
- `iNPC_ID` = runtime ID from grant NPC 1364
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5013
- `iNPC_ID` = runtime ID from terminator NPC 3268
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Go to Eternal Vistas

## Task 5014

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **758** (proximity/talk).
- Reward table ID **157** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5014
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5014
- `iNPC_ID` = runtime ID from terminator NPC 758
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Go to Eternal Vistas
