# Active error queue (magazine)

**Rules:** [`CONTINUOUS-ERROR-TRACKING.md`](CONTINUOUS-ERROR-TRACKING.md)

- Only **one** `IN_PROGRESS` error at a time.
- New errors during verify → append here as `QUEUED`; do not switch focus.
- When focused error → `VERIFIED_FIXED`, promote the **top** `QUEUED` item.

**Current bundle (canonical):** `main 2.unity3d` — size=7113528 hash=`15388e0b01c7cc256a4b2ee4c8a2135bc6faefa1e136402ac4ed40472bd29873`

---

## IN_PROGRESS

*(none — assign next from QUEUED after session start)*

---

## QUEUED (magazine — work top to bottom)

### ERR-001 — Fusion Lair task 463: ProcessEndFail err 1 → Fail Outgoing 466 loop

| Field | Value |
|-------|-------|
| State | QUEUED |
| Requirement | Fusion Lair / Infected Zone (client_requirement §2) |
| Discovered | 2026-06-08 |
| Discovered while | initial intake (client log 22:34) |
| Bundle | 7113528 / `15388e0b…` (client `main 2.unity3d`) |

**Evidence:**

```
ProcessStartSucc : 463
ProcessEndFail : 463 Error Code : 1
Fail Outgoing Task : 466
ProcessStartSucc : 466
ProcessStartFail tasknumber : 463
```

See also `ERROR-TRACKER.md` — client uses `ForceCompleteCurrentTask` + `PrepareTaskForForcedComplete` (instance map hack), not `ForceCompleteV2`.

**Hypothesis:** Server rejects complete outside instance; vanilla fail-outgoing restarts 466. Client’s `ownstatus.iInsMapNum` hack may be insufficient without warp/NPC path or correct `bError` on packet.

**Fix attempts:**

| # | Date | Change | Result |
|---|------|--------|--------|
| — | — | — | — |

**Verification:**

- [ ] Mission 504, tasks 466→468→463, one autocomplete action
- [ ] No `Fail Outgoing Task : 466` during chain OR documented acceptable server path
- [ ] Task 463 reaches success or designed retry (log excerpt)
- [ ] Bundle hash unchanged unless ingest of new client file

---

### ERR-002 — bundle-inject shrinks main.unity3d and breaks load (assetInfo hang)

| Field | Value |
|-------|-------|
| State | QUEUED |
| Requirement | Game must load (client_requirement implicit) |
| Discovered | 2026-06-07 |
| Discovered while | ERR-001 (during deploy verify) |
| Bundle | inject output 7099507 vs client 7125105+ |

**Evidence:** Stuck at `Requesting to assetInfo.php` without `Resources base url`; size gate failed on deploy.

**Hypothesis:** Full `bundle-inject` recompress loses data or breaks bootstrap; cannot deploy DonorCompile SAFE DLL over client `ClientMod` bundle until inject path fixed or DLL swapped without shrink.

**Fix attempts:**

| # | Date | Change | Result |
|---|------|--------|--------|
| 1 | 2026-06-07 | Size gate in deploy script | Blocks bad deploy; no inject fix yet |

**Verification:**

- [ ] Patched DLL deploys; bundle size ≥ client base − 1%
- [ ] `CreateGameMode:2` after Connect
- [ ] `ClientMod` strings still present in DLL

---

### ERR-003 — Timer missions: autocomplete fails / return to NPC (error 1)

| Field | Value |
|-------|-------|
| State | QUEUED |
| Requirement | timer (client_requirement §1) |
| Discovered | initial intake |
| Discovered while | initial intake |
| Bundle | client `main 2.unity3d` |

**Evidence:** Customer requirement; client `PrepareTaskForForcedComplete` sets `SetRemainTime(99999f)` — needs dedicated repro on timer task.

**Hypothesis:** Timer not zeroed/deferred before complete packet; server returns error 1.

**Verification:**

- [ ] Named timer mission ID tested
- [ ] No “return to NPC” / error 1 without retry
- [ ] Log shows timer defer or forced complete path

---

## PARKED

*(none)*

---

## VERIFIED_FIXED (recent)

Move cards here when closed; copy summary to `PATCH-ERROR-LOG.md` if process mistake; detail to `ERROR-TRACKER.md`.

### ERR-000 — Workspace / wrong bundle lineage (vanilla ClientFile vs client main 2)

| Closed | 2026-06-08 |
|--------|------------|
| Fix | `ingest-client-bundle.bat`; cleanup; decompile to `mods/decompiled`; client `ClientMod` + noclip confirmed in DLL |

---

## ID counter

Next ID: **ERR-004**
