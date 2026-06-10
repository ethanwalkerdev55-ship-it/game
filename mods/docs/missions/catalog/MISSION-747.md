# Mission 747 — Jam Band (Part 4 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 747 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2606 | Defeat | — | — | 2607 | — | 3194 | — |
| 2607 | Talk | — | — | — | — | — | 3194 |

## Chain edges (from table)

- Success: **2606** → **2607**

## Task 2606

**Tags:** kill-quota, chains-to=2607

### How this task is received

- Player accepts from NPC table ID **3194** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **746**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **3194** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 2078 x7

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2606
- `iNPC_ID` = runtime ID from grant NPC 3194
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2606
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the Transmitters from the Horrordactyls.

## Task 2607

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **746**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3194** (proximity/talk).
- Reward table ID **658** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2607
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2607
- `iNPC_ID` = runtime ID from terminator NPC 3194
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Protect the Transmitters from the Horrordactyls.
