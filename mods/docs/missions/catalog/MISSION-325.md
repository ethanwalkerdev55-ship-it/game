# Mission 325 — Fusion by the Sea (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 325 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1324 | UseItems | — | — | 1325 | — | 727 | 1691 |
| 1325 | Delivery | — | — | — | — | — | 727 |

## Chain edges (from table)

- Success: **1324** → **1325**

## Task 1324

**Tags:** chains-to=1325

### How this task is received

- Player accepts from NPC table ID **727** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **324**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **727** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1691** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1324
- `iNPC_ID` = runtime ID from grant NPC 727
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1324
- `iNPC_ID` = runtime ID from terminator NPC 1691
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the other half of the chalkboard.

## Task 1325

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **324**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **727** (proximity/talk).
- Collect items: item 339 x1
- Reward table ID **300** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1325
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1325
- `iNPC_ID` = runtime ID from terminator NPC 727
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the other half of the chalkboard.
