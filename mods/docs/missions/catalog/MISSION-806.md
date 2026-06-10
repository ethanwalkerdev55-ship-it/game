# Mission 806 — All the Aliens (Part 1 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 806 |
| Mission type | Normal |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5078 | GotoLocation | — | — | 5079 | — | 688 | 885 |
| 5079 | Talk | — | — | 5080 | — | — | 698 |
| 5080 | Talk | — | — | — | — | — | 688 |

## Chain edges (from table)

- Success: **5078** → **5079**
- Success: **5079** → **5080**

## Task 5078

**Tags:** chains-to=5079

### How this task is received

- Player accepts from NPC table ID **688** via journal (`cnEvent(12,5)`).

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **688** (journal accept or auto-chain).
- Complete at terminator NPC table ID **885** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5078
- `iNPC_ID` = runtime ID from grant NPC 688
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5078
- `iNPC_ID` = runtime ID from terminator NPC 885
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get AmpFibian’s alien DNA for the Nano Enhancement Project.

## Task 5079

**Tags:** chains-to=5080

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **698** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5079
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5079
- `iNPC_ID` = runtime ID from terminator NPC 698
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get AmpFibian’s alien DNA for the Nano Enhancement Project.

## Task 5080

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **688** (proximity/talk).
- Reward table ID **516** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5080
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5080
- `iNPC_ID` = runtime ID from terminator NPC 688
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Get AmpFibian’s alien DNA for the Nano Enhancement Project.
