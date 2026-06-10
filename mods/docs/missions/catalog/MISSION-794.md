# Mission 794 — All Hail the Queen

| Field | Value |
|-------|-------|
| Mission ID | 794 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5035 | Talk | — | — | 5036 | — | 3302 | 3312 |
| 5036 | Defeat | — | — | 5037 | — | — | — |
| 5037 | Talk | — | — | — | — | — | 3301 |

## Chain edges (from table)

- Success: **5035** → **5036**
- Success: **5036** → **5037**

## Task 5035

**Tags:** chains-to=5036

### How this task is received

- Player accepts from NPC table ID **3302** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **793**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3302** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3312** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5035
- `iNPC_ID` = runtime ID from grant NPC 3302
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5035
- `iNPC_ID` = runtime ID from terminator NPC 3312
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the Blood Gnat Queen somewhere along the river.

## Task 5036

**Tags:** kill-quota, chains-to=5037

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 3419 x10

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5036
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5036
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the Blood Gnat Queen somewhere along the river.

## Task 5037

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3301** (proximity/talk).
- Reward table ID **708** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5037
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5037
- `iNPC_ID` = runtime ID from terminator NPC 3301
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the Blood Gnat Queen somewhere along the river.
