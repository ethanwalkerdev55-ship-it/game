# Mission 314 — How to Date a Fusion (Part 3 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 314 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1289 | Defeat | — | — | 1290 | — | 720 | — |
| 1290 | Delivery | — | — | — | — | — | 720 |

## Chain edges (from table)

- Success: **1289** → **1290**

## Task 1289

**Tags:** kill-quota, item-quota, chains-to=1290

### How this task is received

- Player accepts from NPC table ID **720** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **313**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **720** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 339 x0
- Collect items: item 330 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1289
- `iNPC_ID` = runtime ID from grant NPC 720
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1289
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Ace's guitar from the monsters.

## Task 1290

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **313**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **720** (proximity/talk).
- Collect items: item 330 x1
- Reward table ID **289** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1290
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1290
- `iNPC_ID` = runtime ID from terminator NPC 720
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Ace's guitar from the monsters.
