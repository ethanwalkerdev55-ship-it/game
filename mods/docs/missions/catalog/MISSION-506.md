# Mission 506 — Pokey Protection

| Field | Value |
|-------|-------|
| Mission ID | 506 |
| Mission type | Normal |
| Task count | 8 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 473 | GotoLocation | — | — | 476 | — | 773 | 960 |
| 474 | Defeat | — | — | 473 | — | — | — |
| 475 | GotoLocation | — | — | 473 | — | — | 959 |
| 476 | Defeat | — | grant:150, gate:150 | 480 | 477 | — | — |
| 477 | GotoLocation | — | — | 476 | — | — | 960 |
| 478 | Defeat | — | grant:300, gate:300 | 480 | 477 | — | — |
| 479 | GotoLocation | — | — | 473 | — | — | 961 |
| 480 | Talk | — | — | — | — | — | 773 |

## Chain edges (from table)

- Success: **473** → **476**
- Success: **474** → **473**
- Success: **475** → **473**
- Success: **476** → **480**
- Fail (err 1/12): **476** → **477**
- Success: **477** → **476**
- Success: **478** → **480**
- Fail (err 1/12): **478** → **477**
- Success: **479** → **473**

## Task 473

**Tags:** chains-to=476

### How this task is received

- Player accepts from NPC table ID **773** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **773** (journal accept or auto-chain).
- Complete at terminator NPC table ID **960** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 473
- `iNPC_ID` = runtime ID from grant NPC 773
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 473
- `iNPC_ID` = runtime ID from terminator NPC 960
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 474

**Tags:** kill-quota, chains-to=473

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 249 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 474
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 474
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 475

**Tags:** chains-to=473

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **959** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 475
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 475
- `iNPC_ID` = runtime ID from terminator NPC 959
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 476

**Tags:** grant-timer=150s, complete-timer-gate=150s, kill-quota, chains-to=480, fail-restart=477

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Grant timer **150**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **150**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Kill quotas: enemy 83 x4

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 476
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 476
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 477

**Tags:** chains-to=476

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **960** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 477
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 477
- `iNPC_ID` = runtime ID from terminator NPC 960
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 478

**Tags:** grant-timer=300s, complete-timer-gate=300s, kill-quota, chains-to=480, fail-restart=477

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Grant timer **300**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **300**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Kill quotas: enemy 251 x4

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 478
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 478
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 479

**Tags:** chains-to=473

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **961** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 479
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 479
- `iNPC_ID` = runtime ID from terminator NPC 961
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.

## Task 480

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **505**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **773** (proximity/talk).
- Reward table ID **111** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 480
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 480
- `iNPC_ID` = runtime ID from terminator NPC 773
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend Pokey Oaks from Fuse's monsters.
