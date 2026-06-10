# Mission 762 — Overclocking 
(Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 762 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2666 | Talk | — | — | 2667 | — | 3233 | 2148 |
| 2667 | Defeat | — | — | 2668 | — | — | — |
| 2668 | Talk | — | — | — | — | — | 3233 |

## Chain edges (from table)

- Success: **2666** → **2667**
- Success: **2667** → **2668**

## Task 2666

**Tags:** chains-to=2667

### How this task is received

- Player accepts from NPC table ID **3233** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **761**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3233** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2148** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2666
- `iNPC_ID` = runtime ID from grant NPC 3233
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2666
- `iNPC_ID` = runtime ID from terminator NPC 2148
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Remove the Personality Chip.

## Task 2667

**Tags:** kill-quota, item-quota, chains-to=2668

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **761**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2052 x0
- Collect items: item 562 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2667
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2667
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Remove the Personality Chip.

## Task 2668

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **761**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3233** (proximity/talk).
- Reward table ID **674** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2668
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2668
- `iNPC_ID` = runtime ID from terminator NPC 3233
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Remove the Personality Chip.
