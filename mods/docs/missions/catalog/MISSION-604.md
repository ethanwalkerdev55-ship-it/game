# Mission 604 — The Book of Prophecy (Part 4 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 604 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2311 | Defeat | — | — | 2312 | — | 739 | — |
| 2312 | Talk | — | — | — | — | — | 738 |

## Chain edges (from table)

- Success: **2311** → **2312**

## Task 2311

**Tags:** kill-quota, item-quota, chains-to=2312

### How this task is received

- Player accepts from NPC table ID **739** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **603**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **739** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 174 x0
- Collect items: item 545 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2311
- `iNPC_ID` = runtime ID from grant NPC 739
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2311
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Ed defend the Jungle Outpost.

## Task 2312

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **603**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **738** (proximity/talk).
- Reward table ID **552** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2312
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2312
- `iNPC_ID` = runtime ID from terminator NPC 738
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Ed defend the Jungle Outpost.
