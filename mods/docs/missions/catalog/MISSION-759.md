# Mission 759 — Rivals
(Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 759 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2652 | Talk | — | — | 2653 | — | 2148 | 3233 |
| 2653 | Talk | — | grant:90, gate:90 | 2655 | 2654 | — | 3224 |
| 2654 | Talk | — | — | 2653 | — | — | 3233 |
| 2655 | Talk | — | — | — | — | — | 2148 |

## Chain edges (from table)

- Success: **2652** → **2653**
- Success: **2653** → **2655**
- Fail (err 1/12): **2653** → **2654**
- Success: **2654** → **2653**

## Task 2652

**Tags:** chains-to=2653

### How this task is received

- Player accepts from NPC table ID **2148** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **758**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **2148** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3233** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2652
- `iNPC_ID` = runtime ID from grant NPC 2148
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2652
- `iNPC_ID` = runtime ID from terminator NPC 3233
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Teach Jon Winthrop a lesson.

## Task 2653

**Tags:** grant-timer=90s, complete-timer-gate=90s, chains-to=2655, fail-restart=2654

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **758**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3224** (proximity/talk).
- Grant timer **90**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **90**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2653
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2653
- `iNPC_ID` = runtime ID from terminator NPC 3224
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Teach Jon Winthrop a lesson.

## Task 2654

**Tags:** chains-to=2653

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **758**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3233** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2654
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2654
- `iNPC_ID` = runtime ID from terminator NPC 3233
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Teach Jon Winthrop a lesson.

## Task 2655

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **758**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2148** (proximity/talk).
- Reward table ID **671** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2655
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2655
- `iNPC_ID` = runtime ID from terminator NPC 2148
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Teach Jon Winthrop a lesson.
