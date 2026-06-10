# Mission 732 — Macaw of the Wild (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 732 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2539 | Talk | — | — | 2540 | — | 3136 | 3135 |
| 2540 | UseItems | — | — | 2541 | — | — | 3138 |
| 2541 | Defeat | — | — | 2542 | — | — | — |
| 2542 | Talk | — | — | 2543 | — | — | 2806 |
| 2543 | Talk | — | — | — | — | — | 3135 |

## Chain edges (from table)

- Success: **2539** → **2540**
- Success: **2540** → **2541**
- Success: **2541** → **2542**
- Success: **2542** → **2543**

## Task 2539

**Tags:** chains-to=2540

### How this task is received

- Player accepts from NPC table ID **3136** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **731**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3136** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3135** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2539
- `iNPC_ID` = runtime ID from grant NPC 3136
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2539
- `iNPC_ID` = runtime ID from terminator NPC 3135
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Jordan Almond to make an Eye Patch.

## Task 2540

**Tags:** chains-to=2541

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **731**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3138** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2540
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2540
- `iNPC_ID` = runtime ID from terminator NPC 3138
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Jordan Almond to make an Eye Patch.

## Task 2541

**Tags:** kill-quota, item-quota, chains-to=2542

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **731**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 432 x0
- Collect items: item 555 x2

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2541
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2541
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Jordan Almond to make an Eye Patch.

## Task 2542

**Tags:** chains-to=2543

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **731**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2806** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2542
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2542
- `iNPC_ID` = runtime ID from terminator NPC 2806
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Jordan Almond to make an Eye Patch.

## Task 2543

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **731**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3135** (proximity/talk).
- Reward table ID **643** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2543
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2543
- `iNPC_ID` = runtime ID from terminator NPC 3135
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Jordan Almond to make an Eye Patch.
