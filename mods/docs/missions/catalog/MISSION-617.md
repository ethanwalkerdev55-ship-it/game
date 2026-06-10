# Mission 617 — Non-Prophet Organization (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 617 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2337 | UseItems | — | — | 2338 | — | 745 | 2832 |
| 2338 | Talk | — | — | — | — | — | 745 |

## Chain edges (from table)

- Success: **2337** → **2338**

## Task 2337

**Tags:** chains-to=2338

### How this task is received

- Player accepts from NPC table ID **745** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **616**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **745** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2832** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2337
- `iNPC_ID` = runtime ID from grant NPC 745
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2337
- `iNPC_ID` = runtime ID from terminator NPC 2832
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Hex about the prophecy.

## Task 2338

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **616**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **745** (proximity/talk).
- Reward table ID **564** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2338
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2338
- `iNPC_ID` = runtime ID from terminator NPC 745
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Hex about the prophecy.
