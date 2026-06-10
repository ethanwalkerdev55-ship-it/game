# Mission 34 — Nuclear Haste

| Field | Value |
|-------|-------|
| Mission ID | 34 |
| Mission type | Guide |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 136 | GotoLocation | — | — | 137 | — | 704 | 897 |
| 137 | UseItems | — | grant:360, gate:360 | 915 | 138 | — | 816 |
| 138 | GotoLocation | — | — | 137 | — | — | 897 |
| 915 | Delivery | — | — | — | — | — | 704 |

## Chain edges (from table)

- Success: **136** → **137**
- Success: **137** → **915**
- Fail (err 1/12): **137** → **138**
- Success: **138** → **137**

## Task 136

**Tags:** chains-to=137

### How this task is received

- Player accepts from NPC table ID **704** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **33**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **704** (journal accept or auto-chain).
- Complete at terminator NPC table ID **897** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 136
- `iNPC_ID` = runtime ID from grant NPC 704
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 136
- `iNPC_ID` = runtime ID from terminator NPC 897
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a radioactive isotope from the nuclear power plant.

## Task 137

**Tags:** grant-timer=360s, complete-timer-gate=360s, chains-to=915, fail-restart=138

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **33**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **816** (proximity/talk).
- Grant timer **360**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **360**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 137
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 137
- `iNPC_ID` = runtime ID from terminator NPC 816
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a radioactive isotope from the nuclear power plant.

## Task 138

**Tags:** chains-to=137

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **33**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **897** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 138
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 138
- `iNPC_ID` = runtime ID from terminator NPC 897
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a radioactive isotope from the nuclear power plant.

## Task 915

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **33**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **704** (proximity/talk).
- Collect items: item 52 x1
- Reward table ID **34** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 915
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 915
- `iNPC_ID` = runtime ID from terminator NPC 704
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover a radioactive isotope from the nuclear power plant.
