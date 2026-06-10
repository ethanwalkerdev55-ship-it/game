# Mission 12 — Hands on a Hard Drive (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 12 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 55 | Delivery | — | — | 56 | — | 718 | 791 |
| 56 | Delivery | — | grant:300, gate:300 | 124 | 58 | — | 725 |
| 58 | Talk | — | — | 56 | — | — | 791 |
| 124 | Talk | — | — | — | — | — | 791 |

## Chain edges (from table)

- Success: **55** → **56**
- Success: **56** → **124**
- Fail (err 1/12): **56** → **58**
- Success: **58** → **56**

## Task 55

**Tags:** item-quota, chains-to=56

### How this task is received

- Player accepts from NPC table ID **718** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **718** (journal accept or auto-chain).
- Complete at terminator NPC table ID **791** (proximity/talk).
- Collect items: item 38 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 55
- `iNPC_ID` = runtime ID from grant NPC 718
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 55
- `iNPC_ID` = runtime ID from terminator NPC 791
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Restore the data on Coop's hard drive.

## Task 56

**Tags:** grant-timer=300s, complete-timer-gate=300s, item-quota, chains-to=124, fail-restart=58

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **725** (proximity/talk).
- Grant timer **300**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **300**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Collect items: item 38 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 56
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 56
- `iNPC_ID` = runtime ID from terminator NPC 725
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Restore the data on Coop's hard drive.

## Task 58

**Tags:** chains-to=56

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **791** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 58
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 58
- `iNPC_ID` = runtime ID from terminator NPC 791
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Restore the data on Coop's hard drive.

## Task 124

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **11**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **791** (proximity/talk).
- Reward table ID **17** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 124
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 124
- `iNPC_ID` = runtime ID from terminator NPC 791
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Restore the data on Coop's hard drive.
