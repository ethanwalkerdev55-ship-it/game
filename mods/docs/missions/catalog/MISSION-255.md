# Mission 255 — Helping Numbuh Two (Part 3 of 3)

| Field | Value |
|-------|-------|
| Mission ID | 255 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 732 | Defeat | — | — | 733 | — | 704 | — |
| 733 | UseItems | — | — | — | — | — | 1186 |

## Chain edges (from table)

- Success: **732** → **733**

## Task 732

**Tags:** kill-quota, item-quota, chains-to=733

### How this task is received

- Player accepts from NPC table ID **704** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **254**.

### How this task must be completed

- Task type: **Defeat**
- Start at grant NPC table ID **704** (journal accept or auto-chain).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 275 x0
- Collect items: item 204 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 732
- `iNPC_ID` = runtime ID from grant NPC 704
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 732
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair the SCAMPER.

## Task 733

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **254**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **1186** (proximity/talk).
- Reward table ID **173** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 733
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 733
- `iNPC_ID` = runtime ID from terminator NPC 1186
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Help Numbuh Two repair the SCAMPER.
