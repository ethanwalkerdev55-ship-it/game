# Mission 838 — Naughty or Nice King?

| Field | Value |
|-------|-------|
| Mission ID | 838 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5200 | GotoLocation | — | — | 5201 | — | 3269 | 1587 |
| 5201 | Defeat | 142 | — | 5202 | 5200 | — | — |
| 5202 | Talk | — | — | — | — | — | 3269 |

## Chain edges (from table)

- Success: **5200** → **5201**
- Success: **5201** → **5202**
- Fail (err 1/12): **5201** → **5200**

## Task 5200

**Tags:** chains-to=5201

### How this task is received

- Player accepts from NPC table ID **3269** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **837**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **3269** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1587** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5200
- `iNPC_ID` = runtime ID from grant NPC 3269
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5200
- `iNPC_ID` = runtime ID from terminator NPC 1587
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Fusion Ice King and figure out the deal with the penguin monsters.

## Task 5201

**Tags:** instance-id=142, kill-quota, chains-to=5202, fail-restart=5200

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=142**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 252 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5201
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5201
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Fusion Ice King and figure out the deal with the penguin monsters.

## Task 5202

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3269** (proximity/talk).
- Reward table ID **717** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5202
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5202
- `iNPC_ID` = runtime ID from terminator NPC 3269
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Fusion Ice King and figure out the deal with the penguin monsters.
