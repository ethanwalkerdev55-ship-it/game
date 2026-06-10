# Mission 278 — Of Fathers and Fusions (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 278 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 894 | UseItems | — | — | 895 | — | 709 | 1390 |
| 895 | Talk | — | — | 896 | — | — | 702 |
| 896 | Talk | — | — | — | — | — | 709 |

## Chain edges (from table)

- Success: **894** → **895**
- Success: **895** → **896**

## Task 894

**Tags:** chains-to=895

### How this task is received

- Player accepts from NPC table ID **709** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **277**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **709** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1390** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 894
- `iNPC_ID` = runtime ID from grant NPC 709
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 894
- `iNPC_ID` = runtime ID from terminator NPC 1390
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Mandark's Yard.

## Task 895

**Tags:** chains-to=896

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **277**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **702** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 895
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 895
- `iNPC_ID` = runtime ID from terminator NPC 702
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Mandark's Yard.

## Task 896

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **277**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **709** (proximity/talk).
- Reward table ID **205** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 896
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 896
- `iNPC_ID` = runtime ID from terminator NPC 709
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Mandark's Yard.
