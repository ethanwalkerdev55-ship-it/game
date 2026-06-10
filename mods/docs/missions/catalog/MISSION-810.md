# Mission 810 — All the Aliens (Part 5 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 810 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5090 | GotoLocation | — | — | 5091 | — | 688 | 1583 |
| 5091 | Talk | — | — | 5092 | — | — | 3121 |
| 5092 | Talk | — | — | — | — | — | 688 |

## Chain edges (from table)

- Success: **5090** → **5091**
- Success: **5091** → **5092**

## Task 5090

**Tags:** chains-to=5091

### How this task is received

- Player accepts from NPC table ID **688** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **688** (journal accept or auto-chain).
- Complete at terminator NPC table ID **1583** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5090
- `iNPC_ID` = runtime ID from grant NPC 688
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5090
- `iNPC_ID` = runtime ID from terminator NPC 1583
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get Cheese’s creation item for the Nano Enhancement Project.

## Task 5091

**Tags:** chains-to=5092

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3121** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5091
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5091
- `iNPC_ID` = runtime ID from terminator NPC 3121
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get Cheese’s creation item for the Nano Enhancement Project.

## Task 5092

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **688** (proximity/talk).
- Reward table ID **659** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5092
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5092
- `iNPC_ID` = runtime ID from terminator NPC 688
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get Cheese’s creation item for the Nano Enhancement Project.
