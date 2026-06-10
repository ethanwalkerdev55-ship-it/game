# Mission 24 — Chickens from Outer Space! (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 24 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 106 | GotoLocation | — | — | 107 | — | 766 | 2017 |
| 107 | Defeat | — | — | 108 | — | — | — |
| 108 | Talk | — | — | — | — | — | 766 |

## Chain edges (from table)

- Success: **106** → **107**
- Success: **107** → **108**

## Task 106

**Tags:** chains-to=107

### How this task is received

- Player accepts from NPC table ID **766** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **766** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2017** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 106
- `iNPC_ID` = runtime ID from grant NPC 766
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 106
- `iNPC_ID` = runtime ID from terminator NPC 2017
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the fake Chickens From Outer Space.

## Task 107

**Tags:** kill-quota, chains-to=108

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 430 x7

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 107
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 107
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the fake Chickens From Outer Space.

## Task 108

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **20**.
- Prerequisite completed mission **23**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **766** (proximity/talk).
- Reward table ID **26** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 108
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 108
- `iNPC_ID` = runtime ID from terminator NPC 766
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the fake Chickens From Outer Space.
