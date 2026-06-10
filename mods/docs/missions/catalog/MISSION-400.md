# Mission 400 — Control Center Assault (Part 4 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 400 |
| Mission type | Normal |
| Task count | 7 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1861 | GotoLocation | — | — | 1862 | — | 751 | 2240 |
| 1862 | GotoLocation | — | — | 1863 | — | — | 2241 |
| 1863 | Talk | 112 | — | 1864 | 1861 | — | 2283 |
| 1864 | Defeat | 112 | — | 1865 | 1861 | — | — |
| 1865 | Talk | 112 | — | 1866 | 1861 | — | 2283 |
| 1866 | GotoLocation | 112 | — | 1867 | 1867 | — | 2240 |
| 1867 | Talk | — | — | — | — | — | 751 |

## Chain edges (from table)

- Success: **1861** → **1862**
- Success: **1862** → **1863**
- Success: **1863** → **1864**
- Fail (err 1/12): **1863** → **1861**
- Success: **1864** → **1865**
- Fail (err 1/12): **1864** → **1861**
- Success: **1865** → **1866**
- Fail (err 1/12): **1865** → **1861**
- Success: **1866** → **1867**
- Fail (err 1/12): **1866** → **1867**

## Task 1861

**Tags:** chains-to=1862

### How this task is received

- Player accepts from NPC table ID **751** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **751** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2240** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1861
- `iNPC_ID` = runtime ID from grant NPC 751
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1861
- `iNPC_ID` = runtime ID from terminator NPC 2240
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.

## Task 1862

**Tags:** chains-to=1863

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2241** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1862
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1862
- `iNPC_ID` = runtime ID from terminator NPC 2241
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.

## Task 1863

**Tags:** instance-id=112, chains-to=1864, fail-restart=1861

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2283** (proximity/talk).
- Instance required (**m_iRequireInstanceID=112**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1863
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1863
- `iNPC_ID` = runtime ID from terminator NPC 2283
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.

## Task 1864

**Tags:** instance-id=112, kill-quota, chains-to=1865, fail-restart=1861

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=112**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 49 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1864
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1864
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.

## Task 1865

**Tags:** instance-id=112, chains-to=1866, fail-restart=1861

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2283** (proximity/talk).
- Instance required (**m_iRequireInstanceID=112**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1865
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1865
- `iNPC_ID` = runtime ID from terminator NPC 2283
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.

## Task 1866

**Tags:** instance-id=112, chains-to=1867, fail-restart=1867

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2240** (proximity/talk).
- Instance required (**m_iRequireInstanceID=112**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1866
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1866
- `iNPC_ID` = runtime ID from terminator NPC 2240
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.

## Task 1867

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **399**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **751** (proximity/talk).
- Reward table ID **424** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1867
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1867
- `iNPC_ID` = runtime ID from terminator NPC 751
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Assault the final Fusion Control Center with Eduardo.
