# Mission 916 — Knishmas Kapers (Part 8 of 8)

| Field | Value |
|-------|-------|
| Mission ID | 916 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2729 | UseItems | — | — | 2730 | — | 751 | 3307 |
| 2730 | Talk | — | — | — | — | — | 3148 |

## Chain edges (from table)

- Success: **2729** → **2730**

## Task 2729

**Tags:** chains-to=2730

### How this task is received

- Player accepts from NPC table ID **751** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **915**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **751** (journal accept or auto-chain).
- Complete at terminator NPC table ID **3307** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2729
- `iNPC_ID` = runtime ID from grant NPC 751
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2729
- `iNPC_ID` = runtime ID from terminator NPC 3307
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Give the gift to someone in need.

## Task 2730

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3148** (proximity/talk).
- Reward table ID **698** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2730
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2730
- `iNPC_ID` = runtime ID from terminator NPC 3148
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Give the gift to someone in need.
