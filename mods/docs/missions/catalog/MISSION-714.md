# Mission 714 — SACT Attack (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 714 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2498 | UseItems | — | — | 2499 | — | 1908 | 3117 |
| 2499 | Defeat | — | — | 2500 | — | — | — |
| 2500 | Talk | — | — | — | — | — | 1908 |

## Chain edges (from table)

- Success: **2498** → **2499**
- Success: **2499** → **2500**

## Task 2498

**Tags:** chains-to=2499

### How this task is received

- Player accepts from NPC table ID **1908** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **713**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **1908** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3117** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2498
- `iNPC_ID` = runtime ID from grant NPC 1908
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2498
- `iNPC_ID` = runtime ID from terminator NPC 3117
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help recover a meteor sample for SACT.

## Task 2499

**Tags:** kill-quota, chains-to=2500

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **713**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 184 x6

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2499
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2499
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help recover a meteor sample for SACT.

## Task 2500

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **713**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1908** (proximity/talk).
- Reward table ID **632** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2500
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2500
- `iNPC_ID` = runtime ID from terminator NPC 1908
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help recover a meteor sample for SACT.
