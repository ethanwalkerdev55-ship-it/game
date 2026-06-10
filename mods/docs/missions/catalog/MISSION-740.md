# Mission 740 — LOCONUT (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 740 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2577 | GotoLocation | — | — | 2578 | — | 2806 | 2782 |
| 2578 | Talk | — | — | 2579 | — | — | 2806 |
| 2579 | Talk | — | grant:150, gate:150 | 2581 | 2580 | — | 3157 |
| 2580 | Talk | — | — | 2579 | — | — | 2806 |
| 2581 | Talk | — | — | — | — | — | 2806 |

## Chain edges (from table)

- Success: **2577** → **2578**
- Success: **2578** → **2579**
- Success: **2579** → **2581**
- Fail (err 1/12): **2579** → **2580**
- Success: **2580** → **2579**

## Task 2577

**Tags:** chains-to=2578

### How this task is received

- Player accepts from NPC table ID **2806** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **739**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **2806** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2782** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2577
- `iNPC_ID` = runtime ID from grant NPC 2806
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2577
- `iNPC_ID` = runtime ID from terminator NPC 2782
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Convince Numbuh 621 that the LOCONUT won't work.

## Task 2578

**Tags:** chains-to=2579

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **739**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2806** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2578
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2578
- `iNPC_ID` = runtime ID from terminator NPC 2806
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Convince Numbuh 621 that the LOCONUT won't work.

## Task 2579

**Tags:** grant-timer=150s, complete-timer-gate=150s, chains-to=2581, fail-restart=2580

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **739**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3157** (proximity/talk).
- Grant timer **150**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **150**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2579
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2579
- `iNPC_ID` = runtime ID from terminator NPC 3157
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Convince Numbuh 621 that the LOCONUT won't work.

## Task 2580

**Tags:** chains-to=2579

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **739**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2806** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2580
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2580
- `iNPC_ID` = runtime ID from terminator NPC 2806
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Convince Numbuh 621 that the LOCONUT won't work.

## Task 2581

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **739**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2806** (proximity/talk).
- Reward table ID **651** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2581
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2581
- `iNPC_ID` = runtime ID from terminator NPC 2806
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Convince Numbuh 621 that the LOCONUT won't work.
