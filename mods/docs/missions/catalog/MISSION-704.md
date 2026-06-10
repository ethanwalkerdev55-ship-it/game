# Mission 704 — Arrrmed Forces (Part 4 of 6)

| Field | Value |
|-------|-------|
| Mission ID | 704 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2489 | GotoLocation | — | — | 2490 | — | 3090 | 1788 |
| 2490 | GotoLocation | — | — | 2491 | — | — | 2744 |
| 2491 | Talk | — | — | — | — | — | 3090 |

## Chain edges (from table)

- Success: **2489** → **2490**
- Success: **2490** → **2491**

## Task 2489

**Tags:** chains-to=2490

### How this task is received

- Player accepts from NPC table ID **3090** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **703**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **3090** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1788** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2489
- `iNPC_ID` = runtime ID from grant NPC 3090
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2489
- `iNPC_ID` = runtime ID from terminator NPC 1788
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Boost the Shortbread Radio's signal.

## Task 2490

**Tags:** chains-to=2491

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **703**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2744** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2490
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2490
- `iNPC_ID` = runtime ID from terminator NPC 2744
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Boost the Shortbread Radio's signal.

## Task 2491

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **703**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3090** (proximity/talk).
- Reward table ID **628** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2491
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2491
- `iNPC_ID` = runtime ID from terminator NPC 3090
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Boost the Shortbread Radio's signal.
