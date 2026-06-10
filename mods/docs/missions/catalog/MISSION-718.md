# Mission 718 — Cool Drink Stink (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 718 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2516 | Defeat | — | — | 2517 | — | 741 | — |
| 2517 | Talk | — | — | — | — | — | 741 |

## Chain edges (from table)

- Success: **2516** → **2517**

## Task 2516

**Tags:** kill-quota, chains-to=2517

### How this task is received

- Player accepts from NPC table ID **741** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **717**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **741** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 389 x9

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2516
- `iNPC_ID` = runtime ID from grant NPC 741
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2516
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fuse from poisoning the river.

## Task 2517

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **717**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **741** (proximity/talk).
- Reward table ID **636** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2517
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2517
- `iNPC_ID` = runtime ID from terminator NPC 741
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fuse from poisoning the river.
