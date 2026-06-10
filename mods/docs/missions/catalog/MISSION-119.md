# Mission 119 — Bash Your Brains

| Field | Value |
|-------|-------|
| Mission ID | 119 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1643 | GotoLocation | — | — | 1644 | — | 747 | 2022 |
| 1644 | Defeat | — | — | 1645 | — | — | — |
| 1645 | Delivery | — | — | — | — | — | 747 |

## Chain edges (from table)

- Success: **1643** → **1644**
- Success: **1644** → **1645**

## Task 1643

**Tags:** chains-to=1644

### How this task is received

- Player accepts from NPC table ID **747** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **117**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **747** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2022** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1643
- `iNPC_ID` = runtime ID from grant NPC 747
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1643
- `iNPC_ID` = runtime ID from terminator NPC 2022
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the brains of a Skull Basher.

## Task 1644

**Tags:** kill-quota, item-quota, chains-to=1645

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **117**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2054 x0
- Collect items: item 422 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1644
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1644
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the brains of a Skull Basher.

## Task 1645

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **117**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **747** (proximity/talk).
- Collect items: item 422 x1
- Reward table ID **372** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1645
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1645
- `iNPC_ID` = runtime ID from terminator NPC 747
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the brains of a Skull Basher.
