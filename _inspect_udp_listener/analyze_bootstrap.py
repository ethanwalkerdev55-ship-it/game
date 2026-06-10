"""Count bootstrap stalls: assetInfo request without Resources base url in same session."""

import sys

path = sys.argv[1] if len(sys.argv) > 1 else "fusionfall_log.txt"
sessions = []
cur = None

with open(path, encoding="utf-8", errors="replace") as f:
    for i, line in enumerate(f, 1):
        if "UdpLogger initialized" in line or "Load guixdt files" in line:
            if cur:
                sessions.append(cur)
            cur = {"start": i, "assetinfo": None, "resources": None}
        if cur is None:
            continue
        if "Requesting to assetInfo.php" in line and cur["assetinfo"] is None:
            cur["assetinfo"] = i
        if "Resources base url" in line:
            cur["resources"] = i

if cur:
    sessions.append(cur)

with_assetinfo = [s for s in sessions if s["assetinfo"]]
ok = [s for s in with_assetinfo if s["resources"]]
stalls = [s for s in with_assetinfo if not s["resources"]]

print(f"sessions with assetInfo request: {len(with_assetinfo)}")
print(f"bootstrap OK (Resources base): {len(ok)}")
print(f"bootstrap STALL (no Resources): {len(stalls)}")
if stalls:
    print("true stalls:")
    for s in stalls:
        print(f"  assetInfo line {s['assetinfo']} (session start {s['start']})")
