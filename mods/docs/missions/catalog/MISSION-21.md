# Mission 21 — Mutant Eggplants Attack! (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 21 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 89 | Delivery | — | — | 91 | 89 | 765 | 766 |
| 91 | Defeat | — | — | 93 | 91 | — | — |
| 93 | Defeat | — | — | 94 | — | — | — |
| 94 | Delivery | — | — | 139 | 94 | — | 766 |
| 139 | Talk | — | — | — | — | — | 765 |

## Chain edges (from table)

- Success: **89** → **91**
- Fail (err 1/12): **89** → **89**
- Success: **91** → **93**
- Fail (err 1/12): **91** → **91**
- Success: **93** → **94**
- Success: **94** → **139**
- Fail (err 1/12): **94** → **94**

## Task 89

**Tags:** item-quota, chains-to=91, fail-restart=89

### How this task is received

- Player accepts from NPC table ID **765** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **765** (journal accept or auto-chain).
- Complete at terminator NPC table ID **766** (proximity/talk).
- Collect items: item 43 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 89
- `iNPC_ID` = runtime ID from grant NPC 765
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 89
- `iNPC_ID` = runtime ID from terminator NPC 766
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help the eggplants deliver a peace offering.

## Task 91

**Tags:** kill-quota, item-quota, chains-to=93, fail-restart=91

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 436 x0
- Collect items: item 44 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 91
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 91
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help the eggplants deliver a peace offering.

## Task 93

**Tags:** kill-quota, item-quota, chains-to=94

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 436 x0
- Collect items: item 45 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 93
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 93
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help the eggplants deliver a peace offering.

## Task 94

**Tags:** item-quota, chains-to=139, fail-restart=94

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **766** (proximity/talk).
- Collect items: item 43 x1, item 44 x1, item 45 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 94
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 94
- `iNPC_ID` = runtime ID from terminator NPC 766
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help the eggplants deliver a peace offering.

## Task 139

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **765** (proximity/talk).
- Reward table ID **23** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 139
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 139
- `iNPC_ID` = runtime ID from terminator NPC 765
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help the eggplants deliver a peace offering.
