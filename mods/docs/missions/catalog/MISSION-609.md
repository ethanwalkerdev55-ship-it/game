# Mission 609 — A Rune with a View (Part 4 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 609 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2320 | Defeat | — | — | 2321 | — | 1908 | — |
| 2321 | Talk | — | — | — | — | — | 1908 |

## Chain edges (from table)

- Success: **2320** → **2321**

## Task 2320

**Tags:** kill-quota, item-quota, chains-to=2321

### How this task is received

- Player accepts from NPC table ID **1908** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **608**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **1908** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 184 x0
- Collect items: item 546 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2320
- `iNPC_ID` = runtime ID from grant NPC 1908
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2320
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the pages of the Book of Prophecy.

## Task 2321

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **608**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1908** (proximity/talk).
- Reward table ID **556** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2321
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2321
- `iNPC_ID` = runtime ID from terminator NPC 1908
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the pages of the Book of Prophecy.
