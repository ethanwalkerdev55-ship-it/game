# Mission 827 — Echo Location

| Field | Value |
|-------|-------|
| Mission ID | 827 |
| Mission type | Normal |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5151 | GotoLocation | — | — | 5152 | — | 697 | 2978 |
| 5152 | GotoLocation | — | — | 5153 | — | — | 2794 |
| 5153 | GotoLocation | — | — | 5154 | — | — | 2977 |
| 5154 | Talk | — | — | — | — | — | 697 |

## Chain edges (from table)

- Success: **5151** → **5152**
- Success: **5152** → **5153**
- Success: **5153** → **5154**

## Task 5151

**Tags:** chains-to=5152

### How this task is received

- Player accepts from NPC table ID **697** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **826**.
- Prerequisite completed mission **825**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **697** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2978** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5151
- `iNPC_ID` = runtime ID from grant NPC 697
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5151
- `iNPC_ID` = runtime ID from terminator NPC 2978
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate sonic waves in Hero’s Hollow.

## Task 5152

**Tags:** chains-to=5153

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2794** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5152
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5152
- `iNPC_ID` = runtime ID from terminator NPC 2794
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate sonic waves in Hero’s Hollow.

## Task 5153

**Tags:** chains-to=5154

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2977** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5153
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5153
- `iNPC_ID` = runtime ID from terminator NPC 2977
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate sonic waves in Hero’s Hollow.

## Task 5154

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **697** (proximity/talk).
- Reward table ID **31** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5154
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5154
- `iNPC_ID` = runtime ID from terminator NPC 697
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate sonic waves in Hero’s Hollow.
