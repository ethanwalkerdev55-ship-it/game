# FusionFall Modding Guide: FFSpy → Cursor → dnSpy → DisunityFF

This guide walks you through editing game DLLs using a **three-tool pipeline**:

| Tool | Role |
|------|------|
| **FFSpy / ILSpy** | Export full decompiled C# from the DLL |
| **Cursor** | Edit code with AI assistance |
| **dnSpy** | Paste changes back, compile, and save the DLL |
| **DisunityFF** | Extract and inject Unity bundle files |

It is written for beginners. Follow the steps in order the first time; later you can skip steps you already know.

---

## The workflow at a glance

```text
main.unity3d
    │
    ▼  bundle-extract (DisunityFF)
DLL files on disk  (e.g. Assembly - CSharp.dll)
    │
    ▼  export (FFSpy / ILSpy)
mods\decompiled\   (hundreds of .cs files)
    │
    ▼  edit (Cursor + AI)
modified .cs files
    │
    ▼  reconnect (dnSpy: paste → Compile → Save Module)
modified DLL on disk
    │
    ▼  bundle-inject (DisunityFF)
main.unity3d  (updated)
```

You never edit `main.unity3d` directly. You extract its contents, change the DLLs, then inject them back.

---

## What each tool does (and does not do)

| Capability | FFSpy / ILSpy | Cursor | dnSpy | DisunityFF |
|------------|---------------|--------|-------|------------|
| Browse DLL tree | Yes | No | Yes | No |
| Export full decompiled C# | **Yes — best for this** | No | Yes (File → Export to Project) | No |
| Edit code with AI | No | **Yes — best for this** | No | No |
| Compile changes into DLL | No | No | **Yes — required** | No |
| Debug running game | No | No | Yes | No |
| Extract / inject bundles | No | No | No | **Yes** |

**Key idea:** Use **dnSpy-format export** (`export-assembly-csharp.bat`) for files you paste back into dnSpy. ILSpy/FFSpy output is fine for *reading* but uses a different decompiler style (`Object.op_Implicit`, `yield return`) that breaks dnSpy recompile. Only dnSpy can *write* the patched DLL back.

---

## What you need installed

