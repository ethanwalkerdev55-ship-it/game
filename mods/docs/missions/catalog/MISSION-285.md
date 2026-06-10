# Mission 285 — One Spooky Fusion (Part 3 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 285 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 910 | GotoLocation | — | — | 912 | — | 711 | 1359 |
| 911 | GotoLocation | — | — | 912 | — | — | 1359 |
| 912 | UseItems | — | grant:240, gate:240 | — | 911 | — | 1394 |

## Chain edges (from table)

- Success: **910** → **912**
- Success: **911** → **912**
- Fail (err 1/12): **912** → **911**

## Task 910

**Tags:** chains-to=912

### How this task is received

- Player accepts from NPC table ID **711** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **284**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **711** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1359** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 910
- `iNPC_ID` = runtime ID from grant NPC 711
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 910
- `iNPC_ID` = runtime ID from terminator NPC 1359
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Clean up Eternal Meadows.

## Task 911

**Tags:** chains-to=912

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **284**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1359** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 911
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 911
- `iNPC_ID` = runtime ID from terminator NPC 1359
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Clean up Eternal Meadows.

## Task 912

**Tags:** grant-timer=240s, complete-timer-gate=240s, fail-restart=911

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **284**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1394** (proximity/talk).
- Grant timer **240**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **240**: `CheckToCompleteTaskCondition` returns Fail until elapsed.
- Reward table ID **211** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 912
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 912
- `iNPC_ID` = runtime ID from terminator NPC 1394
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Clean up Eternal Meadows.
