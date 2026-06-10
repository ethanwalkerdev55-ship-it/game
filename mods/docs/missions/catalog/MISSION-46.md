# Mission 46 — Major Worry

| Field | Value |
|-------|-------|
| Mission ID | 46 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 182 | GotoLocation | — | — | 183 | — | 703 | 912 |
| 183 | UseItems | — | — | 184 | — | — | 839 |
| 184 | Talk | — | — | — | — | — | 703 |

## Chain edges (from table)

- Success: **182** → **183**
- Success: **183** → **184**

## Task 182

**Tags:** chains-to=183

### How this task is received

- Player accepts from NPC table ID **703** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **149**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **703** (journal accept or auto-chain).
- Complete at terminator NPC table ID **912** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 182
- `iNPC_ID` = runtime ID from grant NPC 703
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 182
- `iNPC_ID` = runtime ID from terminator NPC 912
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Pokey Oaks for clues to Major Glory's location.

## Task 183

**Tags:** chains-to=184

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **149**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **839** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 183
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 183
- `iNPC_ID` = runtime ID from terminator NPC 839
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Pokey Oaks for clues to Major Glory's location.

## Task 184

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **149**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **703** (proximity/talk).
- Reward table ID **46** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 184
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 184
- `iNPC_ID` = runtime ID from terminator NPC 703
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Pokey Oaks for clues to Major Glory's location.
