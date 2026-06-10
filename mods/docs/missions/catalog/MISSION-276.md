# Mission 276 — Laboratory Safety (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 276 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 885 | UseItems | — | grant:300, gate:300 | 887 | 886 | 705 | 1387 |
| 886 | GotoLocation | — | — | 885 | — | — | 1350 |
| 887 | UseItems | — | grant:240, gate:240 | 888 | 886 | — | 1388 |
| 888 | UseItems | — | grant:180, gate:180 | — | 886 | — | 1389 |

## Chain edges (from table)

- Success: **885** → **887**
- Fail (err 1/12): **885** → **886**
- Success: **886** → **885**
- Success: **887** → **888**
- Fail (err 1/12): **887** → **886**
- Fail (err 1/12): **888** → **886**

## Task 885

**Tags:** grant-timer=300s, complete-timer-gate=300s, chains-to=887, fail-restart=886

### How this task is received

- Player accepts from NPC table ID **705** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **275**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **705** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1387** (proximity/talk).
- Grant timer **300**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **300**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 885
- `iNPC_ID` = runtime ID from grant NPC 705
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 885
- `iNPC_ID` = runtime ID from terminator NPC 1387
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Fill holes left by Tech Tunnelers and Doom Drones.

## Task 886

**Tags:** chains-to=885

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **275**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1350** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 886
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 886
- `iNPC_ID` = runtime ID from terminator NPC 1350
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Fill holes left by Tech Tunnelers and Doom Drones.

## Task 887

**Tags:** grant-timer=240s, complete-timer-gate=240s, chains-to=888, fail-restart=886

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **275**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1388** (proximity/talk).
- Grant timer **240**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **240**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 887
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 887
- `iNPC_ID` = runtime ID from terminator NPC 1388
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Fill holes left by Tech Tunnelers and Doom Drones.

## Task 888

**Tags:** grant-timer=180s, complete-timer-gate=180s, fail-restart=886

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **275**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1389** (proximity/talk).
- Grant timer **180**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **180**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **203** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 888
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 888
- `iNPC_ID` = runtime ID from terminator NPC 1389
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Fill holes left by Tech Tunnelers and Doom Drones.
