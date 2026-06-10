# Mission 772 — Board Meeting 
(Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 772 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2699 | Talk | — | — | 2700 | — | 3222 | 3223 |
| 2700 | Talk | — | — | — | — | — | 3222 |

## Chain edges (from table)

- Success: **2699** → **2700**

## Task 2699

**Tags:** chains-to=2700

### How this task is received

- Player accepts from NPC table ID **3222** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **771**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3222** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3223** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2699
- `iNPC_ID` = runtime ID from grant NPC 3222
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2699
- `iNPC_ID` = runtime ID from terminator NPC 3223
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh 2642 build his Lemonade Stand.

## Task 2700

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **771**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3222** (proximity/talk).
- Reward table ID **684** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2700
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2700
- `iNPC_ID` = runtime ID from terminator NPC 3222
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh 2642 build his Lemonade Stand.
