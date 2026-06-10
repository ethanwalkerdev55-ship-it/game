# Mission 398 — Control Center Assault (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 398 |
| Mission type | Normal |
| Task count | 5 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1849 | GotoLocation | — | — | 1850 | — | 752 | 2236 |
| 1850 | GotoLocation | — | — | 1851 | — | — | 2237 |
| 1851 | Defeat | 110 | — | 1852 | 1849 | — | — |
| 1852 | GotoLocation | 110 | — | 1853 | 1853 | — | 2236 |
| 1853 | Talk | — | — | — | — | — | 752 |

## Chain edges (from table)

- Success: **1849** → **1850**
- Success: **1850** → **1851**
- Success: **1851** → **1852**
- Fail (err 1/12): **1851** → **1849**
- Success: **1852** → **1853**
- Fail (err 1/12): **1852** → **1853**

## Task 1849

**Tags:** chains-to=1850

### How this task is received

- Player accepts from NPC table ID **752** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **397**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **752** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2236** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1849
- `iNPC_ID` = runtime ID from grant NPC 752
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1849
- `iNPC_ID` = runtime ID from terminator NPC 2236
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Raid Fusion Control Center 2 with Dexter.

## Task 1850

**Tags:** chains-to=1851

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **397**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2237** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1850
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1850
- `iNPC_ID` = runtime ID from terminator NPC 2237
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Raid Fusion Control Center 2 with Dexter.

## Task 1851

**Tags:** instance-id=110, kill-quota, chains-to=1852, fail-restart=1849

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **397**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=110**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 2034 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1851
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1851
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Raid Fusion Control Center 2 with Dexter.

## Task 1852

**Tags:** instance-id=110, chains-to=1853, fail-restart=1853

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **397**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2236** (proximity/talk).
- Instance required (**m_iRequireInstanceID=110**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1852
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1852
- `iNPC_ID` = runtime ID from terminator NPC 2236
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Raid Fusion Control Center 2 with Dexter.

## Task 1853

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **397**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **752** (proximity/talk).
- Reward table ID **422** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1853
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1853
- `iNPC_ID` = runtime ID from terminator NPC 752
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Raid Fusion Control Center 2 with Dexter.
