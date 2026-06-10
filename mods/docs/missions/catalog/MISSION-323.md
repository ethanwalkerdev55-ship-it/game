# Mission 323 — Numbuh One Fan (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 323 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1318 | GotoLocation | — | — | 1320 | — | 723 | 900 |
| 1319 | GotoLocation | — | — | 1320 | — | — | 900 |
| 1320 | UseItems | — | grant:300, gate:300 | — | 1319 | — | 1690 |

## Chain edges (from table)

- Success: **1318** → **1320**
- Success: **1319** → **1320**
- Fail (err 1/12): **1320** → **1319**

## Task 1318

**Tags:** chains-to=1320

### How this task is received

- Player accepts from NPC table ID **723** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **322**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **723** (journal accept or auto-chain).
- Complete at terminator NPC table ID **900** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1318
- `iNPC_ID` = runtime ID from grant NPC 723
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1318
- `iNPC_ID` = runtime ID from terminator NPC 900
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defuse a Gooby Trap in the Fissure.

## Task 1319

**Tags:** chains-to=1320

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **322**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **900** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1319
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1319
- `iNPC_ID` = runtime ID from terminator NPC 900
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defuse a Gooby Trap in the Fissure.

## Task 1320

**Tags:** grant-timer=300s, complete-timer-gate=300s, fail-restart=1319

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **322**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1690** (proximity/talk).
- Grant timer **300**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **300**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **298** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1320
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1320
- `iNPC_ID` = runtime ID from terminator NPC 1690
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defuse a Gooby Trap in the Fissure.
