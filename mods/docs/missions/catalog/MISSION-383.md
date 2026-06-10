# Mission 383 — Don't Go, Coco (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 383 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2025 | Talk | — | — | 2026 | — | 747 | 742 |
| 2026 | Talk | — | — | 2027 | — | — | 743 |
| 2027 | Talk | — | — | — | — | — | 747 |

## Chain edges (from table)

- Success: **2025** → **2026**
- Success: **2026** → **2027**

## Task 2025

**Tags:** chains-to=2026

### How this task is received

- Player accepts from NPC table ID **747** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **374**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **747** (journal accept or auto-chain).
- Complete at terminator NPC table ID **742** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2025
- `iNPC_ID` = runtime ID from grant NPC 747
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2025
- `iNPC_ID` = runtime ID from terminator NPC 742
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out if Coco is a traitor.

## Task 2026

**Tags:** chains-to=2027

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **374**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **743** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2026
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2026
- `iNPC_ID` = runtime ID from terminator NPC 743
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out if Coco is a traitor.

## Task 2027

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **374**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **747** (proximity/talk).
- Reward table ID **465** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2027
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2027
- `iNPC_ID` = runtime ID from terminator NPC 747
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out if Coco is a traitor.
