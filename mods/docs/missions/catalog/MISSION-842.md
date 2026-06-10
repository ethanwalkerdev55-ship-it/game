# Mission 842 — Record Break-In

| Field | Value |
|-------|-------|
| Mission ID | 842 |
| Mission type | Nano |
| Task count | 18 |
| Has timer tasks | No |
| Has instance tasks | Yes |

## Task index

| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |
|---------|------|----------|--------|-----------|--------|-----------|------------|
| 5216 | Talk | — | — | 5217 | — | 720 | 719 |
| 5217 | GotoLocation | — | — | 5218 | — | — | 3315 |
| 5218 | Talk | — | — | 5219 | — | — | 3316 |
| 5219 | GotoLocation | — | — | 5220 | — | — | 3317 |
| 5220 | GotoLocation | — | — | 5221 | — | — | 3318 |
| 5221 | GotoLocation | — | — | 5222 | — | — | 3319 |
| 5222 | GotoLocation | — | — | 5223 | — | — | 3321 |
| 5223 | GotoLocation | 145 | — | 5224 | 5222 | — | 3322 |
| 5224 | GotoLocation | 145 | — | 5225 | 5222 | — | 3323 |
| 5225 | Talk | 145 | — | 5226 | 5222 | — | 3324 |
| 5226 | GotoLocation | 145 | — | 5227 | 5227 | — | 3327 |
| 5227 | Talk | 146 | — | 5228 | 5222 | — | 3328 |
| 5228 | GotoLocation | 146 | — | 5233 | 5233 | — | 3323 |
| 5229 | EscortDefence | 147 | — | 5230 | 5222 | — | — |
| 5230 | GotoLocation | 147 | — | 5231 | 5231 | — | 3331 |
| 5231 | Talk | 148 | — | 5232 | 5222 | — | 3328 |
| 5232 | GotoLocation | — | — | — | — | — | 3333 |
| 5233 | GotoLocation | 147 | — | 5229 | 5229 | — | 3330 |

## Chain edges (from table)

- Success: **5216** → **5217**
- Success: **5217** → **5218**
- Success: **5218** → **5219**
- Success: **5219** → **5220**
- Success: **5220** → **5221**
- Success: **5221** → **5222**
- Success: **5222** → **5223**
- Success: **5223** → **5224**
- Fail (err 1/12): **5223** → **5222**
- Success: **5224** → **5225**
- Fail (err 1/12): **5224** → **5222**
- Success: **5225** → **5226**
- Fail (err 1/12): **5225** → **5222**
- Success: **5226** → **5227**
- Fail (err 1/12): **5226** → **5227**
- Success: **5227** → **5228**
- Fail (err 1/12): **5227** → **5222**
- Success: **5228** → **5233**
- Fail (err 1/12): **5228** → **5233**
- Success: **5229** → **5230**
- Fail (err 1/12): **5229** → **5222**
- Success: **5230** → **5231**
- Fail (err 1/12): **5230** → **5231**
- Success: **5231** → **5232**
- Fail (err 1/12): **5231** → **5222**
- Success: **5233** → **5229**
- Fail (err 1/12): **5233** → **5229**

## Task 5216

**Tags:** chains-to=5217

### How this task is received

- Player accepts from NPC table ID **720** via journal (`cnEvent(12,5)`).
- Prerequisite completed mission **457**.
- Prerequisite completed mission **316**.

### How this task must be completed

- Task type: **Talk**
- Start at grant NPC table ID **720** (journal accept or auto-chain).
- Complete at terminator NPC table ID **719** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5216
- `iNPC_ID` = runtime ID from grant NPC 720
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5216
- `iNPC_ID` = runtime ID from terminator NPC 719
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5217

**Tags:** chains-to=5218

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3315** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5217
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5217
- `iNPC_ID` = runtime ID from terminator NPC 3315
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5218

**Tags:** chains-to=5219

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3316** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5218
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5218
- `iNPC_ID` = runtime ID from terminator NPC 3316
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5219

