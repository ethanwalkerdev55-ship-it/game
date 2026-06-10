# Mission 819 — With Great Power…

| Field | Value |
|-------|-------|
| Mission ID | 819 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5123 | Talk | — | — | 5124 | — | 699 | 1591 |
| 5124 | Talk | — | — | 5125 | — | — | 1592 |
| 5125 | Talk | — | — | 5126 | — | — | 1596 |
| 5126 | Talk | — | — | — | — | — | 699 |

## Chain edges (from table)

- Success: **5123** → **5124**
- Success: **5124** → **5125**
- Success: **5125** → **5126**

## Task 5123

**Tags:** chains-to=5124

### How this task is received

- Player accepts from NPC table ID **699** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **818**.
- Prerequisite completed mission **816**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **699** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1591** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5123
- `iNPC_ID` = runtime ID from grant NPC 699
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5123
- `iNPC_ID` = runtime ID from terminator NPC 1591
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the web grenades in Monkey Foothills.

## Task 5124

**Tags:** chains-to=5125

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1592** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5124
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5124
- `iNPC_ID` = runtime ID from terminator NPC 1592
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the web grenades in Monkey Foothills.

## Task 5125

**Tags:** chains-to=5126

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1596** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5125
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5125
- `iNPC_ID` = runtime ID from terminator NPC 1596
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the web grenades in Monkey Foothills.

## Task 5126

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **699** (proximity/talk).
- Reward table ID **471** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5126
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5126
- `iNPC_ID` = runtime ID from terminator NPC 699
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Set up the web grenades in Monkey Foothills.
