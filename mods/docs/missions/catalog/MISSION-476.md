# Mission 476 — Toiletnator's Tomfoolery (Part 1 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 476 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2268 | Defeat | — | — | 2269 | — | 736 | — |
| 2269 | UseItems | — | — | 2270 | — | — | 2801 |
| 2270 | UseItems | — | — | 2271 | — | — | 2802 |
| 2271 | UseItems | — | — | 2272 | — | — | 2803 |
| 2272 | Talk | — | — | — | — | — | 736 |

## Chain edges (from table)

- Success: **2268** → **2269**
- Success: **2269** → **2270**
- Success: **2270** → **2271**
- Success: **2271** → **2272**

## Task 2268

**Tags:** kill-quota, item-quota, chains-to=2269

### How this task is received

- Player accepts from NPC table ID **736** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **736** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 133 x0
- Collect items: item 538 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2268
- `iNPC_ID` = runtime ID from grant NPC 736
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2268
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Toiletnator make a name for himself.

## Task 2269

**Tags:** chains-to=2270

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2801** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2269
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2269
- `iNPC_ID` = runtime ID from terminator NPC 2801
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Toiletnator make a name for himself.

## Task 2270

**Tags:** chains-to=2271

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2802** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2270
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2270
- `iNPC_ID` = runtime ID from terminator NPC 2802
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Toiletnator make a name for himself.

## Task 2271

**Tags:** chains-to=2272

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2803** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2271
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2271
- `iNPC_ID` = runtime ID from terminator NPC 2803
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Toiletnator make a name for himself.

## Task 2272

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **736** (proximity/talk).
- Reward table ID **537** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2272
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2272
- `iNPC_ID` = runtime ID from terminator NPC 736
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Toiletnator make a name for himself.