**Tags:** chains-to=5220

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3317** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5219
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5219
- `iNPC_ID` = runtime ID from terminator NPC 3317
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5220

**Tags:** chains-to=5221

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3318** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5220
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5220
- `iNPC_ID` = runtime ID from terminator NPC 3318
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5221

**Tags:** chains-to=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3319** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5221
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5221
- `iNPC_ID` = runtime ID from terminator NPC 3319
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5222

**Tags:** chains-to=5223

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3321** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5222
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5222
- `iNPC_ID` = runtime ID from terminator NPC 3321
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5223

**Tags:** instance-id=145, chains-to=5224, fail-restart=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3322** (proximity/talk).
- Instance required (**m_iRequireInstanceID=145**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5223
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5223
- `iNPC_ID` = runtime ID from terminator NPC 3322
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5224

**Tags:** instance-id=145, chains-to=5225, fail-restart=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3323** (proximity/talk).
- Instance required (**m_iRequireInstanceID=145**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5224
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5224
- `iNPC_ID` = runtime ID from terminator NPC 3323
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5225

**Tags:** instance-id=145, chains-to=5226, fail-restart=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3324** (proximity/talk).
- Instance required (**m_iRequireInstanceID=145**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5225
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5225
- `iNPC_ID` = runtime ID from terminator NPC 3324
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5226

**Tags:** instance-id=145, chains-to=5227, fail-restart=5227

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3327** (proximity/talk).
- Instance required (**m_iRequireInstanceID=145**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5226
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5226
- `iNPC_ID` = runtime ID from terminator NPC 3327
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5227

**Tags:** instance-id=146, chains-to=5228, fail-restart=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3328** (proximity/talk).
- Instance required (**m_iRequireInstanceID=146**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5227
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5227
- `iNPC_ID` = runtime ID from terminator NPC 3328
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5228

**Tags:** instance-id=146, chains-to=5233, fail-restart=5233

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3323** (proximity/talk).
- Instance required (**m_iRequireInstanceID=146**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5228
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5228
- `iNPC_ID` = runtime ID from terminator NPC 3323
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5229

**Tags:** instance-id=147, kill-quota, escort, chains-to=5230, fail-restart=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **EscortDefence**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete: auto when quotas met, timer expires, or remote packet.
- Instance required (**m_iRequireInstanceID=147**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.
- Kill quotas: enemy 3337 x1
- Escort NPC table ID **3338**; death → `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5229
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5229
- `iNPC_ID` = runtime ID from terminator NPC 0
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5230

**Tags:** instance-id=147, chains-to=5231, fail-restart=5231

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3331** (proximity/talk).
- Instance required (**m_iRequireInstanceID=147**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5230
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5230
- `iNPC_ID` = runtime ID from terminator NPC 3331
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5231

**Tags:** instance-id=148, chains-to=5232, fail-restart=5222

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **Talk**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3328** (proximity/talk).
- Instance required (**m_iRequireInstanceID=148**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5231
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5231
- `iNPC_ID` = runtime ID from terminator NPC 3328
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5232

**Tags:** standard

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3333** (proximity/talk).

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5232
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5232
- `iNPC_ID` = runtime ID from terminator NPC 3333
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.

## Task 5233

**Tags:** instance-id=147, chains-to=5229, fail-restart=5229

### How this task is received

- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.

### How this task must be completed

- Task type: **GotoLocation**
- Start: auto-chain or journal (no grant NPC table ID).
- Complete at terminator NPC table ID **3330** (proximity/talk).
- Instance required (**m_iRequireInstanceID=147**): server validates zone; complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`.

### Server packets

**Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
- `iTaskNum` = 5233
- `iNPC_ID` = runtime ID from grant NPC 0
- `iEscortNPC_ID` = escort runtime ID if type 6

**End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
- `iTaskNum` = 5233
- `iNPC_ID` = runtime ID from terminator NPC 3330
- `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
- `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown

### Journal summary

Investigate Belladonna’s missing jacket.
