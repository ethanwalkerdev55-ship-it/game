# Mission 356 — Bet on Ben (Part 2 of 5)

| Field | Value |
|-------|-------|
| Mission ID | 356 |
| Mission type | Normal |
| Task count | 6 |
| Has timer tasks | Yes |
| Has instance tasks | No |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 1942 | GotoLocation | — | — | 1943 | — | 732 | 911 |
| 1943 | UseItems | — | grant:600, gate:600 | 1944 | 1946 | — | 2303 |
| 1944 | UseItems | — | grant:180, gate:180 | 1945 | 1946 | — | 2304 |
| 1945 | UseItems | — | grant:180, gate:180 | 1947 | 1946 | — | 2305 |
| 1946 | GotoLocation | — | — | 1943 | — | — | 911 |
| 1947 | Delivery | — | — | — | — | — | 732 |

## Chain edges (from table)

- Success: **1942** → **1943**
- Success: **1943** → **1944**
- Fail (err 1/12): **1943** → **1946**
- Success: **1944** → **1945**
- Fail (err 1/12): **1944** → **1946**
- Success: **1945** → **1947**
- Fail (err 1/12): **1945** → **1946**
- Success: **1946** → **1943**

## Task 1942

**Tags:** chains-to=1943

### How this task is received

- Player accepts from NPC table ID **732** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **355**.

### How this task must be completed

- Task type: **GotoLocation**
- Start at grant NPC table ID **732** (journal accept or auto-chain).
- Complete at terminator NPC table ID **911** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1942
- `iNPC_ID` = runtime ID from grant NPC 732
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1942
- `iNPC_ID` = runtime ID from terminator NPC 911
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect concentrated Fusion Matter.

## Task 1943

**Tags:** grant-timer=600s, complete-timer-gate=600s, chains-to=1944, fail-restart=1946

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **355**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2303** (proximity/talk).
- Grant timer **600**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **600**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1943
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1943
- `iNPC_ID` = runtime ID from terminator NPC 2303
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect concentrated Fusion Matter.

## Task 1944

**Tags:** grant-timer=180s, complete-timer-gate=180s, chains-to=1945, fail-restart=1946

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **355**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2304** (proximity/talk).
- Grant timer **180**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **180**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1944
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1944
- `iNPC_ID` = runtime ID from terminator NPC 2304
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect concentrated Fusion Matter.

## Task 1945

**Tags:** grant-timer=180s, complete-timer-gate=180s, chains-to=1947, fail-restart=1946

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **355**.

### How this task must be completed

- Task type: **UseItems**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **2305** (proximity/talk).
- Grant timer **180**: server sends `iRemainTime` on start; client auto-sends `TASK_END` when `m_fRemainTime` reaches 0.
- Completion gate timer **180**: `CheckToCompleteTaskCondition` returns Fail until elapsed.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1945
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1945
- `iNPC_ID` = runtime ID from terminator NPC 2305
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect concentrated Fusion Matter.

## Task 1946

**Tags:** chains-to=1943

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **355**.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **911** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1946
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1946
- `iNPC_ID` = runtime ID from terminator NPC 911
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect concentrated Fusion Matter.

## Task 1947

**Tags:** item-quota

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.
- Prerequisite completed mission **355**.

### How this task must be completed

- Task type: **Delivery**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **732** (proximity/talk).
- Collect items: item 493 x3
- Reward table ID **444** (journal reward UI before `TASK_END`).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 1947
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 1947
- `iNPC_ID` = runtime ID from terminator NPC 732
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Collect concentrated Fusion Matter.
