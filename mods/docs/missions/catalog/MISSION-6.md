# Mission 6 — Dee Dee's Dilemmas (Part 4 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 6 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 47 | GotoLocation | — | — | 48 | — | 701 | 873 |
| 48 | Defeat | — | — | 49 | — | — | — |
| 49 | Delivery | — | — | — | — | — | 705 |

## Chain edges (from table)

- Success: **47** → **48**
- Success: **48** → **49**

## Task 47

**Tags:** chains-to=48

### How this task is received

- Player accepts from NPC table ID **701** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **5**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **701** (journal accept or auto-chain).
- Complete at terminator NPC table ID **873** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 47
- `iNPC_ID` = runtime ID from grant NPC 701
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 47
- `iNPC_ID` = runtime ID from terminator NPC 873
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat the monsters in Genius Grove.

## Task 48

**Tags:** kill-quota, chains-to=49

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **5**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 293 x6

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 48
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 48
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat the monsters in Genius Grove.

## Task 49

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **5**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **705** (proximity/talk).
- Collect items: item 35 x1
- Reward table ID **15** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 49
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 49
- `iNPC_ID` = runtime ID from terminator NPC 705
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Defeat the monsters in Genius Grove.
