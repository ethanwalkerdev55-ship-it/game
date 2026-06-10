# Mission 651 — Planetoid Fusion (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 651 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2171 | UseItems | — | — | 2172 | — | 731 | 3043 |
| 2172 | Talk | — | — | — | — | — | 731 |

## Chain edges (from table)

- Success: **2171** → **2172**

## Task 2171

**Tags:** chains-to=2172

### How this task is received

- Player accepts from NPC table ID **731** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **650**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **731** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3043** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2171
- `iNPC_ID` = runtime ID from grant NPC 731
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2171
- `iNPC_ID` = runtime ID from terminator NPC 3043
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Install Mojo's Telescope Lens.

## Task 2172

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **650**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **731** (proximity/talk).
- Reward table ID **503** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2172
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2172
- `iNPC_ID` = runtime ID from terminator NPC 731
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Install Mojo's Telescope Lens.
