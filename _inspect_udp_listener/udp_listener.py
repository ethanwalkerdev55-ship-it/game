"""Capture FusionFall Logger.Log output forwarded to UDP 127.0.0.1:5140.

Requires UdpLogger in the game's Assembly - CSharp - first pass.dll (client bundle).
Start this script before launching the game.

Bootstrap note: "Requesting to assetInfo.php" is logged when the WWW request STARTS.
Failures (missing file, NPAPI error, injected bundle hang) are usually silent — this
listener emits BOOTSTRAP_STALL diagnostics when Resources base url never arrives.
"""

import datetime
import json
import os
import socket
import sys
import threading

HOST = "127.0.0.1"
PORT = 5140
FALLBACK_LOG = os.path.join(
    os.environ.get("LOCALAPPDATA", ""),
    "FusionFall",
    "logs",
    "udp_logger_fallback.log",
)
FFRUNNER_LOG = os.path.join(
    os.environ.get("LOCALAPPDATA", ""),
    "OpenFusionLauncher",
    "ffrunner.log",
)
HIGHLIGHT = (
    "ForceCompleteV2",
    "ProcessEndSucc",
    "ProcessEndFail",
    "ProcessStartSucc",
    "ProcessStartFail",
    "sent complete packet",
    "complete blocked",
    "REQ_PC_TASK",
    "TASK_END",
    "BOOTSTRAP_STALL",
    "BOOTSTRAP_OK",
)

BOOTSTRAP_STALL_SECONDS = 2.0
SESSION_MARKERS = ("UdpLogger initialized", "Load guixdt files")
ASSETINFO_START = "Requesting to assetInfo.php"
RESOURCES_OK = "Resources base url"
LOGIN_OK = "Connecting to loginInfo.php"
CREATE_GAME_MODE_2 = "CreateGameMode:2"

script_dir = os.path.dirname(os.path.abspath(__file__))
log_path = os.path.join(script_dir, "fusionfall_log.txt")
context_path = os.path.join(script_dir, "bootstrap_context.json")


class BootstrapWatchdog:
    def __init__(self, logfile):
        self._logfile = logfile
        self._lock = threading.Lock()
        self._session = 0
        self._assetinfo_pending = False
        self._timer = None

    def _write(self, text, highlight=True):
        timestamp = datetime.datetime.now().strftime("[%Y-%m-%d %H:%M:%S]")
        line = f"{timestamp} {text}\n"
        self._logfile.write(line)
        self._logfile.flush()
        if highlight:
            print(f">>> {line}", end="")
        else:
            print(line, end="")

    def _cancel_timer(self):
        if self._timer is not None:
            self._timer.cancel()
            self._timer = None

    def _load_context(self):
        if not os.path.isfile(context_path):
            return None
        try:
            with open(context_path, "r", encoding="utf-8") as src:
                return json.load(src)
        except (OSError, json.JSONDecodeError):
            return None

    def _tail_ffrunner(self):
        if not os.path.isfile(FFRUNNER_LOG):
            return "ffrunner.log: (missing — launcher may not have run yet)"
        try:
            with open(FFRUNNER_LOG, "r", encoding="utf-8", errors="replace") as src:
                src.seek(0, os.SEEK_END)
                size = src.tell()
                start = max(0, size - 8192)
                src.seek(start)
                chunk = src.read().strip()
            if not chunk:
                return "ffrunner.log: (empty)"
            lines = chunk.splitlines()
            interesting = [
                ln
                for ln in lines
                if any(
                    key in ln.lower()
                    for key in (
                        "assetinfo",
                        "rankurl",
                        "urlnotify",
                        "network",
                        "error",
                        "ready to load main",
                        "main.unity3d",
                    )
                )
            ]
            tail = interesting[-12:] if interesting else lines[-8:]
            return "ffrunner.log tail:\n" + "\n".join(tail)
        except OSError as exc:
            return f"ffrunner.log: (unreadable: {exc})"

    def _emit_stall(self):
        with self._lock:
            if not self._assetinfo_pending:
                return
            self._assetinfo_pending = False
            self._timer = None

        ctx = self._load_context()
        parts = [
            "[DIAG] BOOTSTRAP_STALL: local WWW for assetInfo.php never completed.",
            "The game log line 'Requesting to assetInfo.php' is NOT the error — it only marks request start.",
            "Typical causes: (1) assetInfo.php missing from Unity dataPath/ffcache, (2) manifest CDN revert,",
            "(3) injected main.unity3d breaks NPAPI/local file reads (bundle passes disunity but hangs at runtime).",
        ]
        if ctx:
            offline = ctx.get("offline", {})
            ffcache = ctx.get("ffcache", {})
            manifest = ctx.get("manifest", {})
            ob = offline.get("main.unity3d", {})
            fb = ffcache.get("main.unity3d", {})
            parts.append(
                f"context offline bundle: size={ob.get('size')} hash={ob.get('hash')}"
            )
            parts.append(
                f"context ffcache bundle: size={fb.get('size')} hash={fb.get('hash')}"
            )
            parts.append(f"context manifest url: {manifest.get('main_file_url')}")
            parts.append(f"context manifest hash: {manifest.get('main_hash')}")
            if ob.get("hash") and fb.get("hash") and ob.get("hash") != fb.get("hash"):
                parts.append("WARN: offline and ffcache bundle hashes differ at snapshot time")
            if manifest.get("main_hash") and ob.get("hash") and manifest.get("main_hash") != ob.get("hash"):
                parts.append("WARN: manifest hash did not match offline bundle at snapshot time")
        parts.append(self._tail_ffrunner())
        parts.append(f"Full snapshot: {context_path}")
        parts.append("Run before Connect: tools\\FFPatch\\bootstrap-snapshot.bat")
        self._write("\n".join(parts))

    def on_session_start(self):
        with self._lock:
            self._session += 1
            self._assetinfo_pending = False
            self._cancel_timer()

    def on_message(self, msg):
        if RESOURCES_OK in msg:
            with self._lock:
                self._assetinfo_pending = False
                self._cancel_timer()
            self._write("[DIAG] BOOTSTRAP_OK: Resources base url received — bootstrap passed.")
            return

        if ASSETINFO_START in msg:
            with self._lock:
                self._assetinfo_pending = True
                self._cancel_timer()
                self._timer = threading.Timer(BOOTSTRAP_STALL_SECONDS, self._emit_stall)
                self._timer.daemon = True
                self._timer.start()
            return

        if CREATE_GAME_MODE_2 in msg:
            self._write("[DIAG] BOOTSTRAP_OK: CreateGameMode:2 — login flow completed.")


