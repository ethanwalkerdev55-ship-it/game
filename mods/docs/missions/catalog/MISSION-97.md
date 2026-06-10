# Mission 97 — Not-So-Secret Admirer

| Field | Value |
|-------|-------|
| Mission ID | 97 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 402 | UseItems | — | grant:420, gate:420 | 404 | 403 | 2562 | 866 |
| 403 | Talk | — | — | 402 | — | — | 2562 |
| 404 | Talk | — | — | — | — | — | 2562 |

## Chain edges (from table)

- Success: **402** → **404**
- Fail (err 1/12): **402** → **403**
- Success: **403** → **402**

## Task 402

**Tags:** grant-timer=420s, complete-timer-gate=420s, chains-to=404, fail-restart=403

### How this task is received

- Player accepts from NPC table ID **2562** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **96**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **2562** (journal accept or auto-chain).
- Complete at terminator NPC table ID **866** (proximity/talk).
- Grant timer **420**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **420**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 402
- `iNPC_ID` = runtime ID from grant NPC 2562
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 402
- `iNPC_ID` = runtime ID from terminator NPC 866
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver a love letter to Dee Dee from Mandark.

## Task 403

**Tags:** chains-to=402

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **96**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2562** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 403
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 403
- `iNPC_ID` = runtime ID from terminator NPC 2562
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver a love letter to Dee Dee from Mandark.

## Task 404

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **96**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2562** (proximity/talk).
- Reward table ID **97** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 404
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 404
- `iNPC_ID` = runtime ID from terminator NPC 2562
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Deliver a love letter to Dee Dee from Mandark.
