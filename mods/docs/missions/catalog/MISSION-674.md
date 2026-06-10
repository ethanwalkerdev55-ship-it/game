# Mission 674 — Building a Better Monster (Part 3 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 674 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2431 | Defeat | — | — | 2432 | — | 750 | — |
| 2432 | Talk | — | — | — | — | — | 750 |

## Chain edges (from table)

- Success: **2431** → **2432**

## Task 2431

**Tags:** kill-quota, item-quota, chains-to=2432

### How this task is received

- Player accepts from NPC table ID **750** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **673**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **750** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 1986 x0
- Collect items: item 549 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2431
- `iNPC_ID` = runtime ID from grant NPC 750
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2431
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Creeposaurus Rexes.

## Task 2432

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **673**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **750** (proximity/talk).
- Reward table ID **607** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2432
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2432
- `iNPC_ID` = runtime ID from terminator NPC 750
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Creeposaurus Rexes.
