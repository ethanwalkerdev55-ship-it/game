# Mission 43 — Vine Time

| Field | Value |
|-------|-------|
| Mission ID | 43 |
| Mission type | Guide |
| Task count | 2 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 173 | GotoLocation | — | — | 174 | — | 725 | 908 |
| 174 | UseItems | — | — | — | — | — | 838 |

## Chain edges (from table)

- Success: **173** → **174**

## Task 173

**Tags:** chains-to=174

### How this task is received

- Player accepts from NPC table ID **725** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **44**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **725** (journal accept or auto-chain).
- Complete at terminator NPC table ID **908** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 173
- `iNPC_ID` = runtime ID from grant NPC 725
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 173
- `iNPC_ID` = runtime ID from terminator NPC 908
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Inject Wildvine DNA into the heart of Fuse's infection.

## Task 174

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **44**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **838** (proximity/talk).
- Reward table ID **43** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 174
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 174
- `iNPC_ID` = runtime ID from terminator NPC 838
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Inject Wildvine DNA into the heart of Fuse's infection.
