# Mission 417 — Double Fusion Trouble (Part 1 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 417 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1040 | GotoLocation | — | — | 1041 | — | 705 | 1384 |
| 1041 | Talk | — | — | — | — | — | 1192 |

## Chain edges (from table)

- Success: **1040** → **1041**

## Task 1040

**Tags:** chains-to=1041

### How this task is received

- Player accepts from NPC table ID **705** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **705** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1384** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1040
- `iNPC_ID` = runtime ID from grant NPC 705
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1040
- `iNPC_ID` = runtime ID from terminator NPC 1384
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the Dexbot at the Nuclear Plant.

## Task 1041

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1192** (proximity/talk).
- Reward table ID **241** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1041
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1041
- `iNPC_ID` = runtime ID from terminator NPC 1192
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find the Dexbot at the Nuclear Plant.
