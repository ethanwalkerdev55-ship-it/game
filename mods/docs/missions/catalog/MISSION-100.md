# Mission 100 — Beach Boom

| Field | Value |
|-------|-------|
| Mission ID | 100 |
| Mission type | Guide |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 418 | GotoLocation | — | — | 419 | — | 2562 | 906 |
| 419 | Defeat | — | — | 420 | — | — | — |
| 420 | Delivery | — | — | 421 | — | — | 791 |
| 421 | Delivery | — | — | — | — | — | 2562 |

## Chain edges (from table)

- Success: **418** → **419**
- Success: **419** → **420**
- Success: **420** → **421**

## Task 418

**Tags:** chains-to=419

### How this task is received

- Player accepts from NPC table ID **2562** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **99**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **2562** (journal accept or auto-chain).
- Complete at terminator NPC table ID **906** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 418
- `iNPC_ID` = runtime ID from grant NPC 2562
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 418
- `iNPC_ID` = runtime ID from terminator NPC 906
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover more Fusion Matter for Mandark at the beach.

## Task 419

**Tags:** kill-quota, item-quota, chains-to=420

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **99**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 365 x0
- Collect items: item 125 x2

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 419
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 419
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover more Fusion Matter for Mandark at the beach.

## Task 420

**Tags:** item-quota, chains-to=421

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **99**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **791** (proximity/talk).
- Collect items: item 125 x2

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 420
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 420
- `iNPC_ID` = runtime ID from terminator NPC 791
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover more Fusion Matter for Mandark at the beach.

## Task 421

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **99**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2562** (proximity/talk).
- Collect items: item 125 x1
- Reward table ID **100** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 421
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 421
- `iNPC_ID` = runtime ID from terminator NPC 2562
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover more Fusion Matter for Mandark at the beach.
