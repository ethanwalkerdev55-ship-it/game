# Mission 133 — Shell Game

| Field | Value |
|-------|-------|
| Mission ID | 133 |
| Mission type | Guide |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1689 | Defeat | — | — | 1690 | — | 752 | — |
| 1690 | Delivery | — | — | — | — | — | 752 |

## Chain edges (from table)

- Success: **1689** → **1690**

## Task 1689

**Tags:** kill-quota, item-quota, chains-to=1690

### How this task is received

- Player accepts from NPC table ID **752** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **132**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **752** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2063 x0
- Collect items: item 435 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1689
- `iNPC_ID` = runtime ID from grant NPC 752
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1689
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the shell of a Great Shellworm.

## Task 1690

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **132**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **752** (proximity/talk).
- Collect items: item 435 x1
- Reward table ID **387** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1690
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1690
- `iNPC_ID` = runtime ID from terminator NPC 752
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the shell of a Great Shellworm.
