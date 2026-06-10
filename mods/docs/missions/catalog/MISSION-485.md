# Mission 485 — Teen Dream

| Field | Value |
|-------|-------|
| Mission ID | 485 |
| Mission type | Normal |
| Task count | 1 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2451 | Talk | — | — | — | — | 721 | 725 |

## Chain edges (from table)

- (no outgoing edges in table)

## Task 2451

**Tags:** standard

### How this task is received

- Player accepts from NPC table ID **721** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **721** (journal accept or auto-chain).
- Complete at terminator NPC table ID **725** (proximity/talk).
- Reward table ID **616** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2451
- `iNPC_ID` = runtime ID from grant NPC 721
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2451
- `iNPC_ID` = runtime ID from terminator NPC 725
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Bring Princess' headshot to Max.
