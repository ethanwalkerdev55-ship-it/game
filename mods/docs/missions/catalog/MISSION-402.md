# Mission 402 — Fuse No More (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 402 |
| Mission type | Normal |
| Task count | 6 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2459 | GotoLocation | — | — | 2460 | — | 752 | 2730 |
| 2460 | GotoLocation | — | — | 2461 | — | — | 2731 |
| 2461 | Defeat | 127 | — | 2462 | 2459 | — | — |
| 2462 | Talk | 127 | — | 2463 | 2459 | — | 2729 |
| 2463 | UseItems | 127 | — | 2464 | 2459 | — | 2732 |
| 2464 | Talk | — | — | — | — | — | 752 |

## Chain edges (from table)

- Success: **2459** → **2460**
- Success: **2460** → **2461**
- Success: **2461** → **2462**
- Fail (err 1/12): **2461** → **2459**
- Success: **2462** → **2463**
- Fail (err 1/12): **2462** → **2459**
- Success: **2463** → **2464**
- Fail (err 1/12): **2463** → **2459**

## Task 2459

**Tags:** chains-to=2460

### How this task is received

- Player accepts from NPC table ID **752** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **401**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **752** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2730** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2459
- `iNPC_ID` = runtime ID from grant NPC 752
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2459
- `iNPC_ID` = runtime ID from terminator NPC 2730
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a way past Fuse's Shield.

## Task 2460

**Tags:** chains-to=2461

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **401**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2731** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2460
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2460
- `iNPC_ID` = runtime ID from terminator NPC 2731
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a way past Fuse's Shield.

## Task 2461

**Tags:** instance-id=127, kill-quota, chains-to=2462, fail-restart=2459

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **401**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=127**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 2464 x8

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2461
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2461
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a way past Fuse's Shield.

## Task 2462

**Tags:** instance-id=127, chains-to=2463, fail-restart=2459

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **401**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2729** (proximity/talk).
- Instance required (**m_iRequireInstanceID=127**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2462
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2462
- `iNPC_ID` = runtime ID from terminator NPC 2729
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a way past Fuse's Shield.

## Task 2463

**Tags:** instance-id=127, chains-to=2464, fail-restart=2459

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **401**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2732** (proximity/talk).
- Instance required (**m_iRequireInstanceID=127**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2463
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2463
- `iNPC_ID` = runtime ID from terminator NPC 2732
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a way past Fuse's Shield.

## Task 2464

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **401**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **752** (proximity/talk).
- Reward table ID **621** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2464
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2464
- `iNPC_ID` = runtime ID from terminator NPC 752
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find a way past Fuse's Shield.
