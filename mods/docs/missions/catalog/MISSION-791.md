# Mission 791 — Infestation Situation

| Field | Value |
|-------|-------|
| Mission ID | 791 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5026 | Talk | — | — | 5027 | — | 3301 | 3150 |
| 5027 | Talk | — | — | — | — | — | 3302 |

## Chain edges (from table)

- Success: **5026** → **5027**

## Task 5026

**Tags:** chains-to=5027

### How this task is received

- Player accepts from NPC table ID **3301** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **3301** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3150** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5026
- `iNPC_ID` = runtime ID from grant NPC 3301
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5026
- `iNPC_ID` = runtime ID from terminator NPC 3150
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Flapjack

## Task 5027

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3302** (proximity/talk).
- Reward table ID **705** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5027
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5027
- `iNPC_ID` = runtime ID from terminator NPC 3302
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Talk to Flapjack
