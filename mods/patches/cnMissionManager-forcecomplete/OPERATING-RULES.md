# Mission patch — operating rules (mandatory)

These rules exist because **verify-il passing does not mean the game loads**. Black screens cost hours and erode client trust.

---

## 1. Golden lineage (only approved bases)

| Layer | Path | Purpose |
|-------|------|---------|
| Client bundle | `ClientFile/` → `_inspect_bundle/client/main.unity3d` | **Only** load-safe bundle (includes UdpLogger in first pass DLL) |
| Client DLL | `_inspect_bundle/client/Assembly - CSharp.dll` | Patch base — never `main.bak` for deploy |
| UdpLogger | `_inspect_bundle/client/Assembly - CSharp - first pass.dll` | Never overwrite with vanilla first pass |
| Staging DLL | `tools/FFPatch/staging/Assembly-CSharp.patched.dll` | All patch work lands here first |

**Forbidden bases for deploy:** `main.bak.unity3d`, vanilla `main.bak` DLL alone, DonorCompile `Update`, full customer V2 golden without load proof.

---

## 2. Deploy gate (all must pass before `bundle-inject`)

1. **Restore client bundle** — `restore-client.bat` or copy `client/main.unity3d` → game cache.
2. **Patch staging** — `apply-mission-patch-staging.bat` (never writes game cache directly).
3. **verify-il** — markers + required methods for patch tier.
4. **verify-load-safe** — `Update` IL **byte-identical** to client base (`ldc.i4.5` MainGame gate, MaxStack unchanged).
5. **No new types** in main DLL except patch helpers already in donor list.
6. **Explicit deploy** — only `apply-mission-patch-client-deploy.bat --tierN` after staging passes all gates (`run-verify-staging.bat`).
7. **Manifest** — always update hash/size + `file:///D:/work/roberto/.../main.unity3d`.

If any step fails: **do not inject**. Log in `ERROR-TRACKER.md`.

---

## 3. Patch tiers (what may be transplanted)

| Tier | Methods | Load risk | Client issues addressed |
|------|---------|-----------|-------------------------|
| **0** | None (client backup) | None | Baseline |
| **LITE** | Helpers + `ForceCompleteCurrentTask` + `TryForceCompleteChainTask` + `RequestForceCompleteTaskEnd` | Low | Fusion Lair `bError:true` on first hotkey |
| **SAFE+** | SAFE + `ForceCompleteOnEndFail` / `ForceCompleteOnStartSucc` surgical hooks | Medium — staging required | Instance 463 `bError:true`, retry err 1, advance chain on ProcessStartSucc |
| **LITE+EndFail** | LITE + full `ProcessEndFail` transplant | **BREAKS LOAD** | Do not use |
| **LITE+End*** | LITE + `ProcessEndFail` + `ProcessEndSucc` + `ProcessStartSucc` pending guard | High | Full chain — **blocked until LITE+EndFail loads** |
| **V2 full** | All donor async + `RequestTaskComplete` replace | **Proven break** | Do not use |

**Never transplant:** `Update`, `CallGuideMessage`, `NanoTimeEffect`, full-class `Edit Class` paste.

---

## 4. dnSpy / source file rules

- `step*.cs` and `add-class-members-compile-paste.txt` are **paste units**, not deploy artifacts.
- Donor source regenerates via `tools/FFPatch/DonorCompile/build-donor.ps1`.
- After editing `step*.cs`, regenerate donor before Cecil inject.

---

## 5. Log analysis (client acceptance)

Mission **504** chain: **466 → 468 → 463**.

| Log line | Meaning |
|----------|---------|
| `ForceCompleteV2: start chain task N` | Patch active |
| `ForceCompleteV2: instance zone complete task 463` | Fusion Lair fix |
| `ForceCompleteV2: retry task 463 err 1 attempt 2+` | Timer/instance retry |
| `ProcessEndFail : 463 Error Code : 1` then **no** `Fail Outgoing Task : 466` | EndFail hook working |
| `Fail Outgoing Task : 466` during force-complete | **Bug** — vanilla fallback still running |
| No `ForceCompleteV2:` at all | Patch not in deployed DLL |

---

## 6. Incident response

| Symptom | Action |
|---------|--------|
| Black screen / stall at Character Creation | `restore-client.bat` immediately — no patch debugging on live bundle |
| Log only `UdpLogger` + `configurableInput` | DLL broke startup — revert, log in ERROR-TRACKER |
| Hotkey does nothing | Checker gate / patch not deployed — see ERROR-TRACKER |
| 463 error 1 loop | Need LITE+EndFail minimum |

---

## 7. Agent / developer conduct

- **Do not** ask the client to revert or test deploys the agent can run locally.
- **Do not** auto-run `apply-mission-patch-client-deploy.bat`.
- **Do** update `ERROR-TRACKER.md` on every failed or successful staging run.
- **Do** fix root cause in tooling before retrying the same transplant strategy.

---

## 8. Commands (quick reference)

```bat
REM Safe — restore client working game
D:\work\roberto\tools\FFPatch\restore-client.bat

REM Patch to staging only (safe) — default tier 2
D:\work\roberto\tools\FFPatch\apply-mission-patch-staging.bat
D:\work\roberto\tools\FFPatch\apply-mission-patch-staging.bat --tier1
D:\work\roberto\tools\FFPatch\apply-mission-patch-staging.bat --tier3

REM Verify staging DLL
D:\work\roberto\tools\FFPatch\run-verify-staging.bat

REM Experimental live deploy (explicit only, after staging)
D:\work\roberto\tools\FFPatch\apply-mission-patch-client-deploy.bat --tier2
```
