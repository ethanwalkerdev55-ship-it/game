# Mission 795 — Pest Control

| Field | Value |
|-------|-------|
| Mission ID | 795 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5038 | Defeat | — | — | 5039 | — | 3301 | — |
| 5039 | Talk | — | — | — | — | — | 3301 |

## Chain edges (from table)

- Success: **5038** → **5039**

## Task 5038

**Tags:** kill-quota, chains-to=5039

### How this task is received

- Player accepts from NPC table ID **3301** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **794**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **3301** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 3419 x100

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5038
- `iNPC_ID` = runtime ID from grant NPC 3301
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5038
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat the Killer Blood Gnats

## Task 5039

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3301** (proximity/talk).
- Reward table ID **709** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5039
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5039
- `iNPC_ID` = runtime ID from terminator NPC 3301
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat the Killer Blood Gnats
