# FusionFall mission patch — error log

Track every mistake so we do not repeat it. Update this file when a deploy fails or a root cause is confirmed.

**Queue / focus rules:** [`../../CONTINUOUS-ERROR-TRACKING.md`](../../CONTINUOUS-ERROR-TRACKING.md)

| Date | Symptom | Root cause | Fix / prevention |
|------|---------|------------|------------------|
| 2026-06-06 | F11 no chain progress | V9 "already applied" skipped V6 Del removal | PatchSafe always runs; never rely on skip-if-applied for critical fixes |
| 2026-06-06 | Crash after "request end" | PatchSafe injected corrupt IL into RequestForceCompleteTaskEnd prologue | verify-il + no abort-log injection; client path drops timer stack entirely |
| 2026-06-06 | Stall after "complete without npc gate" | RequestForceCompleteTaskEnd re-called GetMe() → null | Client path: TryForceComplete calls RequestTaskComplete directly |
| 2026-06-06 | V12 stray ldarg before inline block | Cecil insert order wrong | dumpil TryForceCompleteChainTask before every deploy |
| 2026-06-06 | "Donor missing method" noise every apply | Legacy TransplantMethodNames included V4–V8 methods not in donor | Client transplant list: 6 methods only |
| 2026-06-07 | 463 rollback on single Right Ctrl | Client-only patch lacked ProcessEndSucc chain + ProcessEndFail retry | Customer golden: full ForceCompleteV2 stack via DonorCompile |
| 2026-06-07 | Timer / Fusion Lair not handled | main.current used direct RequestTaskComplete only | RequestForceCompleteTaskEnd + instance blocker + Update timer defer |
| 2026-06-07 | Black screen on load after customer V2 | Full Update transplant from DonorCompile: eGameMode.MainGame = 0 in stub vs 5 in game → ReceiveStartGames never runs | Never transplant Update; restore vanilla Update + PatchCustomerUpdate timer defer only; verify-il ldc.i4.5 gate |
| 2026-06-07 | Black screen persists after Update fix | DonorCompile ProcessStartSucc/ProcessStartFail/RequestTaskComplete transplants on vanilla base still break load; Apply() partial transplant ≠ golden | Hybrid base: main.current; inject EndSucc/EndFail/TryForceComplete/helpers only; deploy full golden copy |
| 2026-06-07 | Black screen after vanilla-base customer deploy | ProcessEndSucc/ProcessEndFail donor inject on vanilla base breaks load (Update gate OK but runtime init fails) | Customer golden base must be main.current hybrid; restore main.bak.unity3d before each inject |
| 2026-06-07 | Black screen after apply-client-v2 on client bundle | Full ProcessStart*/ProcessEnd* + RequestTaskComplete donor transplant breaks load (verify-il passes) | Use apply-client-lite only; never deploy V2 full stack until load proven |

## Recurring mistake pattern

**Stacking unverified Cecil layers on a fragile golden lineage** instead of transplanting a known-working simpler mod (`main.current`) and applying one audited hotfix (chain bypass).

## Current approved lineage

1. **Client bundle base:** `_inspect_bundle/client/main.unity3d` (from `ClientFile/` — includes UdpLogger in first pass DLL)
2. **Patch base DLL:** `_inspect_bundle/client/Assembly - CSharp.dll` (client working state, not `main.bak`)
3. **Donor:** `_inspect_bundle/main.current/Assembly - CSharp.dll` → `golden/Assembly-CSharp.client.dll`
4. Post-transplant: `PatchSafeClient` (checker gate removal + RequestTaskComplete chain bypass)
5. Gate: `FFPatch verify-il` must pass before `bundle-inject`
6. **Deploy:** `tools/FFPatch/apply-mission-patch-client.bat` (never restore `main.bak.unity3d` — strips UdpLogger)

## Log acceptance (mission 504 / task 466)

After F11, UDP log should show server traffic for task end (no legacy ForceCompleteV2 timer strings required). Failure without server packet → run `dumpil` on deployed DLL before adding another patch layer.
