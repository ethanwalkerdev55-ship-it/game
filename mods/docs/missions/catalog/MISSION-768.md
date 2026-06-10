# Mission 768 — Evil Takes Root (Part 1 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 768 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2685 | UseItems | — | — | 2686 | — | 3223 | 3245 |
| 2686 | UseItems | — | — | 2687 | — | — | 3246 |
| 2687 | UseItems | — | — | 2688 | — | — | 3247 |
| 2688 | Talk | — | — | — | — | — | 3223 |

## Chain edges (from table)

- Success: **2685** → **2686**
- Success: **2686** → **2687**
- Success: **2687** → **2688**

## Task 2685

**Tags:** chains-to=2686

### How this task is received

- Player accepts from NPC table ID **3223** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **3223** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3245** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2685
- `iNPC_ID` = runtime ID from grant NPC 3223
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2685
- `iNPC_ID` = runtime ID from terminator NPC 3245
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Gather samples from the Dark Tree.

## Task 2686

**Tags:** chains-to=2687

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3246** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2686
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2686
- `iNPC_ID` = runtime ID from terminator NPC 3246
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Gather samples from the Dark Tree.

## Task 2687

**Tags:** chains-to=2688

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3247** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2687
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2687
- `iNPC_ID` = runtime ID from terminator NPC 3247
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Gather samples from the Dark Tree.

## Task 2688

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3223** (proximity/talk).
- Reward table ID **680** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2688
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2688
- `iNPC_ID` = runtime ID from terminator NPC 3223
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Gather samples from the Dark Tree.
