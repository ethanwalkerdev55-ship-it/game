# Mission 257 — Blossom's Picnic Panic (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 257 |
| Mission type | Normal |
| Task count | 1 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 739 | Talk | — | — | — | — | 703 | 702 |

## Chain edges (from table)

- (no outgoing edges in table)

## Task 739

**Tags:** standard

### How this task is received

- Player accepts from NPC table ID **703** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **256**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **703** (journal accept or auto-chain).
- Complete at terminator NPC table ID **702** (proximity/talk).
- Reward table ID **175** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 739
- `iNPC_ID` = runtime ID from grant NPC 703
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 739
- `iNPC_ID` = runtime ID from terminator NPC 702
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out why the Professor missed the picnic.
