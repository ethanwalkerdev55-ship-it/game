# Mission 641 — Demonic Business (Part 3 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 641 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2379 | Defeat | — | — | 2380 | — | 751 | — |
| 2380 | Talk | — | — | — | — | — | 751 |

## Chain edges (from table)

- Success: **2379** → **2380**

## Task 2379

**Tags:** kill-quota, chains-to=2380

### How this task is received

- Player accepts from NPC table ID **751** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **640**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **751** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2082 x9

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2379
- `iNPC_ID` = runtime ID from grant NPC 751
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2379
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Fight the Fusionflies.

## Task 2380

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **640**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **751** (proximity/talk).
- Reward table ID **584** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2380
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2380
- `iNPC_ID` = runtime ID from terminator NPC 751
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Fight the Fusionflies.
