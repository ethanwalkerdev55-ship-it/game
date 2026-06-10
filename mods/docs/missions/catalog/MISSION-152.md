# Mission 152 — Spex Trek

| Field | Value |
|-------|-------|
| Mission ID | 152 |
| Mission type | Guide |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1446 | Delivery | — | — | 1447 | — | 707 | 710 |
| 1447 | GotoLocation | — | — | 1448 | — | — | 913 |
| 1448 | UseItems | — | — | 1449 | — | — | 1832 |
| 1449 | Delivery | — | — | — | — | — | 710 |

## Chain edges (from table)

- Success: **1446** → **1447**
- Success: **1447** → **1448**
- Success: **1448** → **1449**

## Task 1446

**Tags:** item-quota, chains-to=1447

### How this task is received

- Player accepts from NPC table ID **707** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **70**.

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **707** (journal accept or auto-chain).
- Complete at terminator NPC table ID **710** (proximity/talk).
- Collect items: item 364 x0

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1446
- `iNPC_ID` = runtime ID from grant NPC 707
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1446
- `iNPC_ID` = runtime ID from terminator NPC 710
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Billy in Endsville.

## Task 1447

**Tags:** chains-to=1448

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **70**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **913** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1447
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1447
- `iNPC_ID` = runtime ID from terminator NPC 913
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Billy in Endsville.

## Task 1448

**Tags:** chains-to=1449

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **70**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1832** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1448
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1448
- `iNPC_ID` = runtime ID from terminator NPC 1832
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Billy in Endsville.

## Task 1449

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **70**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **710** (proximity/talk).
- Collect items: item 365 x1
- Reward table ID **332** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1449
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1449
- `iNPC_ID` = runtime ID from terminator NPC 710
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Billy in Endsville.
