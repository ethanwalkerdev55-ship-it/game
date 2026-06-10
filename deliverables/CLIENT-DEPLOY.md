# Deploying the patched main.unity3d

The patch is inside `main.unity3d`. There is no separate DLL.

If the launcher loads the wrong bundle, the game looks unchanged. That is what happens when you copy the file into cache but the launcher still expects the old CDN hash — it puts vanilla back on the next login.

---

## Current build

| Field | Value |
|-------|-------|
| File | `main.unity3d` |
| Size (bytes) | `7125461` |
| SHA256 | `cbddcfd80a4d21c1097a25501b225b91c16df62c377bc9b02ac9fb79fff058c8` |
| Build stamp (in-game log) | `ForceCompleteV2: patch build 2026-06-09-doc504f` |
| Version UUID | `6877b37c-e9cd-4826-b82c-5e8d3d5db744` |

```powershell
Get-FileHash .\main.unity3d -Algorithm SHA256
```

---

## Local install — copy to cache (usual workflow)

This matches how you already test: drop `main.unity3d` into your cache folder. The only extra step is telling the launcher that the file on disk is the one it should load.

Copying alone is not enough because the launcher keeps a record of the expected hash and size in a small JSON file. When your copy does not match that record, the launcher fetches the old file again from the CDN.

You do not need to edit that JSON by hand. Run `register-local-patch.bat` after you copy the file. It reads whatever is in your cache folder, updates the launcher record, syncs the secondary cache, and locks the record so the launcher UI does not switch back to CDN.

### Every time you install a new build

1. Quit the game and launcher completely.
2. Copy `main.unity3d` into your **offline cache** folder, overwriting the old file.

   OpenFusionLauncher stores that path in `%APPDATA%\OpenFusionLauncher\config.json`:

   ```json
   "offline_cache_path": "D:\\YourPath",
   ```

   Copy the bundle to:

   ```
   {offline_cache_path}\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main.unity3d
   ```

   If you have not changed launcher settings, `offline_cache_path` is usually `%LOCALAPPDATA%\OpenFusionLauncher\offline_cache`. You can also open the folder from the launcher UI (blue folder icon on the Game Builds tab).

3. Double-click `register-local-patch.bat`.

   The script reads `config.json` and registry automatically. You do not need to pass a path unless you are copying somewhere outside the launcher’s configured offline cache.

4. Reopen the launcher and Connect.

### If you copy somewhere else

Only if the file is not under the launcher’s `offline_cache_path`:

```bat
register-local-patch.bat -CacheDir "D:\SomeOtherFolder\6877b37c-e9cd-4826-b82c-5e8d3d5db744"
```

### Check that it worked

Press **Right Ctrl** in-game with a mission task active. The log should show:

```
ForceCompleteV2: patch build 2026-06-09-doc504f
```

File size on disk should be **7125461** bytes. If you still see **6993722** or **7125105**, the old bundle is still loading — run step 3 again and make sure the launcher was fully closed first.

---

## Why the extra step exists

The launcher does three things on connect:

1. Read `%APPDATA%\OpenFusionLauncher\versions\6877b37c-e9cd-4826-b82c-5e8d3d5db744.json`
2. Compare hash and size against that record
3. Download from `main_file_url` when they do not match

So “I copied it but on re-log the old one is back” means step 2/3 ran against stale CDN metadata. `register-local-patch.bat` updates that metadata to match the file you copied and points `main_file_url` at your local path.

This is the same work our dev scripts do automatically. The batch file is the portable version for your machine.

---

## Rollout to all players (CDN)

When you want every player to get the patch without copying files themselves, put the bundle on your CDN and update the server-side version manifest hash and size. That is a one-time server change per build, not something each tester does.

1. Replace `main.unity3d` on the CDN URL already in your live manifest.
2. Set `main_file_info.hash` and `main_file_info.size` to the values in the table above.
3. Players restart the launcher and Connect.

Use the local copy workflow for your own testing. Use the CDN path when you publish to everyone.

---

## Rollback

**Local:** restore your previous `main.unity3d` into the cache folder, run `register-local-patch.bat`, reconnect.

**CDN:** restore the previous CDN file and manifest hash/size.

To unlock the launcher record for manual edits:

```powershell
attrib -R "$env:APPDATA\OpenFusionLauncher\versions\6877b37c-e9cd-4826-b82c-5e8d3d5db744.json"
```

---

## Troubleshooting

**File disappears or reverts after login**  
You copied the file but did not run `register-local-patch.bat`, or the launcher was still running when you copied.

**No `ForceCompleteV2` line in log**  
Wrong bundle is loading. Confirm hash with `Get-FileHash`. Run the register script again.

**Connect hangs at loading**  
Separate issue (bootstrap / bundle integrity). Send log excerpt and bundle hash.

**Hotkey logs stamp but mission does not advance**  
Patch is loaded. Mission logic for that task is still under test — note mission ID, task ID, instance zone.

---

## What to ship with a new build

1. `main.unity3d`
2. `register-local-patch.bat` and `register-local-patch.ps1`
3. This file (`CLIENT-DEPLOY.md`) with the updated size/hash in the table above
