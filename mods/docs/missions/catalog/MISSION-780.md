# Mission 780 — Meetin' Cheese (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 780 |
| Mission type | Normal |
| Task count | 1 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2614 | UseItems | — | — | — | — | 3221 | 3121 |

## Chain edges (from table)

- (no outgoing edges in table)

## Task 2614

**Tags:** standard

### How this task is received

- Player accepts from NPC table ID **3221** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **779**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **3221** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3121** (proximity/talk).
- Reward table ID **660** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2614
- `iNPC_ID` = runtime ID from grant NPC 3221
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2614
- `iNPC_ID` = runtime ID from terminator NPC 3121
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out why Cheese is here.
