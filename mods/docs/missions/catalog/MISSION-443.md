# Mission 443 — Hairy Chest Test

| Field | Value |
|-------|-------|
| Mission ID | 443 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1133 | Defeat | — | — | 1134 | — | 758 | — |
| 1134 | Delivery | — | — | — | — | — | 758 |

## Chain edges (from table)

- Success: **1133** → **1134**

## Task 1133

**Tags:** kill-quota, item-quota, chains-to=1134

### How this task is received

- Player accepts from NPC table ID **758** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **758** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 283 x0
- Collect items: item 285 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1133
- `iNPC_ID` = runtime ID from grant NPC 758
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1133
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get a throwning star from a Newspaper Ninja.

## Task 1134

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **758** (proximity/talk).
- Collect items: item 285 x1
- Reward table ID **265** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1134
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1134
- `iNPC_ID` = runtime ID from terminator NPC 758
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get a throwning star from a Newspaper Ninja.
