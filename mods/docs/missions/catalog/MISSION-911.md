# Mission 911 — Knishmas Kapers (Part 4 of 8)

| Field | Value |
|-------|-------|
| Mission ID | 911 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2719 | UseItems | — | — | 2720 | — | 3148 | 719 |
| 2720 | Talk | — | — | — | — | — | 734 |

## Chain edges (from table)

- Success: **2719** → **2720**

## Task 2719

**Tags:** chains-to=2720

### How this task is received

- Player accepts from NPC table ID **3148** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **909**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **3148** (journal accept or auto-chain).
- Complete at terminator NPC table ID **719** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2719
- `iNPC_ID` = runtime ID from grant NPC 3148
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2719
- `iNPC_ID` = runtime ID from terminator NPC 719
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Search for the present thief in Marquee Row

## Task 2720

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **734** (proximity/talk).
- Reward table ID **693** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2720
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2720
- `iNPC_ID` = runtime ID from terminator NPC 734
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Search for the present thief in Marquee Row
