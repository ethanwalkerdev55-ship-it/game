# Mission 638 — Going Berserk (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 638 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2373 | Defeat | — | — | 2374 | — | 752 | — |
| 2374 | Talk | — | — | — | — | — | 752 |

## Chain edges (from table)

- Success: **2373** → **2374**

## Task 2373

**Tags:** kill-quota, chains-to=2374

### How this task is received

- Player accepts from NPC table ID **752** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **637**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **752** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2078 x8

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2373
- `iNPC_ID` = runtime ID from grant NPC 752
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2373
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop the Horrordactyls.

## Task 2374

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **637**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **752** (proximity/talk).
- Reward table ID **581** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2374
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2374
- `iNPC_ID` = runtime ID from terminator NPC 752
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop the Horrordactyls.
