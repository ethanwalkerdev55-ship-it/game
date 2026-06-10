# Mission 347 — Stealing Secrets (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 347 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1919 | UseItems | — | — | 1920 | — | 729 | 2297 |
| 1920 | UseItems | — | — | 1921 | — | — | 2298 |
| 1921 | UseItems | — | — | 1922 | — | — | 2299 |
| 1922 | Talk | — | — | — | — | — | 729 |

## Chain edges (from table)

- Success: **1919** → **1920**
- Success: **1920** → **1921**
- Success: **1921** → **1922**

## Task 1919

**Tags:** chains-to=1920

### How this task is received

- Player accepts from NPC table ID **729** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **346**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **729** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2297** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1919
- `iNPC_ID` = runtime ID from grant NPC 729
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1919
- `iNPC_ID` = runtime ID from terminator NPC 2297
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sneak into Mojo's volcano.

## Task 1920

**Tags:** chains-to=1921

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **346**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2298** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1920
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1920
- `iNPC_ID` = runtime ID from terminator NPC 2298
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sneak into Mojo's volcano.

## Task 1921

**Tags:** chains-to=1922

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **346**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2299** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1921
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1921
- `iNPC_ID` = runtime ID from terminator NPC 2299
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sneak into Mojo's volcano.

## Task 1922

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **346**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **729** (proximity/talk).
- Reward table ID **437** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1922
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1922
- `iNPC_ID` = runtime ID from terminator NPC 729
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Sneak into Mojo's volcano.
