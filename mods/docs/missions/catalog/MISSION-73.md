# Mission 73 — Booger Boogie

| Field | Value |
|-------|-------|
| Mission ID | 73 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 286 | UseItems | — | grant:420, gate:420 | 288 | 287 | 710 | 852 |
| 287 | Talk | — | — | 286 | — | — | 710 |
| 288 | Talk | — | — | — | — | — | 710 |

## Chain edges (from table)

- Success: **286** → **288**
- Fail (err 1/12): **286** → **287**
- Success: **287** → **286**

## Task 286

**Tags:** grant-timer=420s, complete-timer-gate=420s, chains-to=288, fail-restart=287

### How this task is received

- Player accepts from NPC table ID **710** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **71**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **710** (journal accept or auto-chain).
- Complete at terminator NPC table ID **852** (proximity/talk).
- Grant timer **420**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **420**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 286
- `iNPC_ID` = runtime ID from grant NPC 710
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 286
- `iNPC_ID` = runtime ID from terminator NPC 852
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recycle Billy's boogers before they ferment.

## Task 287

**Tags:** chains-to=286

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **71**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **710** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 287
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 287
- `iNPC_ID` = runtime ID from terminator NPC 710
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recycle Billy's boogers before they ferment.

## Task 288

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **71**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **710** (proximity/talk).
- Reward table ID **73** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 288
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 288
- `iNPC_ID` = runtime ID from terminator NPC 710
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recycle Billy's boogers before they ferment.
