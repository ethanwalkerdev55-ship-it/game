# Mission 774 — Chocolate Milk
(Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 774 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2629 | Talk | — | — | 2630 | — | 3221 | 3222 |
| 2630 | Talk | — | — | — | — | — | 3121 |

## Chain edges (from table)

- Success: **2629** → **2630**

## Task 2629

**Tags:** chains-to=2630

### How this task is received

- Player accepts from NPC table ID **3221** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **773**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3221** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3222** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2629
- `iNPC_ID` = runtime ID from grant NPC 3221
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2629
- `iNPC_ID` = runtime ID from terminator NPC 3222
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find some Chocolate Milk for Cheese.

## Task 2630

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **773**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3121** (proximity/talk).
- Reward table ID **664** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2630
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2630
- `iNPC_ID` = runtime ID from terminator NPC 3121
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find some Chocolate Milk for Cheese.
