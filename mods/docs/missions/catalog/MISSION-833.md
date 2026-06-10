# Mission 833 — Making History

| Field | Value |
|-------|-------|
| Mission ID | 833 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5175 | Defeat | — | — | 5176 | — | 692 | — |
| 5176 | Defeat | — | grant:30 | 5176 | 5177 | — | — |
| 5177 | Talk | — | — | — | — | — | 692 |

## Chain edges (from table)

- Success: **5175** → **5176**
- Success: **5176** → **5176**
- Fail (err 1/12): **5176** → **5177**

## Task 5175

**Tags:** kill-quota, chains-to=5176

### How this task is received

- Player accepts from NPC table ID **692** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **832**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **692** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 114 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5175
- `iNPC_ID` = runtime ID from grant NPC 692
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5175
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Destroy the entirety of Planet Fusion’s invasion for Larry.

## Task 5176

**Tags:** grant-timer=30s, kill-quota, chains-to=5176, fail-restart=5177

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Grant timer **30**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Kill quotas: enemy 114 x3

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5176
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5176
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Destroy the entirety of Planet Fusion’s invasion for Larry.

## Task 5177

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **692** (proximity/talk).
- Reward table ID **218** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5177
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5177
- `iNPC_ID` = runtime ID from terminator NPC 692
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Destroy the entirety of Planet Fusion’s invasion for Larry.
