# Mission 64 — What the Hades

| Field | Value |
|-------|-------|
| Mission ID | 64 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 246 | GotoLocation | — | — | 247 | — | 734 | 1806 |
| 247 | UseItems | — | — | 248 | — | — | 1818 |
| 248 | Delivery | — | — | — | — | — | 734 |

## Chain edges (from table)

- Success: **246** → **247**
- Success: **247** → **248**

## Task 246

**Tags:** chains-to=247

### How this task is received

- Player accepts from NPC table ID **734** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **63**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **734** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1806** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 246
- `iNPC_ID` = runtime ID from grant NPC 734
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 246
- `iNPC_ID` = runtime ID from terminator NPC 1806
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a light bulb from the Ruins.

## Task 247

**Tags:** chains-to=248

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **63**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1818** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 247
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 247
- `iNPC_ID` = runtime ID from terminator NPC 1818
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a light bulb from the Ruins.

## Task 248

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **63**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **734** (proximity/talk).
- Collect items: item 77 x1
- Reward table ID **64** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 248
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 248
- `iNPC_ID` = runtime ID from terminator NPC 734
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a light bulb from the Ruins.
