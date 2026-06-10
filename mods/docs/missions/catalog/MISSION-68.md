# Mission 68 — Map Quest

| Field | Value |
|-------|-------|
| Mission ID | 68 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 263 | GotoLocation | — | — | 264 | — | 707 | 1124 |
| 264 | UseItems | — | — | 265 | — | — | 849 |
| 265 | Delivery | — | — | — | — | — | 707 |

## Chain edges (from table)

- Success: **263** → **264**
- Success: **264** → **265**

## Task 263

**Tags:** chains-to=264

### How this task is received

- Player accepts from NPC table ID **707** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **67**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **707** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1124** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 263
- `iNPC_ID` = runtime ID from grant NPC 707
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 263
- `iNPC_ID` = runtime ID from terminator NPC 1124
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the treasure map at Pokey Oaks Junior High.

## Task 264

**Tags:** chains-to=265

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **67**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **849** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 264
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 264
- `iNPC_ID` = runtime ID from terminator NPC 849
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the treasure map at Pokey Oaks Junior High.

## Task 265

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **67**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **707** (proximity/talk).
- Collect items: item 81 x1
- Reward table ID **68** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 265
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 265
- `iNPC_ID` = runtime ID from terminator NPC 707
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the treasure map at Pokey Oaks Junior High.
