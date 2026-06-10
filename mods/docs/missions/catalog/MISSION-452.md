# Mission 452 — Scotsman's Best Friend (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 452 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2068 | Talk | — | — | 2069 | — | 724 | 725 |
| 2069 | Talk | — | — | 2070 | — | — | 738 |
| 2070 | GotoLocation | — | — | 2071 | — | — | 1602 |
| 2071 | GotoLocation | — | — | — | — | — | 2277 |

## Chain edges (from table)

- Success: **2068** → **2069**
- Success: **2069** → **2070**
- Success: **2070** → **2071**

## Task 2068

**Tags:** chains-to=2069

### How this task is received

- Player accepts from NPC table ID **724** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **451**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **724** (journal accept or auto-chain).
- Complete at terminator NPC table ID **725** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2068
- `iNPC_ID` = runtime ID from grant NPC 724
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2068
- `iNPC_ID` = runtime ID from terminator NPC 725
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the Runic Charm.

## Task 2069

**Tags:** chains-to=2070

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **451**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **738** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2069
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2069
- `iNPC_ID` = runtime ID from terminator NPC 738
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the Runic Charm.

## Task 2070

**Tags:** chains-to=2071

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **451**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1602** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2070
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2070
- `iNPC_ID` = runtime ID from terminator NPC 1602
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the Runic Charm.

## Task 2071

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **451**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2277** (proximity/talk).
- Collect items: item 523 x1
- Reward table ID **475** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2071
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2071
- `iNPC_ID` = runtime ID from terminator NPC 2277
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate the Runic Charm.
