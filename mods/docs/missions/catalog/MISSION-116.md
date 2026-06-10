# Mission 116 — Into the Wild

| Field | Value |
|-------|-------|
| Mission ID | 116 |
| Mission type | Guide |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1623 | Defeat | — | — | 1624 | — | 747 | — |
| 1624 | Delivery | — | — | — | — | — | 747 |

## Chain edges (from table)

- Success: **1623** → **1624**

## Task 1623

**Tags:** kill-quota, item-quota, chains-to=1624

### How this task is received

- Player accepts from NPC table ID **747** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **45**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **747** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 415 x0
- Collect items: item 418 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1623
- `iNPC_ID` = runtime ID from grant NPC 747
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1623
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Prove your worth to Hoss Delgado.

## Task 1624

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **45**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **747** (proximity/talk).
- Collect items: item 418 x1
- Reward table ID **366** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1624
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1624
- `iNPC_ID` = runtime ID from terminator NPC 747
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Prove your worth to Hoss Delgado.
