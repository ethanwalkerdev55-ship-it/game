# Mission 365 — Treetop Troubles (Part 1 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 365 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1978 | Defeat | — | — | 1979 | — | 739 | — |
| 1979 | Delivery | — | — | — | — | — | 739 |

## Chain edges (from table)

- Success: **1978** → **1979**

## Task 1978

**Tags:** kill-quota, item-quota, chains-to=1979

### How this task is received

- Player accepts from NPC table ID **739** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **739** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 356 x0
- Collect items: item 501 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1978
- `iNPC_ID` = runtime ID from grant NPC 739
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1978
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Bladed Mantises.

## Task 1979

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **739** (proximity/talk).
- Collect items: item 501 x1
- Reward table ID **451** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1979
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1979
- `iNPC_ID` = runtime ID from terminator NPC 739
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Bladed Mantises.
