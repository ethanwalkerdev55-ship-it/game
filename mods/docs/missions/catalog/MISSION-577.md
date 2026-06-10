# Mission 577 — The Thromnambular Saga (Part 8 of 9)

| Field | Value |
|-------|-------|
| Mission ID | 577 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2299 | UseItems | — | — | 2300 | — | 2808 | 2810 |
| 2300 | UseItems | — | — | 2301 | — | — | 2811 |
| 2301 | UseItems | — | — | 2302 | — | — | 2812 |
| 2302 | Talk | — | — | — | — | — | 2808 |

## Chain edges (from table)

- Success: **2299** → **2300**
- Success: **2300** → **2301**
- Success: **2301** → **2302**

## Task 2299

**Tags:** chains-to=2300

### How this task is received

- Player accepts from NPC table ID **2808** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **576**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **2808** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2810** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2299
- `iNPC_ID` = runtime ID from grant NPC 2808
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2299
- `iNPC_ID` = runtime ID from terminator NPC 2810
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 2300

**Tags:** chains-to=2301

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **576**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2811** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2300
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2300
- `iNPC_ID` = runtime ID from terminator NPC 2811
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 2301

**Tags:** chains-to=2302

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **576**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2812** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2301
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2301
- `iNPC_ID` = runtime ID from terminator NPC 2812
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.

## Task 2302

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **576**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2808** (proximity/talk).
- Reward table ID **547** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2302
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2302
- `iNPC_ID` = runtime ID from terminator NPC 2808
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Complete a mission for Thromnambular.
