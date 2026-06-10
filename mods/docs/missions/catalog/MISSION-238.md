# Mission 238 — Hello From Camp Kidney! (Part 2 of 2)

| Field | Value |
|-------|-------|
| Mission ID | 238 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2216 | Talk | — | — | 2217 | — | 1907 | 736 |
| 2217 | Talk | — | — | — | — | — | 1907 |

## Chain edges (from table)

- Success: **2216** → **2217**

## Task 2216

**Tags:** chains-to=2217

### How this task is received

- Player accepts from NPC table ID **1907** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **1907** (journal accept or auto-chain).
- Complete at terminator NPC table ID **736** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2216
- `iNPC_ID` = runtime ID from grant NPC 1907
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2216
- `iNPC_ID` = runtime ID from terminator NPC 736
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Obtain Toiletnator's flush handle.

## Task 2217

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1907** (proximity/talk).
- Reward table ID **520** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2217
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2217
- `iNPC_ID` = runtime ID from terminator NPC 1907
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Obtain Toiletnator's flush handle.
