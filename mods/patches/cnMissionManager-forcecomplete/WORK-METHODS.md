# ForceCompleteV2 — work methods log

Search: `ForceCompleteV2`, `mission autocomplete`, `cnMissionManager`, `FFPatch`, `golden DLL`, `dnSpy`, `FusionFall`

Date: 2026-06-06  
Project: `D:\work\roberto` — FusionFall / Retribution mission hotkey autocomplete

---

## 1. Goal

Fix **F11 / Right Ctrl** mission autocomplete (`ForceCompleteCurrentTask`) so it works for:

- Timer missions (server error 1 / “return to NPC”)
- Fusion Lair / Infected Zone (start → wait → end chain)

**Scope:** Patch **`cnMissionManager` only** in `Assembly - CSharp.dll`.  
**Do not patch:** `cnAvatarAttack` (hotkey already calls `ForceCompleteCurrentTask()`).

---

## 2. Toolchain (what each tool does)

| Tool | Path | Role |
|------|------|------|
| **Cursor** | `mods/decompiled/` | Edit / review C# logic |
| **dnSpy** | `tools/dnSpyEx/dnSpy.exe` | Compile patches into DLL (one-time golden) |
| **FFPatch** | `tools/FFPatch/` | IL transplant golden → vanilla (automated re-apply) |
| **DisunityFF** | `tools/DisunityFF/disunity.bat` | `bundle-extract` / `bundle-inject` for `main.unity3d` |
| **FFSpy** | `tools/FFSpy/` | Decompile / export reference only |

**Game files:**

```
6877b37c-e9cd-4826-b82c-5e8d3d5db744/main.unity3d          ← game bundle
6877b37c-e9cd-4826-b82c-5e8d3d5db744/main/Assembly - CSharp.dll
_inspect_bundle/main.bak/Assembly - CSharp.dll               ← vanilla backup
tools/FFPatch/golden/Assembly-CSharp.forcecomplete.dll       ← patched golden
```

**Deploy command (after golden exists):**

```bat
D:\work\roberto\tools\FFPatch\apply-mission-patch-v3.bat
```

(Legacy: `apply-mission-patch.bat` only — use v3 bat for full V2+V3 hotfixes.)

**Iteration guide:** `ITERATION-PLAYBOOK.md`

---

## 3. Methods tried — what failed and why

### ❌ Edit Class on whole `cnMissionManager`

- **Error:** CS1525 `Invalid expression term '<'` on coroutine stubs (`<CallGuideMessage>d__97`)
- **Cause:** dnSpy recompiles the **entire** class; vanilla decompile has invalid C# for compiler-generated coroutines
- **Verdict:** Do not use

### ❌ ONE-PASTE full class (`cnMissionManager-ONE-PASTE-EditClass.cs`)

- **Result:** Compiles in dnSpy
- **Runtime:** Game **freezes on loading screen**
- **Cause:** Full-class recompile breaks coroutine IL even when C# looks valid
- **Verdict:** Do not use

### ❌ Create Field × 4 + many manual steps (original step 0)

- **Result:** Works but tedious; user friction
- **Verdict:** Replaced by Add Class Members (one paste)

### ❌ UDP / packet injection

- **Result:** Not viable — `_inspect_udp_listener` is log-only; game uses encrypted TCP
- **Verdict:** Client DLL patch only

### ❌ Full assembly recompile (`_inspect_dll_diff/modified`)

- **Result:** 3500+ compile errors (decompiler artifacts)
- **Verdict:** Not viable

---

## 4. Method that worked — one-time golden build (dnSpy)

### Phase A — Add new members (once)

