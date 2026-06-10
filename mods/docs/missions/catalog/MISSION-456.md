# Mission 456 — What the Dill?

| Field | Value |
|-------|-------|
| Mission ID | 456 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1413 | GotoLocation | — | — | 1414 | — | 722 | 1796 |
| 1414 | Defeat | — | — | 1415 | — | — | — |
| 1415 | Talk | — | — | — | — | — | 722 |

## Chain edges (from table)

- Success: **1413** → **1414**
- Success: **1414** → **1415**

## Task 1413

**Tags:** chains-to=1414

### How this task is received

- Player accepts from NPC table ID **722** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **722** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1796** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1413
- `iNPC_ID` = runtime ID from grant NPC 722
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1413
- `iNPC_ID` = runtime ID from terminator NPC 1796
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the Mayor's dill garden.

## Task 1414

**Tags:** kill-quota, chains-to=1415

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 341 x6

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1414
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1414
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the Mayor's dill garden.

## Task 1415

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **722** (proximity/talk).
- Reward table ID **319** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1415
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1415
- `iNPC_ID` = runtime ID from terminator NPC 722
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the Mayor's dill garden.
