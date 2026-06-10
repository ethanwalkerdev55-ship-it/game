# Mission 381 — Fusion Pirate's Pillage (Part 4 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 381 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2022 | Defeat | — | — | 2023 | — | 748 | — |
| 2023 | Defeat | — | — | 2024 | — | — | — |
| 2024 | Delivery | — | — | — | — | — | 748 |

## Chain edges (from table)

- Success: **2022** → **2023**
- Success: **2023** → **2024**

## Task 2022

**Tags:** kill-quota, chains-to=2023

### How this task is received

- Player accepts from NPC table ID **748** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **380**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **748** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 225 x10

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2022
- `iNPC_ID` = runtime ID from grant NPC 748
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2022
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Power up the Mummified Amulet.

## Task 2023

**Tags:** kill-quota, chains-to=2024

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **380**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 418 x12

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2023
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2023
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Power up the Mummified Amulet.

## Task 2024

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **380**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **748** (proximity/talk).
- Collect items: item 510 x1
- Reward table ID **464** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2024
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2024
- `iNPC_ID` = runtime ID from terminator NPC 748
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Power up the Mummified Amulet.
