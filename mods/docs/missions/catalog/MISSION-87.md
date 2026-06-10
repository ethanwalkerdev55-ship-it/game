# Mission 87 — First Matey

| Field | Value |
|-------|-------|
| Mission ID | 87 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 353 | Talk | — | — | 354 | — | 2560 | 706 |
| 354 | UseItems | — | — | 355 | — | — | 1826 |
| 355 | Delivery | — | — | — | — | — | 709 |

## Chain edges (from table)

- Success: **353** → **354**
- Success: **354** → **355**

## Task 353

**Tags:** chains-to=354

### How this task is received

- Player accepts from NPC table ID **2560** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **91**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **2560** (journal accept or auto-chain).
- Complete at terminator NPC table ID **706** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 353
- `iNPC_ID` = runtime ID from grant NPC 2560
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 353
- `iNPC_ID` = runtime ID from terminator NPC 706
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a map of the suburbs from Stickybeard.

## Task 354

**Tags:** chains-to=355

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **91**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1826** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 354
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 354
- `iNPC_ID` = runtime ID from terminator NPC 1826
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a map of the suburbs from Stickybeard.

## Task 355

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **91**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **709** (proximity/talk).
- Collect items: item 109 x1
- Reward table ID **87** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 355
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 355
- `iNPC_ID` = runtime ID from terminator NPC 709
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a map of the suburbs from Stickybeard.
