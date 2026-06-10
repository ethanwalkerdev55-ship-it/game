# Mission 13 — Xtra Large Fusion (Part 1 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 13 |
| Mission type | Normal |
| Task count | 7 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 59 | GotoLocation | — | — | 60 | 59 | 718 | 886 |
| 60 | Defeat | — | — | 61 | 60 | — | — |
| 61 | Talk | — | — | 62 | 61 | — | 718 |
| 62 | GotoLocation | — | — | 63 | 62 | — | 916 |
| 63 | Defeat | — | — | 363 | — | — | — |
| 64 | GotoLocation | — | — | 63 | — | — | 916 |
| 363 | Delivery | — | grant:60, gate:60 | — | 64 | — | 718 |

## Chain edges (from table)

- Success: **59** → **60**
- Fail (err 1/12): **59** → **59**
- Success: **60** → **61**
- Fail (err 1/12): **60** → **60**
- Success: **61** → **62**
- Fail (err 1/12): **61** → **61**
- Success: **62** → **63**
- Fail (err 1/12): **62** → **62**
- Success: **63** → **363**
- Success: **64** → **63**
- Fail (err 1/12): **363** → **64**

## Task 59

**Tags:** chains-to=60, fail-restart=59

### How this task is received

- Player accepts from NPC table ID **718** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **718** (journal accept or auto-chain).
- Complete at terminator NPC table ID **886** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 59
- `iNPC_ID` = runtime ID from grant NPC 718
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 59
- `iNPC_ID` = runtime ID from terminator NPC 886
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.

## Task 60

**Tags:** kill-quota, chains-to=61, fail-restart=60

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 107 x4

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 60
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 60
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.

## Task 61

**Tags:** chains-to=62, fail-restart=61

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **718** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 61
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 61
- `iNPC_ID` = runtime ID from terminator NPC 718
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.

## Task 62

**Tags:** chains-to=63, fail-restart=62

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **916** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 62
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 62
- `iNPC_ID` = runtime ID from terminator NPC 916
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.

## Task 63

**Tags:** kill-quota, item-quota, chains-to=363

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 306 x0
- Collect items: item 239 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 63
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 63
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.

## Task 64

**Tags:** chains-to=63

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **916** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 64
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 64
- `iNPC_ID` = runtime ID from terminator NPC 916
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.

## Task 363

**Tags:** grant-timer=60s, complete-timer-gate=60s, item-quota, fail-restart=64

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **718** (proximity/talk).
- Grant timer **60**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **60**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Collect items: item 239 x1
- Reward table ID **18** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 363
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 363
- `iNPC_ID` = runtime ID from terminator NPC 718
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defend the salvage yard from attack.
