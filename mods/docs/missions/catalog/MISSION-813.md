# Mission 813 — Shock Value

| Field | Value |
|-------|-------|
| Mission ID | 813 |
| Mission type | Normal |
| Task count | 7 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5101 | Talk | — | — | 5102 | — | 698 | 729 |
| 5102 | Talk | — | — | 5103 | — | — | 2295 |
| 5103 | Talk | — | — | 5104 | — | — | 1586 |
| 5104 | GotoLocation | — | — | 5105 | — | — | 1589 |
| 5105 | Defeat | 138 | — | 5106 | 5104 | — | — |
| 5106 | GotoLocation | — | — | 5107 | — | — | 657 |
| 5107 | Talk | — | — | — | — | — | 698 |

## Chain edges (from table)

- Success: **5101** → **5102**
- Success: **5102** → **5103**
- Success: **5103** → **5104**
- Success: **5104** → **5105**
- Success: **5105** → **5106**
- Fail (err 1/12): **5105** → **5104**
- Success: **5106** → **5107**

## Task 5101

**Tags:** chains-to=5102

### How this task is received

- Player accepts from NPC table ID **698** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **812**.
- Prerequisite completed mission **811**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **698** (journal accept or auto-chain).
- Complete at terminator NPC table ID **729** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5101
- `iNPC_ID` = runtime ID from grant NPC 698
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5101
- `iNPC_ID` = runtime ID from terminator NPC 729
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.

## Task 5102

**Tags:** chains-to=5103

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2295** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5102
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5102
- `iNPC_ID` = runtime ID from terminator NPC 2295
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.

## Task 5103

**Tags:** chains-to=5104

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1586** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5103
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5103
- `iNPC_ID` = runtime ID from terminator NPC 1586
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.

## Task 5104

**Tags:** chains-to=5105

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1589** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5104
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5104
- `iNPC_ID` = runtime ID from terminator NPC 1589
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.

## Task 5105

**Tags:** instance-id=138, kill-quota, chains-to=5106, fail-restart=5104

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=138**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 32 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5105
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5105
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.

## Task 5106

**Tags:** chains-to=5107

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **657** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5106
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5106
- `iNPC_ID` = runtime ID from terminator NPC 657
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.

## Task 5107

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **698** (proximity/talk).
- Reward table ID **493** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5107
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5107
- `iNPC_ID` = runtime ID from terminator NPC 698
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop Fusion AmpFibian from hacking the power grid.
