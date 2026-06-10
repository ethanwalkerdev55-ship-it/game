# Mission 622 — The Power of Prophecy (Part 2 of 4)

| Field | Value |
|-------|-------|
| Mission ID | 622 |
| Mission type | Normal |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 2161 | UseItems | — | — | 2162 | — | 733 | 2604 |
| 2162 | UseItems | — | — | — | — | — | 2605 |

## Chain edges (from table)

- Success: **2161** → **2162**

## Task 2161

**Tags:** chains-to=2162

### How this task is received

- Player accepts from NPC table ID **733** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **621**.

### How this task must be completed

- Task type: **UseItems**
- Start at grant NPC table ID **733** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2604** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2161
- `iNPC_ID` = runtime ID from grant NPC 733
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2161
- `iNPC_ID` = runtime ID from terminator NPC 2604
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Activate the totem in Steam Alley.

## Task 2162

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **621**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2605** (proximity/talk).
- Reward table ID **498** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 2162
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 2162
- `iNPC_ID` = runtime ID from terminator NPC 2605
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Activate the totem in Steam Alley.
