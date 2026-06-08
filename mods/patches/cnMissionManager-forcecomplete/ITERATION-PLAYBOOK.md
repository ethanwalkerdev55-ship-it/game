# Mission Autocomplete — iteration playbook

Search: `ForceCompleteV2`, `FFPatch`, `golden DLL`, `fusionfall_log`, `ROOT-CAUSE`

This guide documents **how to diagnose, patch, deploy, and verify** mission autocomplete the same way each time.

---

## 1. Workflow overview

```
Edit C# source (mods/decompiled + step*.cs)
    ↓
Update golden DLL (dnSpy OR FFPatch patch-v2 / patch-v3 on golden)
    ↓
apply-mission-patch.bat  (FFPatch IL transplant → bundle-inject)
    ↓
In-game test → fusionfall_log.txt → read ForceCompleteV2 lines
    ↓
Document in PROGRESS-LOG.md + ROOT-CAUSE-*.md
```

**One-command deploy (current):**

```bat
D:\work\roberto\tools\FFPatch\apply-mission-patch-v3.bat
```

Runs: build FFPatch → patch-v2 → patch-v3 → apply-mission-patch.

---

## 2. Log analysis checklist

When the customer sends `fusionfall_log.txt`:

| Look for | Meaning |
|----------|---------|
| `ForceCompleteV2: start chain task N` | F11 fired; patch active |
| `ForceCompleteV2: request end N` | Client intends to complete task N |
| `ForceCompleteV2: sent complete packet task N` | **Packet actually sent** (V3+) |
| `ForceCompleteV2: complete blocked …` | Silent drop **before** send — read reason |
| `ProcessEndSucc` / `ProcessEndFail` | Server replied |
| `ForceCompleteV2: advance to task N` | Chain progressed |
| `ForceCompleteV2: instance zone complete task N` | Fusion Lair bypass (V2) |
| `retry task N err X attempt Y` | Retry escalation (Y should increase) |
| `complete after start 466` during 463 | Spurious chain hook (V2 guard bug) |
| `active task id : N` on login | Mission **still on character** (not deleted) |

**Important:** `request end` without `sent complete packet` or `ProcessEnd*` means the client blocked the complete locally — mission is usually still there.

---

## 3. Patch versions (cumulative)

| Version | What it fixes | How applied |
|---------|---------------|-------------|
| **V1** | Base ForceComplete chain (dnSpy golden) | step1–6 + add-class-members |
| **V2** | Instance zone `bError`; pending-only ProcessStartSucc; no retry reset | `FFPatch patch-v2` |
| **V3** | First complete uses `npc 0`; log blocked/sent complete | `FFPatch patch-v3` + step8 |

Source files:

- Helpers: `add-class-members-compile-paste.txt`
- Hooks: `step1-Update.cs` … `step5-ProcessEndFail.cs`, `step6-ForceComplete-block.cs`
- Vanilla method: `step8-RequestTaskComplete.cs`
- Decompiled truth: `mods/decompiled/Assembly-CSharp/cnMissionManager.cs`

---

## 4. Adding the next fix (repeatable steps)

1. **Reproduce** — one F11 press, wait 15s, capture full log.
2. **Diagnose** — map log lines to code path (see §2 and `ROOT-CAUSE-V2.md`).
3. **Edit source** — `cnMissionManager.cs` + matching `step*.cs` / `add-class-members`.
4. **Update golden:**
   - **Small helper change:** re-paste `add-class-members` in dnSpy, or extend `patch-vN` Cecil hotfix.
   - **Hook method:** Edit Method in dnSpy from `stepN-*.cs`.
   - **Vanilla method:** Edit Method from `step8-RequestTaskComplete.cs` or new step file; add method name to `Program.cs` `TransplantMethods`.
5. **Deploy:** `apply-mission-patch-v3.bat` (or `apply-mission-patch.bat` if golden already updated).
6. **Verify log markers** — see §5.
7. **Log work** — append `PROGRESS-LOG.md`; if new bug class, add `ROOT-CAUSE-VX.md`.

---

## 5. Expected log after V3 (healthy chain)

```
ForceCompleteV2: start chain task 466
ForceCompleteV2: request end 466
ForceCompleteV2: complete without npc gate task 466
ForceCompleteV2: sent complete packet task 466 npc 0 err False
ProcessEndSucc / ProcessStartFail / advance to task 468
…
ForceCompleteV2: instance zone complete task 463
ProcessEndSucc
ForceCompleteV2: chain finished
```

If blocked instead:

```
ForceCompleteV2: complete blocked pending checker task 466
```

→ wait for server or clear checker; do not spam F11.

---

## 6. File map

| File | Purpose |
|------|---------|
| `WORK-METHODS.md` | dnSpy + FFPatch toolchain |
| `ITERATION-PLAYBOOK.md` | This file — diagnose / deploy loop |
| `ROOT-CAUSE-V2.md` | Timer + Fusion Lair analysis |
| `PROGRESS-LOG.md` | Done / TODO checklist |
| `BUILD-GOLDEN.txt` | One-time dnSpy golden build |
| `tools/FFPatch/PatchV2.cs` | Cecil golden hotfix V2 |
| `tools/FFPatch/PatchV3.cs` | Cecil golden hotfix V3 |
| `apply-mission-patch-v3.bat` | Full deploy |

---

## 7. Restore vanilla

See `WORK-METHODS.md` §10.
