# Mission 25 — Chickens from Outer Space! (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 25 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 110 | Defeat | — | — | 111 | 110 | 766 | — |
| 111 | GotoLocation | — | — | 112 | 111 | — | 2028 |
| 112 | Defeat | — | — | 113 | 111 | — | — |
| 113 | Talk | — | — | 114 | 111 | — | 1912 |
| 114 | Talk | — | — | — | 114 | — | 766 |

## Chain edges (from table)

- Success: **110** → **111**
- Fail (err 1/12): **110** → **110**
- Success: **111** → **112**
- Fail (err 1/12): **111** → **111**
- Success: **112** → **113**
- Fail (err 1/12): **112** → **111**
- Success: **113** → **114**
- Fail (err 1/12): **113** → **111**
- Fail (err 1/12): **114** → **114**

## Task 110

**Tags:** kill-quota, item-quota, chains-to=111, fail-restart=110

### How this task is received

- Player accepts from NPC table ID **766** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **766** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 430 x0
- Collect items: item 48 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 110
- `iNPC_ID` = runtime ID from grant NPC 766
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 110
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Rescue the Chickens From Outer Space.

## Task 111

**Tags:** chains-to=112, fail-restart=111

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2028** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 111
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 111
- `iNPC_ID` = runtime ID from terminator NPC 2028
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Rescue the Chickens From Outer Space.

## Task 112

**Tags:** kill-quota, chains-to=113, fail-restart=111

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 241 x8

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 112
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 112
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Rescue the Chickens From Outer Space.

## Task 113

**Tags:** chains-to=114, fail-restart=111

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1912** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 113
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 113
- `iNPC_ID` = runtime ID from terminator NPC 1912
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Rescue the Chickens From Outer Space.

## Task 114

**Tags:** fail-restart=114

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **22**.
- Prerequisite completed mission **24**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **766** (proximity/talk).
- Reward table ID **27** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 114
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 114
- `iNPC_ID` = runtime ID from terminator NPC 766
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Rescue the Chickens From Outer Space.
