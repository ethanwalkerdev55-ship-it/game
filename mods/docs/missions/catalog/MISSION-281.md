# Mission 281 — Favors for Father (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 281 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 900 | UseItems | — | — | 901 | — | 709 | 1391 |
| 901 | UseItems | — | — | 902 | — | — | 1392 |
| 902 | UseItems | — | — | 903 | — | — | 1393 |
| 903 | UseItems | — | — | — | — | — | 2565 |

## Chain edges (from table)

- Success: **900** → **901**
- Success: **901** → **902**
- Success: **902** → **903**

## Task 900

**Tags:** chains-to=901

### How this task is received

- Player accepts from NPC table ID **709** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **280**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **709** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1391** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 900
- `iNPC_ID` = runtime ID from grant NPC 709
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 900
- `iNPC_ID` = runtime ID from terminator NPC 1391
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sabotage the Gooby Traps.

## Task 901

**Tags:** chains-to=902

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **280**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1392** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 901
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 901
- `iNPC_ID` = runtime ID from terminator NPC 1392
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sabotage the Gooby Traps.

## Task 902

**Tags:** chains-to=903

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **280**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1393** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 902
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 902
- `iNPC_ID` = runtime ID from terminator NPC 1393
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sabotage the Gooby Traps.

## Task 903

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **280**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2565** (proximity/talk).
- Reward table ID **207** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 903
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 903
- `iNPC_ID` = runtime ID from terminator NPC 2565
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sabotage the Gooby Traps.
