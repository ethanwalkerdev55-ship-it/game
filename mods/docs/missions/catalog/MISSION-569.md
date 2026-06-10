# Mission 569 — Plant a Seed

| Field | Value |
|-------|-------|
| Mission ID | 569 |
| Mission type | Nano |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1547 | Defeat | — | — | 1548 | — | — | — |
| 1548 | GotoLocation | — | — | 1549 | — | — | 2370 |
| 1549 | GotoLocation | — | — | 1550 | — | — | 1976 |
| 1550 | UseItems | 90 | — | 1551 | 1548 | — | 1993 |
| 1551 | Defeat | 90 | — | — | 1548 | — | — |

## Chain edges (from table)

- Success: **1547** → **1548**
- Success: **1548** → **1549**
- Success: **1549** → **1550**
- Success: **1550** → **1551**
- Fail (err 1/12): **1550** → **1548**
- Fail (err 1/12): **1551** → **1548**

## Task 1547

**Tags:** kill-quota, item-quota, chains-to=1548

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2059 x0
- Collect items: item 395 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1547
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1547
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Wisdom Seed in Dark Tree.

## Task 1548

**Tags:** chains-to=1549

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2370** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1548
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1548
- `iNPC_ID` = runtime ID from terminator NPC 2370
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Wisdom Seed in Dark Tree.

## Task 1549

**Tags:** chains-to=1550

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1976** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1549
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1549
- `iNPC_ID` = runtime ID from terminator NPC 1976
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Wisdom Seed in Dark Tree.

## Task 1550

**Tags:** instance-id=90, chains-to=1551, fail-restart=1548

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1993** (proximity/talk).
- Instance required (**m_iRequireInstanceID=90**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1550
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1550
- `iNPC_ID` = runtime ID from terminator NPC 1993
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Wisdom Seed in Dark Tree.

## Task 1551

**Tags:** instance-id=90, kill-quota, fail-restart=1548

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=90**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 50 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1551
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1551
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Place Wisdom Seed in Dark Tree.
