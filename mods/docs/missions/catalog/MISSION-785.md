# Mission 785 — The Grooviest Fusion (Part 1 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 785 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5002 | Talk | — | — | 5003 | — | 3268 | 710 |
| 5003 | Talk | — | — | — | — | 2971 | 3268 |

## Chain edges (from table)

- Success: **5002** → **5003**

## Task 5002

**Tags:** chains-to=5003

### How this task is received

- Player accepts from NPC table ID **3268** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3268** (journal accept or auto-chain).
- Complete at terminator NPC table ID **710** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5002
- `iNPC_ID` = runtime ID from grant NPC 3268
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5002
- `iNPC_ID` = runtime ID from terminator NPC 710
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Ask Billy for a Glass of Water

## Task 5003

**Tags:** standard

### How this task is received

- Player accepts from NPC table ID **2971** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **2971** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3268** (proximity/talk).
- Reward table ID **701** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5003
- `iNPC_ID` = runtime ID from grant NPC 2971
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5003
- `iNPC_ID` = runtime ID from terminator NPC 3268
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Ask Billy for a Glass of Water
