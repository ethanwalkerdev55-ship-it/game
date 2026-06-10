# Mission 619 — Non-Prophet Organization (Part 4 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 619 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2341 | UseItems | — | — | 2342 | — | 744 | 2831 |
| 2342 | Talk | — | — | — | — | — | 738 |

## Chain edges (from table)

- Success: **2341** → **2342**

## Task 2341

**Tags:** chains-to=2342

### How this task is received

- Player accepts from NPC table ID **744** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **618**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **744** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2831** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2341
- `iNPC_ID` = runtime ID from grant NPC 744
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2341
- `iNPC_ID` = runtime ID from terminator NPC 2831
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Activate the totem in the Ruins.

## Task 2342

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **618**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **738** (proximity/talk).
- Reward table ID **566** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2342
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2342
- `iNPC_ID` = runtime ID from terminator NPC 738
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Activate the totem in the Ruins.
