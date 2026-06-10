# Mission 701 — Arrrmed Forces (Part 2 of 6)

| Field | Value |
|-------|-------|
| Mission ID | 701 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2478 | Talk | — | — | 2479 | — | 2856 | 3090 |
| 2479 | Defeat | — | — | 2480 | — | — | — |
| 2480 | Talk | — | — | — | — | — | 3090 |

## Chain edges (from table)

- Success: **2478** → **2479**
- Success: **2479** → **2480**

## Task 2478

**Tags:** chains-to=2479

### How this task is received

- Player accepts from NPC table ID **2856** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **702**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **2856** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3090** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2478
- `iNPC_ID` = runtime ID from grant NPC 2856
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2478
- `iNPC_ID` = runtime ID from terminator NPC 3090
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Contact Sugarnose on Bravo Beach.

## Task 2479

**Tags:** kill-quota, chains-to=2480

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **702**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 366 x7

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2479
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2479
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Contact Sugarnose on Bravo Beach.

## Task 2480

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **702**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3090** (proximity/talk).
- Reward table ID **625** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2480
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2480
- `iNPC_ID` = runtime ID from terminator NPC 3090
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Contact Sugarnose on Bravo Beach.
