# Mission 282 — Favors for Father (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 282 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 904 | Defeat | — | — | 905 | — | 709 | — |
| 905 | Delivery | — | — | — | — | — | 709 |

## Chain edges (from table)

- Success: **904** → **905**

## Task 904

**Tags:** kill-quota, chains-to=905

### How this task is received

- Player accepts from NPC table ID **709** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **281**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **709** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 300 x6

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 904
- `iNPC_ID` = runtime ID from grant NPC 709
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 904
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop the Maelstrom Creepers.

## Task 905

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **281**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **709** (proximity/talk).
- Collect items: item 237 x1
- Reward table ID **208** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 905
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 905
- `iNPC_ID` = runtime ID from terminator NPC 709
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop the Maelstrom Creepers.
