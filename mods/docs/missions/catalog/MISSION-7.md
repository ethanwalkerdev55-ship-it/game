# Mission 7 — Master of Dishaster

| Field | Value |
|-------|-------|
| Mission ID | 7 |
| Mission type | Normal |
| Task count | 7 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 3 | UseItems | — | — | 4 | — | 704 | 814 |
| 4 | Defeat | — | — | 5 | — | — | — |
| 5 | Delivery | — | — | 6 | — | — | 704 |
| 6 | UseItems | — | — | 501 | — | — | 814 |
| 7 | UseItems | — | — | 501 | — | — | 814 |
| 501 | Defeat | — | grant:180, gate:180 | 677 | 7 | — | — |
| 677 | Talk | — | — | — | — | — | 704 |

## Chain edges (from table)

- Success: **3** → **4**
- Success: **4** → **5**
- Success: **5** → **6**
- Success: **6** → **501**
- Success: **7** → **501**
- Success: **501** → **677**
- Fail (err 1/12): **501** → **7**

## Task 3

**Tags:** chains-to=4

### How this task is received

- Player accepts from NPC table ID **704** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **704** (journal accept or auto-chain).
- Complete at terminator NPC table ID **814** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 3
- `iNPC_ID` = runtime ID from grant NPC 704
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 3
- `iNPC_ID` = runtime ID from terminator NPC 814
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.

## Task 4

**Tags:** kill-quota, item-quota, chains-to=5

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 274 x0
- Collect items: item 24 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 4
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 4
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.

## Task 5

**Tags:** item-quota, chains-to=6

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **704** (proximity/talk).
- Collect items: item 24 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5
- `iNPC_ID` = runtime ID from terminator NPC 704
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.

## Task 6

**Tags:** chains-to=501

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **814** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 6
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 6
- `iNPC_ID` = runtime ID from terminator NPC 814
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.

## Task 7

**Tags:** chains-to=501

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **814** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 7
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 7
- `iNPC_ID` = runtime ID from terminator NPC 814
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.

## Task 501

**Tags:** grant-timer=180s, complete-timer-gate=180s, kill-quota, chains-to=677, fail-restart=7

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Grant timer **180**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **180**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Kill quotas: enemy 274 x8

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 501
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 501
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.

## Task 677

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **704** (proximity/talk).
- Reward table ID **22** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 677
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 677
- `iNPC_ID` = runtime ID from terminator NPC 704
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair a busted communications dish.
