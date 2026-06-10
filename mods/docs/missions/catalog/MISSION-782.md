# Mission 782 — On the Precipice
(Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 782 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2643 | Talk | — | — | 2644 | — | 3229 | 3228 |
| 2644 | Defeat | — | — | 2645 | — | — | — |
| 2645 | Talk | — | — | — | — | — | 3228 |

## Chain edges (from table)

- Success: **2643** → **2644**
- Success: **2644** → **2645**

## Task 2643

**Tags:** chains-to=2644

### How this task is received

- Player accepts from NPC table ID **3229** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **781**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3229** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3228** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2643
- `iNPC_ID` = runtime ID from grant NPC 3229
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2643
- `iNPC_ID` = runtime ID from terminator NPC 3228
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out more about the Meteorite Sample.

## Task 2644

**Tags:** kill-quota, chains-to=2645

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **781**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 1989 x8

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2644
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2644
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out more about the Meteorite Sample.

## Task 2645

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **781**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3228** (proximity/talk).
- Reward table ID **668** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2645
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2645
- `iNPC_ID` = runtime ID from terminator NPC 3228
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find out more about the Meteorite Sample.
