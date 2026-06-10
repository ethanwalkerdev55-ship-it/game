# Mission 656 — Defend the Defenses (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 656 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2175 | Talk | — | — | 2176 | — | 729 | 733 |
| 2176 | Talk | — | — | — | — | — | 729 |

## Chain edges (from table)

- Success: **2175** → **2176**

## Task 2175

**Tags:** chains-to=2176

### How this task is received

- Player accepts from NPC table ID **729** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **655**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **729** (journal accept or auto-chain).
- Complete at terminator NPC table ID **733** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2175
- `iNPC_ID` = runtime ID from grant NPC 729
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2175
- `iNPC_ID` = runtime ID from terminator NPC 733
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the Surface-to-Orbit Missile Array.

## Task 2176

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **655**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **729** (proximity/talk).
- Reward table ID **505** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2176
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2176
- `iNPC_ID` = runtime ID from terminator NPC 729
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect the Surface-to-Orbit Missile Array.
