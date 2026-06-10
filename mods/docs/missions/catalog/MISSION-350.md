# Mission 350 — Stealing Secrets (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 350 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1766 | GotoLocation | — | — | 1767 | — | 730 | 2178 |
| 1767 | GotoLocation | — | — | 1768 | — | — | 2179 |
| 1768 | Defeat | 97 | — | — | 1766 | — | — |

## Chain edges (from table)

- Success: **1766** → **1767**
- Success: **1767** → **1768**
- Fail (err 1/12): **1768** → **1766**

## Task 1766

**Tags:** chains-to=1767

### How this task is received

- Player accepts from NPC table ID **730** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **349**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **730** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2178** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1766
- `iNPC_ID` = runtime ID from grant NPC 730
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1766
- `iNPC_ID` = runtime ID from terminator NPC 2178
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Fusion Max in Tech Square.

## Task 1767

**Tags:** chains-to=1768

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **349**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2179** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1767
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1767
- `iNPC_ID` = runtime ID from terminator NPC 2179
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Fusion Max in Tech Square.

## Task 1768

**Tags:** instance-id=97, kill-quota, fail-restart=1766

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **349**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=97**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 34 x1
- Reward table ID **410** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1768
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1768
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Find Fusion Max in Tech Square.
