# Mission 372 — Mystery of the Meteor (Part 5 of 6)

| Field | Value |
|-------|-------|
| Mission ID | 372 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2002 | GotoLocation | — | — | 2003 | — | 738 | 2267 |
| 2003 | UseItems | — | grant:200, gate:200 | 2005 | 2004 | — | 2314 |
| 2004 | GotoLocation | — | — | 2003 | — | — | 2268 |
| 2005 | Delivery | — | — | — | — | — | 738 |

## Chain edges (from table)

- Success: **2002** → **2003**
- Success: **2003** → **2005**
- Fail (err 1/12): **2003** → **2004**
- Success: **2004** → **2003**

## Task 2002

**Tags:** chains-to=2003

### How this task is received

- Player accepts from NPC table ID **738** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **371**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **738** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2267** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2002
- `iNPC_ID` = runtime ID from grant NPC 738
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2002
- `iNPC_ID` = runtime ID from terminator NPC 2267
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Reduce the infection in the Crystalline Caverns.

## Task 2003

**Tags:** grant-timer=200s, complete-timer-gate=200s, chains-to=2005, fail-restart=2004

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **371**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2314** (proximity/talk).
- Grant timer **200**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **200**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2003
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2003
- `iNPC_ID` = runtime ID from terminator NPC 2314
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Reduce the infection in the Crystalline Caverns.

## Task 2004

**Tags:** chains-to=2003

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **371**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2268** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2004
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2004
- `iNPC_ID` = runtime ID from terminator NPC 2268
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Reduce the infection in the Crystalline Caverns.

## Task 2005

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **371**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **738** (proximity/talk).
- Collect items: item 506 x1
- Reward table ID **458** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2005
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2005
- `iNPC_ID` = runtime ID from terminator NPC 738
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Reduce the infection in the Crystalline Caverns.
