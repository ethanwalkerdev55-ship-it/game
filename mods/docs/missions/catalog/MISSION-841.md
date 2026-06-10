# Mission 841 — The Unstable Nano

| Field | Value |
|-------|-------|
| Mission ID | 841 |
| Mission type | Nano |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5213 | Talk | — | — | 5214 | — | 3076 | 3075 |
| 5214 | Talk | — | — | 5215 | — | — | 3074 |
| 5215 | Talk | — | — | — | — | — | 3076 |

## Chain edges (from table)

- Success: **5213** → **5214**
- Success: **5214** → **5215**

## Task 5213

**Tags:** chains-to=5214

### How this task is received

- Player accepts from NPC table ID **3076** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3076** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3075** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5213
- `iNPC_ID` = runtime ID from grant NPC 3076
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5213
- `iNPC_ID` = runtime ID from terminator NPC 3075
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Learn about the Unstable Nano and create one of your own.

## Task 5214

**Tags:** chains-to=5215

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3074** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5214
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5214
- `iNPC_ID` = runtime ID from terminator NPC 3074
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Learn about the Unstable Nano and create one of your own.

## Task 5215

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3076** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5215
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5215
- `iNPC_ID` = runtime ID from terminator NPC 3076
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Learn about the Unstable Nano and create one of your own.
