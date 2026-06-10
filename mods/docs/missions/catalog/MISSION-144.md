# Mission 144 — Move Over, Mandy

| Field | Value |
|-------|-------|
| Mission ID | 144 |
| Mission type | Guide |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1725 | Defeat | — | — | 1726 | — | 744 | — |
| 1726 | Talk | — | — | — | — | — | 744 |

## Chain edges (from table)

- Success: **1725** → **1726**

## Task 1725

**Tags:** kill-quota, chains-to=1726

### How this task is received

- Player accepts from NPC table ID **744** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **142**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **744** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 1989 x10

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1725
- `iNPC_ID` = runtime ID from grant NPC 744
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1725
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Continue search for the Hypnotic Charm.

## Task 1726

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **142**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **744** (proximity/talk).
- Reward table ID **399** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1726
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1726
- `iNPC_ID` = runtime ID from terminator NPC 744
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Continue search for the Hypnotic Charm.
