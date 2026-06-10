# Mission 17 — Lee Kanker Likes You! (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 17 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 74 | Delivery | — | — | 75 | — | 712 | 735 |
| 75 | UseItems | — | — | 76 | — | — | 817 |
| 76 | Defeat | — | — | 77 | — | — | — |
| 77 | UseItems | — | — | — | — | — | 735 |

## Chain edges (from table)

- Success: **74** → **75**
- Success: **75** → **76**
- Success: **76** → **77**

## Task 74

**Tags:** item-quota, chains-to=75

### How this task is received

- Player accepts from NPC table ID **712** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **15**.

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **712** (journal accept or auto-chain).
- Complete at terminator NPC table ID **735** (proximity/talk).
- Collect items: item 39 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 74
- `iNPC_ID` = runtime ID from grant NPC 712
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 74
- `iNPC_ID` = runtime ID from terminator NPC 735
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Lee Kanker wants me to deliver a note to her sister, May.

## Task 75

**Tags:** chains-to=76

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **15**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **817** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 75
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 75
- `iNPC_ID` = runtime ID from terminator NPC 817
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Lee Kanker wants me to deliver a note to her sister, May.

## Task 76

**Tags:** kill-quota, chains-to=77

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **15**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 326 x6

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 76
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 76
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Lee Kanker wants me to deliver a note to her sister, May.

## Task 77

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **15**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **735** (proximity/talk).
- Reward table ID **20** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 77
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 77
- `iNPC_ID` = runtime ID from terminator NPC 735
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Lee Kanker wants me to deliver a note to her sister, May.
