# Mission 380 — Fusion Pirate's Pillage (Part 3 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 380 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2019 | GotoLocation | — | — | 2020 | — | 748 | 2272 |
| 2020 | Defeat | — | — | 2021 | — | — | — |
| 2021 | Delivery | — | — | — | — | — | 748 |

## Chain edges (from table)

- Success: **2019** → **2020**
- Success: **2020** → **2021**

## Task 2019

**Tags:** chains-to=2020

### How this task is received

- Player accepts from NPC table ID **748** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **379**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **748** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2272** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2019
- `iNPC_ID` = runtime ID from grant NPC 748
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2019
- `iNPC_ID` = runtime ID from terminator NPC 2272
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Track the stolen Nano items.

## Task 2020

**Tags:** kill-quota, item-quota, chains-to=2021

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **379**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 417 x0
- Collect items: item 509 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2020
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2020
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Track the stolen Nano items.

## Task 2021

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **379**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **748** (proximity/talk).
- Collect items: item 509 x1
- Reward table ID **463** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2021
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2021
- `iNPC_ID` = runtime ID from terminator NPC 748
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Track the stolen Nano items.
