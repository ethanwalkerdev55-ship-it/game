# Mission 480 — Toiletnator's Tomfoolery (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 480 |
| Mission type | Normal |
| Task count | 1 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2283 | Talk | — | — | — | — | 740 | 727 |

## Chain edges (from table)

- (no outgoing edges in table)

## Task 2283

**Tags:** standard

### How this task is received

- Player accepts from NPC table ID **740** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **479**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **740** (journal accept or auto-chain).
- Complete at terminator NPC table ID **727** (proximity/talk).
- Reward table ID **541** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2283
- `iNPC_ID` = runtime ID from grant NPC 740
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2283
- `iNPC_ID` = runtime ID from terminator NPC 727
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Bring the baby monkeys to Bubbles.
