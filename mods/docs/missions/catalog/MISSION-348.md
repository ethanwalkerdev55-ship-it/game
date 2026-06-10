# Mission 348 — Stealing Secrets (Part 3 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 348 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1923 | UseItems | — | — | 1924 | — | 731 | 2300 |
| 1924 | UseItems | — | — | 1925 | — | — | 2301 |
| 1925 | UseItems | — | — | 1926 | — | — | 2302 |
| 1926 | Talk | — | — | — | — | — | 731 |

## Chain edges (from table)

- Success: **1923** → **1924**
- Success: **1924** → **1925**
- Success: **1925** → **1926**

## Task 1923

**Tags:** chains-to=1924

### How this task is received

- Player accepts from NPC table ID **731** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **347**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **731** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2300** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1923
- `iNPC_ID` = runtime ID from grant NPC 731
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1923
- `iNPC_ID` = runtime ID from terminator NPC 2300
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Fusion Matter Detectors in Mandark's lab.

## Task 1924

**Tags:** chains-to=1925

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **347**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2301** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1924
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1924
- `iNPC_ID` = runtime ID from terminator NPC 2301
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Mandark thinks I do not notice things that are easy to notice. He suspects I am up to no good, but it is Mandark who is up to no good! Let's catch him.

## Task 1925

**Tags:** chains-to=1926

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **347**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2302** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1925
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1925
- `iNPC_ID` = runtime ID from terminator NPC 2302
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Fusion Matter Detectors in Mandark's lab.

## Task 1926

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **347**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **731** (proximity/talk).
- Reward table ID **438** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1926
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1926
- `iNPC_ID` = runtime ID from terminator NPC 731
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Fusion Matter Detectors in Mandark's lab.
