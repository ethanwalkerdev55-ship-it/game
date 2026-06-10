# Mission 557 — Comic Capers

| Field | Value |
|-------|-------|
| Mission ID | 557 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1737 | GotoLocation | — | — | 1738 | — | 3270 | 1093 |
| 1738 | Defeat | — | — | 1739 | — | — | — |
| 1739 | Delivery | — | — | — | — | — | 774 |

## Chain edges (from table)

- Success: **1737** → **1738**
- Success: **1738** → **1739**

## Task 1737

**Tags:** chains-to=1738

### How this task is received

- Player accepts from NPC table ID **3270** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **506**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **3270** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1093** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1737
- `iNPC_ID` = runtime ID from grant NPC 3270
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1737
- `iNPC_ID` = runtime ID from terminator NPC 1093
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Eddy with a recovery mission.

## Task 1738

**Tags:** kill-quota, item-quota, chains-to=1739

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **506**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 264 x0
- Collect items: item 449 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1738
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1738
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Eddy with a recovery mission.

## Task 1739

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **506**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **774** (proximity/talk).
- Reward table ID **403** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1739
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1739
- `iNPC_ID` = runtime ID from terminator NPC 774
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Eddy with a recovery mission.
