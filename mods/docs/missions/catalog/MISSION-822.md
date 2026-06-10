# Mission 822 — Mega Fusion Education (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 822 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5134 | GotoLocation | — | — | 5135 | — | 697 | 1594 |
| 5135 | GotoLocation | — | — | 5136 | — | — | 1584 |
| 5136 | Talk | — | — | — | — | — | 697 |

## Chain edges (from table)

- Success: **5134** → **5135**
- Success: **5135** → **5136**

## Task 5134

**Tags:** chains-to=5135

### How this task is received

- Player accepts from NPC table ID **697** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **821**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **697** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1594** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5134
- `iNPC_ID` = runtime ID from grant NPC 697
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5134
- `iNPC_ID` = runtime ID from terminator NPC 1594
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Learn how to use Null Void coordinate data from Albedo.

## Task 5135

**Tags:** chains-to=5136

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1584** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5135
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5135
- `iNPC_ID` = runtime ID from terminator NPC 1584
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Learn how to use Null Void coordinate data from Albedo.

## Task 5136

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **697** (proximity/talk).
- Reward table ID **711** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5136
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5136
- `iNPC_ID` = runtime ID from terminator NPC 697
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Learn how to use Null Void coordinate data from Albedo.
