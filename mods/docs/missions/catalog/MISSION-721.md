# Mission 721 — Faux Coco (Part 2 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 721 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2524 | Defeat | — | — | 2525 | — | 742 | — |
| 2525 | Talk | — | — | — | — | — | 742 |

## Chain edges (from table)

- Success: **2524** → **2525**

## Task 2524

**Tags:** kill-quota, item-quota, chains-to=2525

### How this task is received

- Player accepts from NPC table ID **742** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **720**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **742** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 388 x0
- Collect items: item 553 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2524
- `iNPC_ID` = runtime ID from grant NPC 742
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2524
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Keep working on the Coco decoy.

## Task 2525

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **720**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **742** (proximity/talk).
- Reward table ID **639** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2525
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2525
- `iNPC_ID` = runtime ID from terminator NPC 742
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Keep working on the Coco decoy.
