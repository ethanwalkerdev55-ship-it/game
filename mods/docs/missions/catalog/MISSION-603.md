# Mission 603 — The Book of Prophecy (Part 3 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 603 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2309 | Talk | — | — | 2310 | — | 738 | 735 |
| 2310 | Talk | — | — | — | — | — | 739 |

## Chain edges (from table)

- Success: **2309** → **2310**

## Task 2309

**Tags:** chains-to=2310

### How this task is received

- Player accepts from NPC table ID **738** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **602**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **738** (journal accept or auto-chain).
- Complete at terminator NPC table ID **735** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2309
- `iNPC_ID` = runtime ID from grant NPC 738
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2309
- `iNPC_ID` = runtime ID from terminator NPC 735
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get the Book of Prophecy from May Kanker.

## Task 2310

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **602**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **739** (proximity/talk).
- Reward table ID **551** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2310
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2310
- `iNPC_ID` = runtime ID from terminator NPC 739
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get the Book of Prophecy from May Kanker.
