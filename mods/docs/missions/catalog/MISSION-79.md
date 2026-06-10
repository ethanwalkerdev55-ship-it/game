# Mission 79 — Sandy Candy

| Field | Value |
|-------|-------|
| Mission ID | 79 |
| Mission type | Guide |
| Task count | 3 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 315 | GotoLocation | — | — | 316 | — | 726 | 906 |
| 316 | UseItems | — | — | 317 | — | — | 853 |
| 317 | Delivery | — | — | — | — | — | 726 |

## Chain edges (from table)

- Success: **315** → **316**
- Success: **316** → **317**

## Task 315

**Tags:** chains-to=316

### How this task is received

- Player accepts from NPC table ID **726** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **163**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **726** (journal accept or auto-chain).
- Complete at terminator NPC table ID **906** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 315
- `iNPC_ID` = runtime ID from grant NPC 726
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 315
- `iNPC_ID` = runtime ID from terminator NPC 906
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the third stash of candy from the beach.

## Task 316

**Tags:** chains-to=317

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **163**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **853** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 316
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 316
- `iNPC_ID` = runtime ID from terminator NPC 853
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the third stash of candy from the beach.

## Task 317

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **163**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **726** (proximity/talk).
- Collect items: item 100 x1
- Reward table ID **79** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 317
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 317
- `iNPC_ID` = runtime ID from terminator NPC 726
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the third stash of candy from the beach.
