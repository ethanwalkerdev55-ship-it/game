# Mission 258 — Blossom's Picnic Panic (Part 3 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 258 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 740 | Defeat | — | — | 741 | — | 702 | — |
| 741 | Delivery | — | — | — | — | — | 702 |

## Chain edges (from table)

- Success: **740** → **741**

## Task 740

**Tags:** kill-quota, item-quota, chains-to=741

### How this task is received

- Player accepts from NPC table ID **702** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **257**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **702** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 286 x0
- Collect items: item 205 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 740
- `iNPC_ID` = runtime ID from grant NPC 702
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 740
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Prof. Utonium repair the lab.

## Task 741

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **257**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **702** (proximity/talk).
- Collect items: item 205 x3
- Reward table ID **176** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 741
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 741
- `iNPC_ID` = runtime ID from terminator NPC 702
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Prof. Utonium repair the lab.
