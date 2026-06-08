"""Capture FusionFall Logger.Log output forwarded to UDP 127.0.0.1:5140.

Requires UdpLogger in the game's Assembly - CSharp - first pass.dll (client bundle).
Start this script before launching the game.
"""

import datetime
import os
import socket
import sys

HOST = "127.0.0.1"
PORT = 5140
FALLBACK_LOG = os.path.join(
    os.environ.get("LOCALAPPDATA", ""),
    "FusionFall",
    "logs",
    "udp_logger_fallback.log",
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
)

script_dir = os.path.dirname(os.path.abspath(__file__))
log_path = os.path.join(script_dir, "fusionfall_log.txt")

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
            # Copy only the tail on startup so we do not duplicate the full history every run.
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
    if os.path.isfile(FALLBACK_LOG):
        print(f"Also watching fallback: {FALLBACK_LOG}")
    else:
        print("No fallback log yet (game writes here if UDP send fails).")
    print("Highlight keywords:", ", ".join(HIGHLIGHT))
    print("-" * 60)

    with open(log_path, "a", encoding="utf-8") as logfile:
        tail_fallback_if_present(logfile)
        while True:
            data, _addr = sock.recvfrom(65536)
            msg = data.decode("utf-8", errors="replace").strip()
            timestamp = datetime.datetime.now().strftime("[%Y-%m-%d %H:%M:%S]")
            line = f"{timestamp} {msg}\n"
            logfile.write(line)
            logfile.flush()

            if any(key in msg for key in HIGHLIGHT):
                print(f">>> {line}", end="")
            else:
                print(line, end="")


if __name__ == "__main__":
    main()
