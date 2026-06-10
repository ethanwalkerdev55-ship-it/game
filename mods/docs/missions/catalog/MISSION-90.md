# Mission 90 — Gun Run

| Field | Value |
|-------|-------|
| Mission ID | 90 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 365 | GotoLocation | — | — | 366 | — | 709 | 913 |
| 366 | UseItems | — | — | 367 | — | — | 859 |
| 367 | Delivery | — | — | — | — | — | 709 |

## Chain edges (from table)

- Success: **365** → **366**
- Success: **366** → **367**

## Task 365

**Tags:** chains-to=366

### How this task is received

- Player accepts from NPC table ID **709** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **89**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **709** (journal accept or auto-chain).
- Complete at terminator NPC table ID **913** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 365
- `iNPC_ID` = runtime ID from grant NPC 709
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 365
- `iNPC_ID` = runtime ID from terminator NPC 913
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a power cell for a new weapon.

## Task 366

**Tags:** chains-to=367

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **89**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **859** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 366
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 366
- `iNPC_ID` = runtime ID from terminator NPC 859
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a power cell for a new weapon.

## Task 367

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **89**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **709** (proximity/talk).
- Collect items: item 113 x1
- Reward table ID **90** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 367
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 367
- `iNPC_ID` = runtime ID from terminator NPC 709
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a power cell for a new weapon.
