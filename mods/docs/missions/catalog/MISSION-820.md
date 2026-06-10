# Mission 820 — Into the Spider-Lair

| Field | Value |
|-------|-------|
| Mission ID | 820 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5127 | GotoLocation | — | — | 5128 | — | 699 | 2792 |
| 5128 | Talk | — | — | 5129 | — | — | 750 |
| 5129 | GotoLocation | — | — | 5130 | — | — | 662 |
| 5130 | Defeat | 139 | — | 5131 | 5129 | — | — |
| 5131 | Talk | — | — | — | — | — | 750 |

## Chain edges (from table)

- Success: **5127** → **5128**
- Success: **5128** → **5129**
- Success: **5129** → **5130**
- Success: **5130** → **5131**
- Fail (err 1/12): **5130** → **5129**

## Task 5127

**Tags:** chains-to=5128

### How this task is received

- Player accepts from NPC table ID **699** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **819**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **699** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2792** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5127
- `iNPC_ID` = runtime ID from grant NPC 699
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5127
- `iNPC_ID` = runtime ID from terminator NPC 2792
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Tetrax to learn more about Fusion Spidermonkey.

## Task 5128

**Tags:** chains-to=5129

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **750** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5128
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5128
- `iNPC_ID` = runtime ID from terminator NPC 750
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Tetrax to learn more about Fusion Spidermonkey.

## Task 5129

**Tags:** chains-to=5130

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **662** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5129
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5129
- `iNPC_ID` = runtime ID from terminator NPC 662
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Tetrax to learn more about Fusion Spidermonkey.

## Task 5130

**Tags:** instance-id=139, kill-quota, chains-to=5131, fail-restart=5129

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=139**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 3256 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5130
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5130
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Tetrax to learn more about Fusion Spidermonkey.

## Task 5131

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **750** (proximity/talk).
- Reward table ID **471** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5131
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5131
- `iNPC_ID` = runtime ID from terminator NPC 750
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Tetrax to learn more about Fusion Spidermonkey.
