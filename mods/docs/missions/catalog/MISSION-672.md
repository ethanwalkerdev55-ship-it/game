# Mission 672 — Building a Better Monster (Part 1 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 672 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2427 | Defeat | — | — | 2428 | — | 731 | — |
| 2428 | Talk | — | — | — | — | — | 731 |

## Chain edges (from table)

- Success: **2427** → **2428**

## Task 2427

**Tags:** kill-quota, chains-to=2428

### How this task is received

- Player accepts from NPC table ID **731** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **731** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 433 x12

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2427
- `iNPC_ID` = runtime ID from grant NPC 731
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2427
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recapture Monkey Mountain.

## Task 2428

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **731** (proximity/talk).
- Reward table ID **605** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2428
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2428
- `iNPC_ID` = runtime ID from terminator NPC 731
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recapture Monkey Mountain.
