# Mission 367 — Treetop Troubles (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 367 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1982 | GotoLocation | — | — | 1983 | — | 740 | 1602 |
| 1983 | UseItems | — | grant:250, gate:250 | 1985 | 1984 | — | 2311 |
| 1984 | GotoLocation | — | — | 1983 | — | — | 1602 |
| 1985 | Talk | — | — | — | — | — | 740 |

## Chain edges (from table)

- Success: **1982** → **1983**
- Success: **1983** → **1985**
- Fail (err 1/12): **1983** → **1984**
- Success: **1984** → **1983**

## Task 1982

**Tags:** chains-to=1983

### How this task is received

- Player accepts from NPC table ID **740** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **366**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **740** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1602** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1982
- `iNPC_ID` = runtime ID from grant NPC 740
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1982
- `iNPC_ID` = runtime ID from terminator NPC 1602
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Leakey Lake from Terrafusing.

## Task 1983

**Tags:** grant-timer=250s, complete-timer-gate=250s, chains-to=1985, fail-restart=1984

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **366**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2311** (proximity/talk).
- Grant timer **250**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **250**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1983
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1983
- `iNPC_ID` = runtime ID from terminator NPC 2311
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Leakey Lake from Terrafusing.

## Task 1984

**Tags:** chains-to=1983

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **366**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1602** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1984
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1984
- `iNPC_ID` = runtime ID from terminator NPC 1602
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Leakey Lake from Terrafusing.

## Task 1985

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **366**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **740** (proximity/talk).
- Reward table ID **453** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1985
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1985
- `iNPC_ID` = runtime ID from terminator NPC 740
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Leakey Lake from Terrafusing.
