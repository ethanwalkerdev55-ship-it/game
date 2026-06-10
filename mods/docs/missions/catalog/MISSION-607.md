# Mission 607 — A Rune with a View (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 607 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2159 | Defeat | — | — | 2160 | — | 734 | — |
| 2160 | Talk | — | — | — | — | — | 734 |

## Chain edges (from table)

- Success: **2159** → **2160**

## Task 2159

**Tags:** kill-quota, item-quota, chains-to=2160

### How this task is received

- Player accepts from NPC table ID **734** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **606**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **734** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 167 x0
- Collect items: item 535 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2159
- `iNPC_ID` = runtime ID from grant NPC 734
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2159
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Grim's Portal Protector.

## Task 2160

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **606**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **734** (proximity/talk).
- Reward table ID **497** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2160
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2160
- `iNPC_ID` = runtime ID from terminator NPC 734
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover Grim's Portal Protector.