def bind_listener():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    try:
        sock.bind((HOST, PORT))
    except OSError as exc:
        print(f"ERROR: cannot bind {HOST}:{PORT} — {exc}", file=sys.stderr)
        print("Another listener may already be running on port 5140.", file=sys.stderr)
        sys.exit(1)
    return sock


def tail_fallback_if_present(logfile):
    if not os.path.isfile(FALLBACK_LOG):
        return
    try:
        with open(FALLBACK_LOG, "r", encoding="utf-8", errors="replace") as src:
            src.seek(0, os.SEEK_END)
            size = src.tell()
            start = max(0, size - 65536)
            src.seek(start)
            chunk = src.read()
        if chunk.strip():
            logfile.write(f"\n--- fallback log tail ({FALLBACK_LOG}) ---\n")
            logfile.write(chunk)
            if not chunk.endswith("\n"):
                logfile.write("\n")
            logfile.flush()
            print(f"Imported tail from fallback log: {FALLBACK_LOG}")
    except OSError as exc:
        print(f"Note: could not read fallback log: {exc}")


def main():
    sock = bind_listener()
    print(f"Listening on {HOST}:{PORT}")
    print(f"Writing to {log_path}")
    print(
        "Bootstrap watchdog: stalls after "
        f"'{ASSETINFO_START}' without '{RESOURCES_OK}' in {BOOTSTRAP_STALL_SECONDS}s"
    )
    if os.path.isfile(FALLBACK_LOG):
        print(f"Also watching fallback: {FALLBACK_LOG}")
    else:
        print("No fallback log yet (game writes here if UDP send fails).")
    print("Highlight keywords:", ", ".join(HIGHLIGHT))
    print("-" * 60)

    watchdog = None
    with open(log_path, "a", encoding="utf-8") as logfile:
        watchdog = BootstrapWatchdog(logfile)
        tail_fallback_if_present(logfile)
        while True:
            data, _addr = sock.recvfrom(65536)
            msg = data.decode("utf-8", errors="replace").strip()
            timestamp = datetime.datetime.now().strftime("[%Y-%m-%d %H:%M:%S]")
            line = f"{timestamp} {msg}\n"
            logfile.write(line)
            logfile.flush()

            if any(marker in msg for marker in SESSION_MARKERS):
                watchdog.on_session_start()

            watchdog.on_message(msg)

            if any(key in msg for key in HIGHLIGHT):
                print(f">>> {line}", end="")
            else:
                print(line, end="")


if __name__ == "__main__":
    main()
