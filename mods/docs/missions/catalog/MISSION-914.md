# Mission 914 — Knishmas Kapers (Part 6 of 8)

| Field | Value |
|-------|-------|
| Mission ID | 914 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2725 | UseItems | — | — | 2726 | — | 3148 | 3305 |
| 2726 | Talk | — | — | — | — | — | 748 |

## Chain edges (from table)

- Success: **2725** → **2726**

## Task 2725

**Tags:** chains-to=2726

### How this task is received

- Player accepts from NPC table ID **3148** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **913**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **3148** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3305** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2725
- `iNPC_ID` = runtime ID from grant NPC 3148
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2725
- `iNPC_ID` = runtime ID from terminator NPC 3305
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the present thief in The Ruins

## Task 2726

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **748** (proximity/talk).
- Reward table ID **696** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2726
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2726
- `iNPC_ID` = runtime ID from terminator NPC 748
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the present thief in The Ruins
