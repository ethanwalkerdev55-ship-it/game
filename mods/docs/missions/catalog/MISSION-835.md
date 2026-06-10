# Mission 835 — Time to Groove

| Field | Value |
|-------|-------|
| Mission ID | 835 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5184 | GotoLocation | — | — | 5185 | — | 692 | 3132 |
| 5185 | GotoLocation | — | — | 5186 | — | — | 637 |
| 5186 | Defeat | 141 | — | 5187 | 5185 | — | — |
| 5187 | GotoLocation | 141 | — | 5188 | 5188 | — | 3132 |
| 5188 | Talk | — | — | — | — | — | 692 |

## Chain edges (from table)

- Success: **5184** → **5185**
- Success: **5185** → **5186**
- Success: **5186** → **5187**
- Fail (err 1/12): **5186** → **5185**
- Success: **5187** → **5188**
- Fail (err 1/12): **5187** → **5188**

## Task 5184

**Tags:** chains-to=5185

### How this task is received

- Player accepts from NPC table ID **692** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **834**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **692** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3132** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5184
- `iNPC_ID` = runtime ID from grant NPC 692
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5184
- `iNPC_ID` = runtime ID from terminator NPC 3132
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Larry’s communicator conundrum and establish contact with Buck.

## Task 5185

**Tags:** chains-to=5186

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **637** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5185
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5185
- `iNPC_ID` = runtime ID from terminator NPC 637
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Larry’s communicator conundrum and establish contact with Buck.

## Task 5186

**Tags:** instance-id=141, kill-quota, chains-to=5187, fail-restart=5185

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=141**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 53 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5186
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5186
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Larry’s communicator conundrum and establish contact with Buck.

## Task 5187

**Tags:** instance-id=141, chains-to=5188, fail-restart=5188

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3132** (proximity/talk).
- Instance required (**m_iRequireInstanceID=141**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5187
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5187
- `iNPC_ID` = runtime ID from terminator NPC 3132
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Larry’s communicator conundrum and establish contact with Buck.

## Task 5188

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **692** (proximity/talk).
- Reward table ID **715** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5188
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5188
- `iNPC_ID` = runtime ID from terminator NPC 692
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Larry’s communicator conundrum and establish contact with Buck.
