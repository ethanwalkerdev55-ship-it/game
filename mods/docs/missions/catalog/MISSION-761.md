# Mission 761 — Overclocking
(Part 1 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 761 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2661 | Talk | — | — | 2662 | — | 3233 | 2148 |
| 2662 | Talk | — | — | 2663 | — | — | 751 |
| 2663 | UseItems | — | — | 2665 | — | — | 3241 |
| 2664 | UseItems | — | — | 2665 | — | — | 3241 |
| 2665 | Talk | — | grant:140, gate:140 | — | 2664 | — | 3233 |

## Chain edges (from table)

- Success: **2661** → **2662**
- Success: **2662** → **2663**
- Success: **2663** → **2665**
- Success: **2664** → **2665**
- Fail (err 1/12): **2665** → **2664**

## Task 2661

**Tags:** chains-to=2662

### How this task is received

- Player accepts from NPC table ID **3233** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **760**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3233** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2148** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2661
- `iNPC_ID` = runtime ID from grant NPC 3233
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2661
- `iNPC_ID` = runtime ID from terminator NPC 2148
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help to fix DOC-27.

## Task 2662

**Tags:** chains-to=2663

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **760**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **751** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2662
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2662
- `iNPC_ID` = runtime ID from terminator NPC 751
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help to fix DOC-27.

## Task 2663

**Tags:** chains-to=2665

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **760**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3241** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2663
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2663
- `iNPC_ID` = runtime ID from terminator NPC 3241
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help to fix DOC-27.

## Task 2664

**Tags:** chains-to=2665

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **760**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3241** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2664
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2664
- `iNPC_ID` = runtime ID from terminator NPC 3241
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help to fix DOC-27.

## Task 2665

**Tags:** grant-timer=140s, complete-timer-gate=140s, fail-restart=2664

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **760**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3233** (proximity/talk).
- Grant timer **140**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **140**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **673** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2665
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2665
- `iNPC_ID` = runtime ID from terminator NPC 3233
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help to fix DOC-27.
