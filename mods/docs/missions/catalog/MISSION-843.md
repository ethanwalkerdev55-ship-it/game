# Mission 843 — Nostalgia Chip

| Field | Value |
|-------|-------|
| Mission ID | 843 |
| Mission type | Nano |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5234 | Talk | — | — | 5235 | — | 730 | 729 |
| 5235 | GotoLocation | — | — | 5236 | — | — | 3343 |
| 5236 | EscortDefence | 150 | — | 5237 | 5235 | — | — |
| 5237 | EscortDefence | 150 | — | — | 5235 | — | — |

## Chain edges (from table)

- Success: **5234** → **5235**
- Success: **5235** → **5236**
- Success: **5236** → **5237**
- Fail (err 1/12): **5236** → **5235**
- Fail (err 1/12): **5237** → **5235**

## Task 5234

**Tags:** chains-to=5235

### How this task is received

- Player accepts from NPC table ID **730** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **421**.
- Prerequisite completed mission **345**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **730** (journal accept or auto-chain).
- Complete at terminator NPC table ID **729** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5234
- `iNPC_ID` = runtime ID from grant NPC 730
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5234
- `iNPC_ID` = runtime ID from terminator NPC 729
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate a corrupted memory chip containing confidential data on Dexter’s old lab.

## Task 5235

**Tags:** chains-to=5236

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3343** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5235
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5235
- `iNPC_ID` = runtime ID from terminator NPC 3343
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate a corrupted memory chip containing confidential data on Dexter’s old lab.

## Task 5236

**Tags:** instance-id=150, kill-quota, escort, chains-to=5237, fail-restart=5235

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **EscortDefence**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=150**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 3340 x35
- Escort NPC table ID **3342**; death → `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5236
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5236
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate a corrupted memory chip containing confidential data on Dexter’s old lab.

## Task 5237

**Tags:** instance-id=150, kill-quota, escort, fail-restart=5235

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **EscortDefence**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=150**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 3341 x1
- Escort NPC table ID **3342**; death → `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5237
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5237
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate a corrupted memory chip containing confidential data on Dexter’s old lab.
