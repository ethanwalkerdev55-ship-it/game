# Mission 478 — Toiletnator's Tomfoolery (Part 3 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 478 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2276 | Defeat | — | — | 2277 | — | 741 | — |
| 2277 | Talk | — | — | — | — | — | 741 |

## Chain edges (from table)

- Success: **2276** → **2277**

## Task 2276

**Tags:** kill-quota, chains-to=2277

### How this task is received

- Player accepts from NPC table ID **741** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **477**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **741** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 161 x10

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2276
- `iNPC_ID` = runtime ID from grant NPC 741
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2276
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop the Rabid Arachnids.

## Task 2277

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **477**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **741** (proximity/talk).
- Reward table ID **539** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2277
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2277
- `iNPC_ID` = runtime ID from terminator NPC 741
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Stop the Rabid Arachnids.
