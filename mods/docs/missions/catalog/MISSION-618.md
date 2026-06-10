# Mission 618 — Non-Prophet Organization (Part 3 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 618 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2339 | Talk | — | — | 2340 | — | 744 | 710 |
| 2340 | Talk | — | — | — | — | — | 744 |

## Chain edges (from table)

- Success: **2339** → **2340**

## Task 2339

**Tags:** chains-to=2340

### How this task is received

- Player accepts from NPC table ID **744** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **617**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **744** (journal accept or auto-chain).
- Complete at terminator NPC table ID **710** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2339
- `iNPC_ID` = runtime ID from grant NPC 744
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2339
- `iNPC_ID` = runtime ID from terminator NPC 710
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Whack Billy with the Pillow of Common Sense.

## Task 2340

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **617**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **744** (proximity/talk).
- Reward table ID **565** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2340
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2340
- `iNPC_ID` = runtime ID from terminator NPC 744
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Whack Billy with the Pillow of Common Sense.
