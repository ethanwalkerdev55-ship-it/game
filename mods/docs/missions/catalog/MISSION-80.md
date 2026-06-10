# Mission 80 — Bloo is Green

| Field | Value |
|-------|-------|
| Mission ID | 80 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 318 | GotoLocation | — | — | 319 | — | 726 | 905 |
| 319 | Defeat | — | — | 320 | — | — | — |
| 320 | UseItems | — | — | — | — | — | 726 |

## Chain edges (from table)

- Success: **318** → **319**
- Success: **319** → **320**

## Task 318

**Tags:** chains-to=319

### How this task is received

- Player accepts from NPC table ID **726** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **79**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **726** (journal accept or auto-chain).
- Complete at terminator NPC table ID **905** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 318
- `iNPC_ID` = runtime ID from grant NPC 726
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 318
- `iNPC_ID` = runtime ID from terminator NPC 905
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect some soda pop for Bloo's upset tummy.

## Task 319

**Tags:** kill-quota, item-quota, chains-to=320

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **79**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 180 x0
- Collect items: item 101 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 319
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 319
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect some soda pop for Bloo's upset tummy.

## Task 320

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **79**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **726** (proximity/talk).
- Reward table ID **80** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 320
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 320
- `iNPC_ID` = runtime ID from terminator NPC 726
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect some soda pop for Bloo's upset tummy.
