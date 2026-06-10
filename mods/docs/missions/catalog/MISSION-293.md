# Mission 293 — Imaginary Reinforcements (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 293 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 941 | UseItems | — | — | 942 | — | 715 | 1436 |
| 942 | UseItems | — | — | 943 | — | — | 1437 |
| 943 | UseItems | — | — | 944 | — | — | 1438 |
| 944 | Delivery | — | — | — | — | — | 708 |

## Chain edges (from table)

- Success: **941** → **942**
- Success: **942** → **943**
- Success: **943** → **944**

## Task 941

**Tags:** chains-to=942

### How this task is received

- Player accepts from NPC table ID **715** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **292**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **715** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1436** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 941
- `iNPC_ID` = runtime ID from grant NPC 715
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 941
- `iNPC_ID` = runtime ID from terminator NPC 1436
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the stolen toothpaste.

## Task 942

**Tags:** chains-to=943

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **292**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1437** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 942
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 942
- `iNPC_ID` = runtime ID from terminator NPC 1437
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the stolen toothpaste.

## Task 943

**Tags:** chains-to=944

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **292**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1438** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 943
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 943
- `iNPC_ID` = runtime ID from terminator NPC 1438
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the stolen toothpaste.

## Task 944

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **292**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **708** (proximity/talk).
- Collect items: item 247 x3
- Reward table ID **218** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 944
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 944
- `iNPC_ID` = runtime ID from terminator NPC 708
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the stolen toothpaste.
