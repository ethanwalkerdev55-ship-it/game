# Mission 623 — The Power of Prophecy (Part 3 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 623 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2347 | Talk | — | — | 2348 | — | 748 | 1910 |
| 2348 | UseItems | — | — | — | — | — | 2835 |

## Chain edges (from table)

- Success: **2347** → **2348**

## Task 2347

**Tags:** chains-to=2348

### How this task is received

- Player accepts from NPC table ID **748** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **622**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **748** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1910** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2347
- `iNPC_ID` = runtime ID from grant NPC 748
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2347
- `iNPC_ID` = runtime ID from terminator NPC 1910
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Awaken the totems.

## Task 2348

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **622**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2835** (proximity/talk).
- Reward table ID **569** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2348
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2348
- `iNPC_ID` = runtime ID from terminator NPC 2835
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Awaken the totems.
