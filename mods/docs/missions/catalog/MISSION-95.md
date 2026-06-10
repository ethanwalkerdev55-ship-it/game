# Mission 95 — Monkey Business

| Field | Value |
|-------|-------|
| Mission ID | 95 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 395 | GotoLocation | — | — | 396 | — | 709 | 898 |
| 396 | Defeat | — | — | 397 | — | — | — |
| 397 | Delivery | — | — | — | — | — | 2561 |

## Chain edges (from table)

- Success: **395** → **396**
- Success: **396** → **397**

## Task 395

**Tags:** chains-to=396

### How this task is received

- Player accepts from NPC table ID **709** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **94**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **709** (journal accept or auto-chain).
- Complete at terminator NPC table ID **898** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 395
- `iNPC_ID` = runtime ID from grant NPC 709
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 395
- `iNPC_ID` = runtime ID from terminator NPC 898
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect monkey DNA from Charles Darwin.

## Task 396

**Tags:** kill-quota, item-quota, chains-to=397

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **94**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 134 x0
- Collect items: item 120 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 396
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 396
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect monkey DNA from Charles Darwin.

## Task 397

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **94**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2561** (proximity/talk).
- Collect items: item 116 x1, item 120 x1
- Reward table ID **95** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 397
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 397
- `iNPC_ID` = runtime ID from terminator NPC 2561
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect monkey DNA from Charles Darwin.
