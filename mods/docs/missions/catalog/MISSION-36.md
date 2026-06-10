# Mission 36 — Max Marathon

| Field | Value |
|-------|-------|
| Mission ID | 36 |
| Mission type | Guide |
| Task count | 1 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 18 | Talk | — | — | — | — | 785 | 725 |

## Chain edges (from table)

- (no outgoing edges in table)

## Task 18

**Tags:** standard

### How this task is received

- Player accepts from NPC table ID **785** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **35**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **785** (journal accept or auto-chain).
- Complete at terminator NPC table ID **725** (proximity/talk).
- Reward table ID **36** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 18
- `iNPC_ID` = runtime ID from grant NPC 785
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 18
- `iNPC_ID` = runtime ID from terminator NPC 725
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver a TRACKAMABOB to Grandpa Max.
