# Mission 45 — Tough Cell

| Field | Value |
|-------|-------|
| Mission ID | 45 |
| Mission type | Guide |
| Task count | 4 |
| Has timer tasks | No |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 179 | GotoLocation | — | — | 180 | — | 732 | 911 |
| 180 | Defeat | — | — | 181 | — | — | — |
| 181 | Delivery | — | — | 797 | — | — | 725 |
| 797 | Talk | — | — | — | — | — | 747 |

## Chain edges (from table)

- Success: **179** → **180**
- Success: **180** → **181**
- Success: **181** → **797**

## Task 179

**Tags:** chains-to=180

### How this task is received

- Player accepts from NPC table ID **732** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **118**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **732** (journal accept or auto-chain).
- Complete at terminator NPC table ID **911** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 179
- `iNPC_ID` = runtime ID from grant NPC 732
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 179
- `iNPC_ID` = runtime ID from terminator NPC 911
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the other half of the Plumber energy cell for Max.

## Task 180

**Tags:** kill-quota, item-quota, chains-to=181

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **118**.

### How this task must be completed

- Task type: **Defeat**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Kill quotas: enemy 408 x0
- Collect items: item 62 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 180
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 180
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the other half of the Plumber energy cell for Max.

## Task 181

**Tags:** item-quota, chains-to=797

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **118**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **725** (proximity/talk).
- Collect items: item 62 x1

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 181
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 181
- `iNPC_ID` = runtime ID from terminator NPC 725
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the other half of the Plumber energy cell for Max.

## Task 797

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **118**.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **747** (proximity/talk).
- Reward table ID **45** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 797
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 797
- `iNPC_ID` = runtime ID from terminator NPC 747
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Recover the other half of the Plumber energy cell for Max.
