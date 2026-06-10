# Mission 132 — Plant Plan

| Field | Value |
|-------|-------|
| Mission ID | 132 |
| Mission type | Guide |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1687 | Defeat | — | — | 1688 | — | 746 | — |
| 1688 | UseItems | — | — | — | — | — | 2207 |

## Chain edges (from table)

- Success: **1687** → **1688**

## Task 1687

**Tags:** kill-quota, chains-to=1688

### How this task is received

- Player accepts from NPC table ID **746** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **165**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **746** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2059 x6

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1687
- `iNPC_ID` = runtime ID from grant NPC 746
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1687
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Locate the Crazy Brain Candy.

## Task 1688

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **165**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2207** (proximity/talk).
- Reward table ID **386** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1688
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1688
- `iNPC_ID` = runtime ID from terminator NPC 2207
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Locate the Crazy Brain Candy.
