# Mission 792 — Blood Gnat Spat

| Field | Value |
|-------|-------|
| Mission ID | 792 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5028 | Defeat | — | — | 5029 | 5029 | 3302 | — |
| 5029 | Talk | — | — | — | — | — | 3302 |

## Chain edges (from table)

- Success: **5028** → **5029**
- Fail (err 1/12): **5028** → **5029**

## Task 5028

**Tags:** kill-quota, chains-to=5029, fail-restart=5029

### How this task is received

- Player accepts from NPC table ID **3302** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **791**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **3302** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 3419 x20

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5028
- `iNPC_ID` = runtime ID from grant NPC 3302
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5028
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Killer Blood Gnats

## Task 5029

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3302** (proximity/talk).
- Reward table ID **706** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5029
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5029
- `iNPC_ID` = runtime ID from terminator NPC 3302
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat Killer Blood Gnats
