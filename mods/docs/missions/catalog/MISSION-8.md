# Mission 8 — Hangin' with the Coopster (Part 1 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 8 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 28 | UseItems | — | — | 29 | — | 718 | 808 |
| 29 | Delivery | — | — | — | — | — | 718 |

## Chain edges (from table)

- Success: **28** → **29**

## Task 28

**Tags:** chains-to=29

### How this task is received

- Player accepts from NPC table ID **718** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **718** (journal accept or auto-chain).
- Complete at terminator NPC table ID **808** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 28
- `iNPC_ID` = runtime ID from grant NPC 718
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 28
- `iNPC_ID` = runtime ID from terminator NPC 808
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a video game that Coop forgot to return to the rental store.

## Task 29

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **718** (proximity/talk).
- Collect items: item 30 x1
- Reward table ID **9** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 29
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 29
- `iNPC_ID` = runtime ID from terminator NPC 718
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a video game that Coop forgot to return to the rental store.
