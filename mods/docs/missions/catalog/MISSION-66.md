# Mission 66 — Don't Be a Drip

| Field | Value |
|-------|-------|
| Mission ID | 66 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 255 | Delivery | — | grant:180, gate:180 | 257 | 256 | 2558 | 704 |
| 256 | Talk | — | — | 255 | — | — | 2558 |
| 257 | Talk | — | — | — | — | — | 2558 |

## Chain edges (from table)

- Success: **255** → **257**
- Fail (err 1/12): **255** → **256**
- Success: **256** → **255**

## Task 255

**Tags:** grant-timer=180s, complete-timer-gate=180s, item-quota, chains-to=257, fail-restart=256

### How this task is received

- Player accepts from NPC table ID **2558** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Delivery**
- Start at grant NPC table ID **2558** (journal accept or auto-chain).
- Complete at terminator NPC table ID **704** (proximity/talk).
- Grant timer **180**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **180**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Collect items: item 79 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 255
- `iNPC_ID` = runtime ID from grant NPC 2558
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 255
- `iNPC_ID` = runtime ID from terminator NPC 704
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver ice cream to the KND tree house before it melts.

## Task 256

**Tags:** chains-to=255

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2558** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 256
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 256
- `iNPC_ID` = runtime ID from terminator NPC 2558
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver ice cream to the KND tree house before it melts.

## Task 257

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2558** (proximity/talk).
- Reward table ID **66** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 257
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 257
- `iNPC_ID` = runtime ID from terminator NPC 2558
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver ice cream to the KND tree house before it melts.
