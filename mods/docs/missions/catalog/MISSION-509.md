# Mission 509 — Gum Drop

| Field | Value |
|-------|-------|
| Mission ID | 509 |
| Mission type | Normal |
| Task count | 7 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 493 | Talk | — | — | 496 | — | 3273 | 768 |
| 494 | Talk | — | — | 495 | — | — | 774 |
| 495 | Talk | — | — | 496 | — | — | 795 |
| 496 | Defeat | — | — | 499 | — | — | — |
| 497 | Delivery | — | — | 498 | — | — | 774 |
| 498 | Delivery | — | — | 499 | — | — | 795 |
| 499 | Delivery | — | — | — | — | — | 768 |

## Chain edges (from table)

- Success: **493** → **496**
- Success: **494** → **495**
- Success: **495** → **496**
- Success: **496** → **499**
- Success: **497** → **498**
- Success: **498** → **499**

## Task 493

**Tags:** chains-to=496

### How this task is received

- Player accepts from NPC table ID **3273** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3273** (journal accept or auto-chain).
- Complete at terminator NPC table ID **768** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 493
- `iNPC_ID` = runtime ID from grant NPC 3273
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 493
- `iNPC_ID` = runtime ID from terminator NPC 768
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.

## Task 494

**Tags:** chains-to=495

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **774** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 494
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 494
- `iNPC_ID` = runtime ID from terminator NPC 774
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.

## Task 495

**Tags:** chains-to=496

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **795** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 495
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 495
- `iNPC_ID` = runtime ID from terminator NPC 795
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.

## Task 496

**Tags:** kill-quota, item-quota, chains-to=499

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 83 x0
- Collect items: item 137 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 496
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 496
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.

## Task 497

**Tags:** item-quota, chains-to=498

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **774** (proximity/talk).
- Collect items: item 137 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 497
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 497
- `iNPC_ID` = runtime ID from terminator NPC 774
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.

## Task 498

**Tags:** item-quota, chains-to=499

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **795** (proximity/talk).
- Collect items: item 138 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 498
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 498
- `iNPC_ID` = runtime ID from terminator NPC 795
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.

## Task 499

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **768** (proximity/talk).
- Reward table ID **170** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 499
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 499
- `iNPC_ID` = runtime ID from terminator NPC 768
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Five get more gumball ammo.