| Tool | Purpose | Notes |
|------|---------|-------|
| **Java (JRE 17+)** | Runs DisunityFF | Required by `disunity.bat`. Eclipse Temurin works well. |
| **FFSpy / ILSpy** | Export DLL to `.cs` files | Ready-to-use GUI at `tools\FFSpy\ILSpy.exe`. One-click export: `tools\FFSpy\export-assembly-csharp.bat`. |
| **Cursor** | Edit exported `.cs` files with AI | You are already using this. |
| **dnSpy** | Compile and save `.dll` files | Download from the [dnSpyEx releases](https://github.com/dnSpyEx/dnSpy/releases) page on GitHub. |
| **DisunityFF** | `bundle-extract` and `bundle-inject` | OpenFusion fork at `tools\DisunityFF\` (see `VERSION.txt`). **Not** stock ata4 0.3.4 or disunity 0.5.x. |

---

## Recommended folder layout

Create these folders once inside `D:\work\roberto\`:

```text
D:\work\roberto\
├── 6877b37c-e9cd-4826-b82c-5e8d3d5db744\
│   └── main\
│       ├── main.unity3d
│       ├── Assembly - CSharp.dll      ← main game logic (most mods target this)
│       └── System.dll                 ← .NET runtime (rarely modified)
├── mods\
│   ├── decompiled\
│   │   └── Assembly-CSharp\           ← FFSpy export goes here (edit in Cursor)
│   ├── patches\                       ← optional: small single-file patches
│   ├── backups\                       ← copies of originals before each change
│   └── CHANGELOG.md                   ← track which classes/methods you changed
├── FFSpy-7.0\                         ← ILSpy 7.0 source (optional; for reference)
├── tools\
│   ├── FFSpy\                        ← ILSpy 7.2.1 GUI + export scripts
│   └── DisunityFF\
├── commad.txt                         ← your extract/inject commands
└── GUIDE-dnSpy-Cursor-DisunityFF.md   ← this file
```

**Two locations to remember:**

- `mods\decompiled\` — working copy for Cursor (export destination)
- `6877b37c-...\main\Assembly - CSharp.dll` — the real DLL dnSpy saves and DisunityFF injects

---

## Step 0 — Back up before you change anything

Always make a backup **before** extract, edit, or inject.

1. Locate your cache folder. In this project:

   ```text
   D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\
   ```

   Your real cache folder may be elsewhere on disk. Use the path where your `main.unity3d` actually lives.

2. Copy the backup:

   ```text
   D:\work\roberto\mods\backups\main.unity3d.backup-YYYY-MM-DD\
   ```

3. Include both `main.unity3d` and the `main\` folder (with all DLLs).

If the game stops working after a mod, restore the backup and you are back to the original state.

---

## Step 1 — Extract the bundle with DisunityFF

`tools\DisunityFF\` must be the **OpenFusion DisUnityFF fork** (DisUnity 0.3.4 base + `bundle-create`). It is **not** compatible with disunity **0.5.x** (`bundle-unpack` / `bundle-pack` only). Verify with `disunity.bat -h` — you should see `bundle-create`.

Open **PowerShell** or **Command Prompt** and run:

```bat
D:\work\roberto\tools\DisunityFF\disunity.bat bundle-extract D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d
```

Replace the path if your `main.unity3d` is elsewhere.

### What happens

- DisunityFF reads `main.unity3d`.
- It writes extracted files into a folder next to the bundle — in this project, that is:

  ```text
  D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\
  ```

- You should see `Assembly - CSharp.dll`, `System.dll`, and possibly other files.

### Which DLL to mod

| DLL | When to edit |
|-----|--------------|
| `Assembly - CSharp.dll` | **Almost always** — game logic, missions, UI, combat, etc. |
| `System.dll` | Rarely — .NET framework runtime; leave alone unless you know you need it |

### List bundle contents (optional)

```bat
D:\work\roberto\tools\DisunityFF\disunity.bat bundle-list D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d
```

---

## Step 2 — Export the DLL (dnSpy format)

**Important:** ILSpy/FFSpy and dnSpy produce **different C# styles**. Only dnSpy-format export is safe to paste back into dnSpy **Edit Class**.

| Export | Script | Output folder | Paste back into dnSpy? |
|--------|--------|---------------|------------------------|
| **dnSpy (default)** | `export-assembly-csharp.bat` | `mods\decompiled\Assembly-CSharp\` | **Yes** |
| Single class | `export-class-dnspy.bat cnMissionManager` | same | **Yes** |
| ILSpy (reference) | `export-assembly-csharp-ilspy.bat` | `mods\decompiled-ilspy\Assembly-CSharp\` | **No** |
| Browse only | `open-ffspy.bat` | — | **No** |

### Option A — One-click dnSpy export (recommended)

First time only:

```bat
D:\work\roberto\tools\FFSpy\install-dnspy-export.bat
```

Then export:

```bat
D:\work\roberto\tools\FFSpy\export-assembly-csharp.bat
```

This uses `dnSpy.Console.exe` with `--no-yield` so coroutines stay as nested compiler-generated classes (not `yield return`). Output goes to:

```text
D:\work\roberto\mods\decompiled\Assembly-CSharp\
```

You should get ~850 `.cs` files with `this.` / `base.` and `// Token:` comments.

### Option B — Export one class (surgical edits)

```bat
D:\work\roberto\tools\FFSpy\export-class-dnspy.bat cnMissionManager
```

Use this when you only need to patch a few methods — avoids whole-class recompile risks.

### Option C — FFSpy / ILSpy GUI (browse only)

```bat
D:\work\roberto\tools\FFSpy\open-ffspy.bat
```

Use ILSpy to **search and read** code. Do **not** use **Save Code** output for dnSpy paste-back.

### Option D — ILSpy command line (reference only)

```bat
D:\work\roberto\tools\FFSpy\export-assembly-csharp-ilspy.bat
```

Output uses `Object.op_Implicit`, `yield return`, and `_003C` names — **not** dnSpy round-trip format.

### Option E — dnSpy GUI export

1. Open `Assembly - CSharp.dll` in dnSpy (`tools\dnSpyEx\dnSpy.exe`).
2. Select the assembly in Assembly Explorer.
3. **File → Export to Project** → `mods\decompiled\Assembly-CSharp\`.

### Verify DisunityFF (OpenFusion fork)

```bat
D:\work\roberto\tools\DisunityFF\disunity.bat -h
```

You must see **`bundle-create`** in the command list. If not, you have stock ata4 DisUnity — see `tools\DisunityFF\VERSION.txt`.

### What exported code looks like

Decompiled C# is readable but not perfect. Expect:

- Generic variable names (`num`, `flag`, `text`)
- `// Token: 0x...` comments (dnSpy format — normal)
- Nested `<MethodName>d__N` coroutine classes (dnSpy `--no-yield` — normal)

That is normal. You are editing working decompiled text, not the original source.

---

## Step 3 — Edit the code in Cursor

### Open the exported project

1. Open **Cursor**.
2. **File → Open Folder** → select `D:\work\roberto\` (or just `mods\decompiled\`).
3. Browse the `.cs` files in the left file explorer.

### Find the code you want to change

Use **Ctrl+Shift+F** (search across files) to find:

- Strings from `fusionfall_log.txt` (e.g. `ProcessStartFail`, `TaskID`, `DelMissionTaskChecker`)
- Class or method names you identified in ILSpy/dnSpy
- Feature-related keywords (mission, task, combat, etc.)

### Edit with AI

Open the relevant `.cs` file and ask the AI assistant for help. Include:

- The **full method or class** you are changing
- **What you want to happen** in the game
- The **DLL name** (`Assembly - CSharp.dll`) and **class/method name**
- Relevant **log lines** from `fusionfall_log.txt`
- Any **compile errors** from dnSpy (if you already tried to apply a patch)

Example prompt:

```text
I'm modding FusionFall. In Assembly - CSharp.dll, class TaskNode, method ProcessStartSucc —
I want to always succeed instead of failing with Error Code 1.
Here is the current decompiled code:
[paste code here]
```

### Track your changes

Create or update `D:\work\roberto\mods\CHANGELOG.md` as you edit. Example entry:

```markdown
## 2026-06-05
- File: TaskNode.cs
- Method: ProcessStartSucc
- Change: Skip failure path when Error Code == 1
- Status: edited in Cursor, not yet applied in dnSpy
```

This list tells you exactly what to paste back into dnSpy in Step 4.

### Save your work

Press **Ctrl+S** after every edit. Keep all changes under `mods\decompiled\`.

---

## Step 4 — Reconnect to dnSpy (apply changes back to the DLL)

dnSpy has no **Import Project** button. "Reconnect" means: open the **original DLL**, paste your Cursor edits into dnSpy's editor, compile, and save.

### Open the original DLL in dnSpy

1. Start **dnSpy**.
2. **File → Open** → select:

   ```text
   D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\Assembly - CSharp.dll
   ```

   This must be the same DLL DisunityFF extracted — not a copy elsewhere.

3. Keep dnSpy open for the rest of this step.

### Paste each changed method or class

Work through your `CHANGELOG.md` entries. For **each** changed method:

1. In dnSpy's **Assembly Explorer**, navigate to the class and method (use **Ctrl+Shift+K** to search).
2. Right-click the method → **Edit Method (C#)**.
   - For a whole class: right-click the class → **Edit Class (C#)**.
3. Select all code in dnSpy's editor (**Ctrl+A**) and delete it.
4. In Cursor, open the matching `.cs` file, copy the updated method (or class), and paste into dnSpy.
5. Click **Compile**.
6. If it succeeds, close the editor tab.
7. If it **fails**:
   - Read the error message carefully.
   - Paste the error + code into Cursor and ask the AI to fix it.
   - Try **Compile** again in dnSpy.

Repeat for every entry in your changelog.

### Tips for successful compile

- Paste only the **method** or **class** — not the entire `.cs` file with `using` lines unless you are using **Edit Class** for the whole class.
- Make **small changes** first; large rewrites often fail to compile.
- If one method keeps failing, try changing less — e.g. flip one `if` condition instead of rewriting the whole method.

### Save the modified DLL

After all changes compile:

1. **File → Save Module**.
2. Confirm the save path is:

   ```text
   D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\Assembly - CSharp.dll
   ```

3. If dnSpy offers to create a backup, click **Yes**.

**Critical:** Compiling in dnSpy updates the in-memory module only. The game will not see your changes until you **Save Module**.

### Verify (optional)

Re-open a method you changed and confirm your new code is visible in the decompiler view.

---

## Step 5 — Inject the modified DLL back into the bundle

Close dnSpy (or unload the module) so the DLL file is not locked, then run:

```bat
D:\work\roberto\tools\DisunityFF\disunity.bat bundle-inject D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d
```

### What happens

- DisunityFF reads the extracted files in the `main\` folder (including your modified `Assembly - CSharp.dll`).
- It packs them back into `main.unity3d`, replacing the bundle in place.

If inject fails, check that:

- You ran `bundle-extract` on this same `main.unity3d` first.
- The modified DLL is in `main\` beside the other extracted files.
- dnSpy is closed and the DLL is not locked.

---

## Step 6 — Test in the game

1. Start the game (or restart if it was already running).
2. Trigger the feature you modified.
3. Watch for crashes, silent failures, or wrong behavior.
4. Check `fusionfall_log.txt` for new log lines.

### If something goes wrong

| Problem | What to try |
|---------|-------------|
| Game crashes on start | Restore `main.unity3d` and the `main\` folder from `mods\backups\`. |
| Change has no effect | Confirm you **Save Module** in dnSpy and ran `bundle-inject` on the correct `main.unity3d`. |
| dnSpy compile error | Paste error + code into Cursor; fix types, `ref`/`out`, or syntax issues. |
| Inject fails | Re-run `bundle-extract`, re-apply your dnSpy changes, then inject again. |

Update `mods\CHANGELOG.md` with test results (worked / failed / needs retry).

---

## Quick reference — full pipeline

```bat
:: 0. Backup main.unity3d and main\ folder manually

:: 1. Extract bundle
D:\work\roberto\tools\DisunityFF\disunity.bat bundle-extract D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d

:: 2. Export DLL
D:\work\roberto\tools\FFSpy\export-assembly-csharp.bat
::    Output: D:\work\roberto\mods\decompiled\Assembly-CSharp\

:: 3. Edit .cs files in Cursor (track changes in mods\CHANGELOG.md)

:: 4. dnSpy → Open Assembly - CSharp.dll → paste changes → Compile → Save Module

:: 5. Inject bundle
D:\work\roberto\tools\DisunityFF\disunity.bat bundle-inject D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d

:: 6. Test in game
```

These inject/extract commands match `commad.txt` in this project.

---

## Tool shortcuts

### ILSpy / FFSpy

| Action | How |
|--------|-----|
| Open DLL | File → Open |
| Export full project | Right-click DLL → Save Code / Export to Project |
| Search types | Ctrl+F or the Search pane |

### dnSpy

| Action | How |
|--------|-----|
| Export full project | Select assembly → **File → Export to Project** (not right-click) |
| Search assemblies | **Ctrl+Shift+K** |
| Edit a method | Right-click method → **Edit Method (C#)** |
| Apply edits | Click **Compile** |
| Write DLL to disk | **File → Save Module** |

### Cursor

| Shortcut | Action |
|----------|--------|
| **Ctrl+Shift+F** | Search across all decompiled `.cs` files |
| **Ctrl+L** | Open AI chat for editing help |

---

## Common beginner mistakes

1. **Editing `main.unity3d` directly** — Always use `bundle-extract` / `bundle-inject`.
2. **Editing only `mods\decompiled\` and skipping dnSpy** — Cursor changes are not in the game until dnSpy compiles and saves the DLL.
3. **Forgetting Save Module** — Compile alone does not write to disk.
4. **Injecting while dnSpy has the DLL open** — Close dnSpy first or inject will fail.
5. **No backup** — One bad DLL can break the game until you restore.
6. **Changing too much at once** — Patch one method, test, then continue.
7. **Modifying `System.dll` by accident** — Target `Assembly - CSharp.dll` unless you have a specific reason not to.

---

## Optional upgrades

| Upgrade | Benefit |
|---------|---------|
| `mods\CHANGELOG.md` | Know exactly what to paste back into dnSpy |
| **ILSpy-MCP** server | Lets Cursor AI search/decompile the DLL from chat without manual export |
| **dnSpy debugger** | Attach to the running game for breakpoints (after you learn this pipeline) |
| Cursor rule in `.cursor\rules\` | AI remembers your DLL names and mod goals between sessions |
| `tools\FFSpy\export-assembly-csharp.bat` | One-click export (already installed) |

None of these replace the dnSpy **Save Module** step — that remains required.

---

## Getting help in Cursor

When asking the AI for help, paste or attach:

1. The `.cs` file or method you are changing
2. The dnSpy compile error (if any)
3. What you expected vs what happened in-game
4. Relevant lines from `fusionfall_log.txt`
5. Your `CHANGELOG.md` entry for context

That makes it much faster to get a working patch.
