# Mission 22 — Mutant Eggplants Attack! (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 22 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 95 | GotoLocation | — | — | 96 | — | 765 | 2721 |
| 96 | Defeat | — | — | 97 | — | — | — |
| 97 | Talk | — | — | 99 | — | — | 787 |
| 99 | Talk | — | — | 100 | — | — | 788 |
| 100 | Talk | — | — | — | — | — | 765 |

## Chain edges (from table)

- Success: **95** → **96**
- Success: **96** → **97**
- Success: **97** → **99**
- Success: **99** → **100**

## Task 95

**Tags:** chains-to=96

### How this task is received

- Player accepts from NPC table ID **765** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **765** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2721** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 95
- `iNPC_ID` = runtime ID from grant NPC 765
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 95
- `iNPC_ID` = runtime ID from terminator NPC 2721
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Figure out who is attacking the Chickens From Outer Space.

## Task 96

**Tags:** kill-quota, item-quota, chains-to=97

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 437 x0
- Collect items: item 46 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 96
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 96
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Figure out who is attacking the Chickens From Outer Space.

## Task 97

**Tags:** chains-to=99

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **787** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 97
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 97
- `iNPC_ID` = runtime ID from terminator NPC 787
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Figure out who is attacking the Chickens From Outer Space.

## Task 99

**Tags:** chains-to=100

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **788** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 99
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 99
- `iNPC_ID` = runtime ID from terminator NPC 788
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Figure out who is attacking the Chickens From Outer Space.

## Task 100

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **765** (proximity/talk).
- Reward table ID **24** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 100
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 100
- `iNPC_ID` = runtime ID from terminator NPC 765
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Figure out who is attacking the Chickens From Outer Space.
