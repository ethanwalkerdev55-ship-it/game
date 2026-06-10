# Mission 453 — Scotsman's Best Friend (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 453 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1877 | GotoLocation | — | — | 1878 | — | 724 | 2244 |
| 1878 | GotoLocation | — | — | 1879 | — | — | 2245 |
| 1879 | Defeat | 114 | — | 1880 | 1877 | — | — |
| 1880 | GotoLocation | 114 | — | 1881 | 1881 | — | 2244 |
| 1881 | Delivery | — | — | — | — | — | 724 |

## Chain edges (from table)

- Success: **1877** → **1878**
- Success: **1878** → **1879**
- Success: **1879** → **1880**
- Fail (err 1/12): **1879** → **1877**
- Success: **1880** → **1881**
- Fail (err 1/12): **1880** → **1881**

## Task 1877

**Tags:** chains-to=1878

### How this task is received

- Player accepts from NPC table ID **724** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **452**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **724** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2244** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1877
- `iNPC_ID` = runtime ID from grant NPC 724
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1877
- `iNPC_ID` = runtime ID from terminator NPC 2244
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find and defeat Fusion Scotsman.

## Task 1878

**Tags:** chains-to=1879

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **452**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2245** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1878
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1878
- `iNPC_ID` = runtime ID from terminator NPC 2245
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find and defeat Fusion Scotsman.

## Task 1879

**Tags:** instance-id=114, kill-quota, chains-to=1880, fail-restart=1877

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **452**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=114**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 1916 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1879
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1879
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find and defeat Fusion Scotsman.

## Task 1880

**Tags:** instance-id=114, chains-to=1881, fail-restart=1881

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **452**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2244** (proximity/talk).
- Instance required (**m_iRequireInstanceID=114**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1880
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1880
- `iNPC_ID` = runtime ID from terminator NPC 2244
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find and defeat Fusion Scotsman.

## Task 1881

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **452**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **724** (proximity/talk).
- Collect items: item 477 x1
- Reward table ID **426** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1881
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1881
- `iNPC_ID` = runtime ID from terminator NPC 724
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find and defeat Fusion Scotsman.
