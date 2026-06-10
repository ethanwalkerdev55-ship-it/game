# Mission 612 — Head to Totem (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 612 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2326 | Defeat | — | — | 2327 | — | 738 | — |
| 2327 | UseItems | — | — | — | — | — | 2827 |

## Chain edges (from table)

- Success: **2326** → **2327**

## Task 2326

**Tags:** kill-quota, chains-to=2327

### How this task is received

- Player accepts from NPC table ID **738** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **611**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **738** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 388 x7

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2326
- `iNPC_ID` = runtime ID from grant NPC 738
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2326
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Try to activate a totem in the Twisted Forest.

## Task 2327

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **611**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2827** (proximity/talk).
- Reward table ID **559** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2327
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2327
- `iNPC_ID` = runtime ID from terminator NPC 2827
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Try to activate a totem in the Twisted Forest.
