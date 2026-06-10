# Mission 421 — Double Fusion Trouble (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 421 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1363 | GotoLocation | — | — | 1364 | — | 1192 | 1789 |
| 1364 | GotoLocation | — | — | 1365 | — | — | 1790 |
| 1365 | Defeat | 66 | — | 1366 | 1363 | — | — |
| 1366 | GotoLocation | 66 | — | 1367 | 1367 | — | 1789 |
| 1367 | Talk | — | — | — | — | — | 701 |

## Chain edges (from table)

- Success: **1363** → **1364**
- Success: **1364** → **1365**
- Success: **1365** → **1366**
- Fail (err 1/12): **1365** → **1363**
- Success: **1366** → **1367**
- Fail (err 1/12): **1366** → **1367**

## Task 1363

**Tags:** item-quota, chains-to=1364

### How this task is received

- Player accepts from NPC table ID **1192** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **420**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **1192** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1789** (proximity/talk).
- Collect items: item 346 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1363
- `iNPC_ID` = runtime ID from grant NPC 1192
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1363
- `iNPC_ID` = runtime ID from terminator NPC 1789
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Dexter's lab from Fusions.

## Task 1364

**Tags:** chains-to=1365

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **420**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1790** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1364
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1364
- `iNPC_ID` = runtime ID from terminator NPC 1790
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Dexter's lab from Fusions.

## Task 1365

**Tags:** instance-id=66, kill-quota, chains-to=1366, fail-restart=1363

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **420**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=66**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 1710 x1, enemy 1711 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1365
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1365
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Dexter's lab from Fusions.

## Task 1366

**Tags:** instance-id=66, chains-to=1367, fail-restart=1367

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **420**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1789** (proximity/talk).
- Instance required (**m_iRequireInstanceID=66**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1366
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1366
- `iNPC_ID` = runtime ID from terminator NPC 1789
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Dexter's lab from Fusions.

## Task 1367

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **420**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **701** (proximity/talk).
- Reward table ID **487** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1367
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1367
- `iNPC_ID` = runtime ID from terminator NPC 701
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect Dexter's lab from Fusions.
