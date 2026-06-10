# Mission 76 — Fun and Games, Part One

| Field | Value |
|-------|-------|
| Mission ID | 76 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 302 | GotoLocation | — | — | 303 | — | 726 | 1599 |
| 303 | Defeat | — | — | 304 | — | — | — |
| 304 | Delivery | — | — | — | — | — | 726 |

## Chain edges (from table)

- Success: **302** → **303**
- Success: **303** → **304**

## Task 302

**Tags:** chains-to=303

### How this task is received

- Player accepts from NPC table ID **726** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **153**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **726** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1599** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 302
- `iNPC_ID` = runtime ID from grant NPC 726
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 302
- `iNPC_ID` = runtime ID from terminator NPC 1599
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Bloo's game from the auditorium.

## Task 303

**Tags:** kill-quota, item-quota, chains-to=304

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **153**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 338 x0
- Collect items: item 92 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 303
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 303
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Bloo's game from the auditorium.

## Task 304

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **153**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **726** (proximity/talk).
- Collect items: item 92 x1
- Reward table ID **326** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 304
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 304
- `iNPC_ID` = runtime ID from terminator NPC 726
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Bloo's game from the auditorium.
