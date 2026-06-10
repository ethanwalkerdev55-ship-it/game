# Mission 477 — Toiletnator's Tomfoolery (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 477 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2273 | UseItems | — | — | 2274 | — | 736 | 2802 |
| 2274 | Talk | — | — | 2275 | — | — | 736 |
| 2275 | Talk | — | — | — | — | — | 741 |

## Chain edges (from table)

- Success: **2273** → **2274**
- Success: **2274** → **2275**

## Task 2273

**Tags:** chains-to=2274

### How this task is received

- Player accepts from NPC table ID **736** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **476**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **736** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2802** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2273
- `iNPC_ID` = runtime ID from grant NPC 736
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2273
- `iNPC_ID` = runtime ID from terminator NPC 2802
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Do a favor for Toiletnator.

## Task 2274

**Tags:** chains-to=2275

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **476**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **736** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2274
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2274
- `iNPC_ID` = runtime ID from terminator NPC 736
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Do a favor for Toiletnator.

## Task 2275

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **476**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **741** (proximity/talk).
- Reward table ID **538** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2275
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2275
- `iNPC_ID` = runtime ID from terminator NPC 741
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Do a favor for Toiletnator.
