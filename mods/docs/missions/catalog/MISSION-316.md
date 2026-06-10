# Mission 316 — How to Date a Fusion (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 316 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1297 | GotoLocation | — | — | 1298 | — | 719 | 922 |
| 1298 | GotoLocation | — | — | 1299 | — | — | 1658 |
| 1299 | Defeat | 62 | — | — | 1297 | — | — |

## Chain edges (from table)

- Success: **1297** → **1298**
- Success: **1298** → **1299**
- Fail (err 1/12): **1299** → **1297**

## Task 1297

**Tags:** chains-to=1298

### How this task is received

- Player accepts from NPC table ID **719** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **315**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **719** (journal accept or auto-chain).
- Complete at terminator NPC table ID **922** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1297
- `iNPC_ID` = runtime ID from grant NPC 719
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1297
- `iNPC_ID` = runtime ID from terminator NPC 922
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Enter the Secret Backstage Area.

## Task 1298

**Tags:** chains-to=1299

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **315**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1658** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1298
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1298
- `iNPC_ID` = runtime ID from terminator NPC 1658
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Enter the Secret Backstage Area.

## Task 1299

**Tags:** instance-id=62, kill-quota, fail-restart=1297

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **315**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=62**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 1712 x1
- Reward table ID **291** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1299
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1299
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Enter the Secret Backstage Area.
