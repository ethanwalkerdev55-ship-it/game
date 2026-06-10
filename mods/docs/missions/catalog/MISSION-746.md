# Mission 746 — Jam Band (Part 3 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 746 |
| Mission type | Normal |
| Task count | 7 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2599 | GotoLocation | — | — | 2600 | — | 3072 | 2789 |
| 2600 | Talk | — | — | 2601 | — | — | 3194 |
| 2601 | UseItems | — | grant:50, gate:50 | 2603 | 2602 | — | 3195 |
| 2602 | Talk | — | — | 2601 | — | — | 3194 |
| 2603 | UseItems | — | grant:45, gate:45 | 2604 | 2602 | — | 3196 |
| 2604 | UseItems | — | grant:40, gate:40 | 2605 | 2602 | — | 3197 |
| 2605 | Talk | — | — | — | — | — | 3194 |

## Chain edges (from table)

- Success: **2599** → **2600**
- Success: **2600** → **2601**
- Success: **2601** → **2603**
- Fail (err 1/12): **2601** → **2602**
- Success: **2602** → **2601**
- Success: **2603** → **2604**
- Fail (err 1/12): **2603** → **2602**
- Success: **2604** → **2605**
- Fail (err 1/12): **2604** → **2602**

## Task 2599

**Tags:** chains-to=2600

### How this task is received

- Player accepts from NPC table ID **3072** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **3072** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2789** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2599
- `iNPC_ID` = runtime ID from grant NPC 3072
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2599
- `iNPC_ID` = runtime ID from terminator NPC 2789
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.

## Task 2600

**Tags:** chains-to=2601

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3194** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2600
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2600
- `iNPC_ID` = runtime ID from terminator NPC 3194
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.

## Task 2601

**Tags:** grant-timer=50s, complete-timer-gate=50s, chains-to=2603, fail-restart=2602

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3195** (proximity/talk).
- Grant timer **50**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **50**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2601
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2601
- `iNPC_ID` = runtime ID from terminator NPC 3195
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.

## Task 2602

**Tags:** chains-to=2601

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3194** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2602
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2602
- `iNPC_ID` = runtime ID from terminator NPC 3194
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.

## Task 2603

**Tags:** grant-timer=45s, complete-timer-gate=45s, chains-to=2604, fail-restart=2602

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3196** (proximity/talk).
- Grant timer **45**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **45**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2603
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2603
- `iNPC_ID` = runtime ID from terminator NPC 3196
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.

## Task 2604

**Tags:** grant-timer=40s, complete-timer-gate=40s, chains-to=2605, fail-restart=2602

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3197** (proximity/talk).
- Grant timer **40**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **40**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2604
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2604
- `iNPC_ID` = runtime ID from terminator NPC 3197
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.

## Task 2605

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **745**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3194** (proximity/talk).
- Reward table ID **657** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2605
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2605
- `iNPC_ID` = runtime ID from terminator NPC 3194
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the SACT jamming device.
