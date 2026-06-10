# Mission 137 — Post Hypnotic Infection

| Field | Value |
|-------|-------|
| Mission ID | 137 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1703 | GotoLocation | — | — | 1704 | — | 744 | 1847 |
| 1704 | Defeat | — | — | 1705 | — | — | — |
| 1705 | Delivery | — | — | — | — | — | 744 |

## Chain edges (from table)

- Success: **1703** → **1704**
- Success: **1704** → **1705**

## Task 1703

**Tags:** chains-to=1704

### How this task is received

- Player accepts from NPC table ID **744** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **139**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **744** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1847** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1703
- `iNPC_ID` = runtime ID from grant NPC 744
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1703
- `iNPC_ID` = runtime ID from terminator NPC 1847
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect a charm from the Monkey Foothills.

## Task 1704

**Tags:** kill-quota, item-quota, chains-to=1705

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **139**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 232 x0
- Collect items: item 439 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1704
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1704
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect a charm from the Monkey Foothills.

## Task 1705

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **139**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **744** (proximity/talk).
- Reward table ID **392** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1705
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1705
- `iNPC_ID` = runtime ID from terminator NPC 744
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect a charm from the Monkey Foothills.