**dnSpy:** Right-click **`cnMissionManager`** → **Add Class Members (C#)...**

**Paste:** `add-class-members-compile-paste.txt` (from `public partial class cnMissionManager` through closing `}`)

**Rules:**

- Do **not** include dnSpy stub `FuncType` enum or `ManagerFunc` delegate → CS0102 duplicate
- Do **not** close class with `}` before pasted fields → CS0116 namespace error
- Paste file: `add-class-members-compile-paste.txt`

**Adds:** 4 fields + 7 helper methods (see `PROGRESS-LOG.md` for names)

### Phase B — Edit existing methods (once each)

**dnSpy:** Expand class in tree → right-click **method name** → **Edit Method (C#)...**  
(Not right-click on class — that shows Edit Class / Add Class Members only)

| Method | Paste file |
|--------|------------|
| `Update` | `step1-Update.cs` |
| `ProcessStartSucc` | `step2-ProcessStartSucc.cs` |
| `ProcessStartFail` | `step3-ProcessStartFail.cs` |
| `ProcessEndSucc` | `step4-ProcessEndSucc.cs` |
| `ProcessEndFail` | `step5-ProcessEndFail.cs` |
| `ForceCompleteCurrentTask` | `step6-ForceComplete-block.cs` **lines 3–19 only** |
| `RequestTaskComplete` | `step8-RequestTaskComplete.cs` (V3 diagnostics + or via Cecil patch-v3) |

Compile after **each** method.

### Compile fixes discovered

| Issue | Fix |
|-------|-----|
| CS0116 members outside class | Delete early `}` before fields; one `}` at end |
| CS0102 duplicate ManagerFunc | Remove enum/delegate from Add Class Members paste |
| CS0117 `base.gameObject` in ProcessEndFail | Use `SendSystemMessageBox(null, 12, "", "", null)` |
| CS0030 cast to MonoBehaviour | Same — use `null` for GameObject param |
| CS0111 duplicate helpers on step6 | Paste **only** `ForceCompleteCurrentTask` method, not full step6 file |

### Phase C — Save golden

**File → Save Module AS:**

```
D:\work\roberto\tools\FFPatch\golden\Assembly-CSharp.forcecomplete.dll
```

Verify:

```bat
D:\work\roberto\tools\FFPatch\run-verify-golden.bat
```

---

## 5. Automated deploy — FFPatch (repeat forever)

Built: `tools/FFPatch/FFPatch.exe` (Mono.Cecil IL transplant)

**What it does:**

1. Restore vanilla `Assembly - CSharp.dll` from `_inspect_bundle/main.bak/`
2. Copy from golden: 4 fields, 6 replaced methods, 7 new methods
3. **Keep vanilla** coroutine nested types (`<CallGuideMessage>d__97`, `<NanoTimeEffect>d__98`)
4. Verify patch markers in output DLL
5. Run `bundle-inject` on `main.unity3d`

**Bugs fixed in FFPatch:**

- File lock on write — Cecil held DLL open during `File.Copy`; fixed by disposing assemblies before copy
- Verify too strict — golden may not contain literal `ForceCompleteV2` string; also checks `GetForceCompleteTarget` / `bForceCompleteChain`
- Backup in bundle folder — backups moved to `tools/FFPatch/backups/`; bat deletes `*.pre-ffpatch.bak` before inject

**Status (2026-06-06):** Patch applied + bundle-inject completed successfully.

---

## 6. Patch logic summary (ForceCompleteV2)

**Trigger:** F11 (key 305) or Right Ctrl → `cnAvatarAttack` → `ForceCompleteCurrentTask()`

**Flow:**

```
ForceCompleteCurrentTask()
  → GetForceCompleteTarget()
  → bForceCompleteChain = true
  → TryForceCompleteChainTask()
       → PrepareTaskForForceComplete()
       → NeedsForceCompleteTaskStart()? → RequestTaskStart()
       → else RequestForceCompleteTaskEnd() → RequestTaskComplete() packet

Async (server responses):
  Update()              — pending timer tasks
  ProcessStartSucc      — auto-end after start for IZ/timer
  ProcessStartFail      — retry end if task already active
  ProcessEndSucc        — chain to next task
  ProcessEndFail        — retry with/without NPC, error flag (max 9)
```

**Source of truth (Cursor):** `mods/decompiled/Assembly-CSharp/cnMissionManager.cs`  
**Patch files:** `mods/patches/cnMissionManager-forcecomplete/step*.cs`

---

## 7. File map

```
mods/patches/cnMissionManager-forcecomplete/
  WORK-METHODS.md              ← this file
  PROGRESS-LOG.md              ← step-by-step completion checklist
  BUILD-GOLDEN.txt             ← short golden-build guide
  add-class-members-compile-paste.txt   ← working Add Class Members paste
  step1-Update.cs … step6-ForceComplete-block.cs
  APPLY-IN-ORDER.txt           ← old surgical steps (superseded by BUILD-GOLDEN + FFPatch)

tools/FFPatch/
  FFPatch.exe (after build)
  apply-mission-patch.bat      ← run to deploy
  run-verify-golden.bat
  golden/Assembly-CSharp.forcecomplete.dll
  backups/                     ← pre-patch DLL backups

.cursor/rules/fusionfall-dnspy-modding.mdc   ← Cursor rules for this workflow
mods/CHANGELOG.md
GUIDE-dnSpy-Cursor-DisunityFF.md
```

---

## 8. Do NOT use

- `cnMissionManager-ONE-PASTE-EditClass.cs` — load freeze
- **Edit Class** on `cnMissionManager` — coroutine compile errors
- `Assembly - CSharp - first pass.dll` — not needed for this patch
- Pasting full `step6` into Edit Method — CS0111 duplicates

---

## 9. Verify in game

1. Restart FusionFall
2. Accept a mission (timer or Fusion Lair if possible)
3. Press **F11**
4. Check `fusionfall_log.txt` for:
   - `ForceCompleteV2: start chain task`
   - `ForceCompleteV2: request end`
   - `ForceCompleteV2: chain finished`

---

## 10. Restore vanilla

```bat
copy /Y D:\work\roberto\_inspect_bundle\main.bak\Assembly - CSharp.dll D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\
D:\work\roberto\tools\DisunityFF\disunity.bat bundle-inject D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d
```

Or restore `main.unity3d.bak` if game won't load.

---

## 11. Related docs

- `PROGRESS-LOG.md` — what steps are done / TODO
- `BUILD-GOLDEN.txt` — condensed golden build steps
- `mods/CHANGELOG.md` — project changelog entry
