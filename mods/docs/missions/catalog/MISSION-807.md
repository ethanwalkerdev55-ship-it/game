# Mission 807 — All the Aliens (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 807 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5081 | GotoLocation | — | — | 5082 | — | 688 | 2512 |
| 5082 | Talk | — | — | 5083 | — | — | 699 |
| 5083 | Talk | — | — | — | — | — | 688 |

## Chain edges (from table)

- Success: **5081** → **5082**
- Success: **5082** → **5083**

## Task 5081

**Tags:** chains-to=5082

### How this task is received

- Player accepts from NPC table ID **688** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **688** (journal accept or auto-chain).
- Complete at terminator NPC table ID **2512** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5081
- `iNPC_ID` = runtime ID from grant NPC 688
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5081
- `iNPC_ID` = runtime ID from terminator NPC 2512
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get Spidermonkey’s alien DNA for the Nano Enhancement Project.

## Task 5082

**Tags:** chains-to=5083

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **699** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5082
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5082
- `iNPC_ID` = runtime ID from terminator NPC 699
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get Spidermonkey’s alien DNA for the Nano Enhancement Project.

## Task 5083

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **688** (proximity/talk).
- Reward table ID **646** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5083
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5083
- `iNPC_ID` = runtime ID from terminator NPC 688
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get Spidermonkey’s alien DNA for the Nano Enhancement Project.
