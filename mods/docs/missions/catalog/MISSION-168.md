# Mission 168 — Double Doublecross

| Field | Value |
|-------|-------|
| Mission ID | 168 |
| Mission type | Guide |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1714 | Talk | — | — | 1715 | — | 731 | 728 |
| 1715 | Talk | — | — | — | — | — | 744 |

## Chain edges (from table)

- Success: **1714** → **1715**

## Task 1714

**Tags:** chains-to=1715

### How this task is received

- Player accepts from NPC table ID **731** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **143**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **731** (journal accept or auto-chain).
- Complete at terminator NPC table ID **728** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1714
- `iNPC_ID` = runtime ID from grant NPC 731
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1714
- `iNPC_ID` = runtime ID from terminator NPC 728
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Return to Mandy at Devil's Bluff.

## Task 1715

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **143**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **744** (proximity/talk).
- Reward table ID **395** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1715
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1715
- `iNPC_ID` = runtime ID from terminator NPC 744
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Return to Mandy at Devil's Bluff.
