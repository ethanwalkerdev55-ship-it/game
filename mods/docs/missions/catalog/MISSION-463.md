# Mission 463 — A Wee Wish

| Field | Value |
|-------|-------|
| Mission ID | 463 |
| Mission type | Normal |
| Task count | 6 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2073 | UseItems | — | — | 2074 | — | 724 | 2331 |
| 2074 | Delivery | — | — | 2075 | — | — | 724 |
| 2075 | Defeat | — | — | 2076 | — | — | — |
| 2076 | Defeat | — | — | 2077 | — | — | — |
| 2077 | Defeat | — | — | 2078 | — | — | — |
| 2078 | Delivery | — | — | — | — | — | 724 |

## Chain edges (from table)

- Success: **2073** → **2074**
- Success: **2074** → **2075**
- Success: **2075** → **2076**
- Success: **2076** → **2077**
- Success: **2077** → **2078**

## Task 2073

**Tags:** chains-to=2074

### How this task is received

- Player accepts from NPC table ID **724** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **453**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **724** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2331** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2073
- `iNPC_ID` = runtime ID from grant NPC 724
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2073
- `iNPC_ID` = runtime ID from terminator NPC 2331
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get bagpipes for Scotsman.

## Task 2074

**Tags:** item-quota, chains-to=2075

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **453**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **724** (proximity/talk).
- Collect items: item 525 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2074
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2074
- `iNPC_ID` = runtime ID from terminator NPC 724
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get bagpipes for Scotsman.

## Task 2075

**Tags:** kill-quota, item-quota, chains-to=2076

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **453**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 156 x0
- Collect items: item 526 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2075
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2075
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get bagpipes for Scotsman.

## Task 2076

**Tags:** kill-quota, item-quota, chains-to=2077

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **453**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 348 x0
- Collect items: item 526 x2

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2076
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2076
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get bagpipes for Scotsman.

## Task 2077

**Tags:** kill-quota, item-quota, chains-to=2078

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **453**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 349 x0
- Collect items: item 526 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2077
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2077
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get bagpipes for Scotsman.

## Task 2078

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **453**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **724** (proximity/talk).
- Collect items: item 526 x3
- Reward table ID **477** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2078
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2078
- `iNPC_ID` = runtime ID from terminator NPC 724
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get bagpipes for Scotsman.
