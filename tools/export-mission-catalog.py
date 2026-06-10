#!/usr/bin/env python3
"""Extract FusionFall mission table from TableData bundle and write per-mission docs."""

from __future__ import annotations

import csv
import json
import textwrap
from collections import defaultdict
from pathlib import Path

from unitypack.asset import Asset

ROOT = Path(__file__).resolve().parents[1]
TABLEDATA_BUNDLE = (
    ROOT
    / "6877b37c-e9cd-4826-b82c-5e8d3d5db744"
    / "tabledata_2eresourceFile"
    / "CustomAssetBundle-1dca92eecee4742d985b799d8226666d"
)
OUT_DIR = ROOT / "mods" / "docs" / "missions" / "catalog"
CSV_PATH = OUT_DIR / "mission-table-full.csv"
INDEX_PATH = ROOT / "mods" / "docs" / "missions" / "MISSION-CATALOG.md"
JSON_PATH = OUT_DIR / "mission-table-full.json"

MISSION_TYPES = {1: "Guide", 2: "Nano", 3: "Normal"}
TASK_TYPES = {
    1: "Talk",
    2: "GotoLocation",
    3: "UseItems",
    4: "Delivery",
    5: "Defeat",
    6: "EscortDefence",
}


def load_mission_table():
    if not TABLEDATA_BUNDLE.exists():
        raise SystemExit(f"Missing TableData bundle: {TABLEDATA_BUNDLE}")

    with TABLEDATA_BUNDLE.open("rb") as f:
        asset = Asset.from_file(f)
        xdt = asset.objects[7].contents
        mt = xdt["m_pMissionTable"]
        return mt["m_pMissionData"], mt["m_pMissionStringData"], mt["m_pJournalData"]


def mission_name(strings, idx: int) -> str:
    if not idx or idx >= len(strings):
        return ""
    return strings[idx].get("m_pstrNameString", "") or ""


def journal_summary(journal, strings, journal_id: int) -> str:
    if not journal_id or journal_id >= len(journal):
        return ""
    summary_id = journal[journal_id].get("m_iMissionSummary", 0)
    return mission_name(strings, summary_id)


def row_to_dict(row) -> dict:
    return {k: (list(v) if hasattr(v, "keys") and not isinstance(v, (str, bytes)) else v) for k, v in dict(row).items()}


def classify_task(row: dict) -> list[str]:
    tags = []
    if row["m_iSTGrantTimer"] > 0:
        tags.append(f"grant-timer={row['m_iSTGrantTimer']}s")
    if row["m_iCSUCheckTimer"] > 0:
        tags.append(f"complete-timer-gate={row['m_iCSUCheckTimer']}s")
    if row["m_iRequireInstanceID"] > 0:
        tags.append(f"instance-id={row['m_iRequireInstanceID']}")
    if any(row["m_iCSUEnemyID"]):
        tags.append("kill-quota")
    if any(row["m_iCSUItemID"]):
        tags.append("item-quota")
    if row["m_iHTaskType"] == 6:
        tags.append("escort")
    if row["m_iSUOutgoingTask"]:
        tags.append(f"chains-to={row['m_iSUOutgoingTask']}")
    if row["m_iFOutgoingTask"]:
        tags.append(f"fail-restart={row['m_iFOutgoingTask']}")
    return tags


def completion_rules(row: dict) -> list[str]:
    rules = []
    tt = TASK_TYPES.get(row["m_iHTaskType"], f"Type{row['m_iHTaskType']}")
    rules.append(f"Task type: **{tt}**")

    if row["m_iHNPCID"]:
        rules.append(f"Start at grant NPC table ID **{row['m_iHNPCID']}** (journal accept or auto-chain).")
    else:
        rules.append("Start: auto-chain or journal (no grant NPC table ID).")

    if row["m_iHTerminatorNPCID"]:
        rules.append(f"Complete at terminator NPC table ID **{row['m_iHTerminatorNPCID']}** (proximity/talk).")
    else:
        rules.append("Complete: auto when quotas met, timer expires, or remote packet.")

    if row["m_iSTGrantTimer"] > 0:
        rules.append(
            f"Grant timer **{row['m_iSTGrantTimer']}**: server sends `iRemainTime` on start; client auto-sends "
            "`TASK_END` when `m_fRemainTime` reaches 0."
        )
    if row["m_iCSUCheckTimer"] > 0:
        rules.append(
            f"Completion gate timer **{row['m_iCSUCheckTimer']}**: `CheckToCompleteTaskCondition` returns Fail until elapsed."
        )
    if row["m_iRequireInstanceID"] > 0:
        rules.append(
            f"Instance required (**m_iRequireInstanceID={row['m_iRequireInstanceID']}**): server validates zone; "
            "complete outside instance → `ProcessEndFail` error 1; warp-out sends `TASK_END` with `bError=true`."
        )

    enemies = [(row["m_iCSUEnemyID"][i], row["m_iCSUNumToKill"][i]) for i in range(3) if row["m_iCSUEnemyID"][i]]
    if enemies:
        rules.append("Kill quotas: " + ", ".join(f"enemy {e} x{n}" for e, n in enemies))

    items = [(row["m_iCSUItemID"][i], row["m_iCSUItemNumNeeded"][i]) for i in range(3) if row["m_iCSUItemID"][i]]
    if items:
        rules.append("Collect items: " + ", ".join(f"item {i} x{n}" for i, n in items))

    if row["m_iCSUDEFNPCID"]:
        rules.append(f"Escort NPC table ID **{row['m_iCSUDEFNPCID']}**; death → `TASK_END` with `bError=true`.")

    if row["m_iSUReward"]:
        rules.append(f"Reward table ID **{row['m_iSUReward']}** (journal reward UI before `TASK_END`).")

    return rules


def packet_block(row: dict) -> str:
    return textwrap.dedent(
        f"""
        **Start packet** (`sP_CL2FE_REQ_PC_TASK_START`, opcode 318767115):
        - `iTaskNum` = {row['m_iHTaskID']}
        - `iNPC_ID` = runtime ID from grant NPC {row['m_iHNPCID'] or 0}
        - `iEscortNPC_ID` = escort runtime ID if type 6

        **End packet** (`sP_CL2FE_REQ_PC_TASK_END`, opcode 318767116):
        - `iTaskNum` = {row['m_iHTaskID']}
        - `iNPC_ID` = runtime ID from terminator NPC {row['m_iHTerminatorNPCID'] or 0}
        - `iEscortNPC_ID` = escort runtime or **-1** if `bError=true`
        - `iBox1Choice` / `iBox2Choice` = reward bits if reward UI shown
        """
    ).strip()


def write_mission_md(mid: int, tasks: list[dict], strings, journal) -> None:
    tasks_by_id = {t["m_iHTaskID"]: t for t in tasks}
    mtype = MISSION_TYPES.get(tasks[0]["m_iHMissionType"], str(tasks[0]["m_iHMissionType"]))
    mname = mission_name(strings, tasks[0]["m_iHMissionName"])

    has_timer = any(t["m_iSTGrantTimer"] or t["m_iCSUCheckTimer"] for t in tasks)
    has_instance = any(t["m_iRequireInstanceID"] for t in tasks)

    lines = [
        f"# Mission {mid} — {mname or '(unnamed)'}",
        "",
        f"| Field | Value |",
        f"|-------|-------|",
        f"| Mission ID | {mid} |",
        f"| Mission type | {mtype} |",
        f"| Task count | {len(tasks)} |",
        f"| Has timer tasks | {'Yes' if has_timer else 'No'} |",
        f"| Has instance tasks | {'Yes' if has_instance else 'No'} |",
        "",
        "## Task index",
        "",
        "| Task ID | Type | Instance | Timers | Success → | Fail → | Grant NPC | Terminator |",
        "|---------|------|----------|--------|-----------|--------|-----------|------------|",
    ]

    for t in sorted(tasks, key=lambda x: x["m_iHTaskID"]):
        timers = []
        if t["m_iSTGrantTimer"]:
            timers.append(f"grant:{t['m_iSTGrantTimer']}")
        if t["m_iCSUCheckTimer"]:
            timers.append(f"gate:{t['m_iCSUCheckTimer']}")
        timer_s = ", ".join(timers) or "—"
        lines.append(
            f"| {t['m_iHTaskID']} | {TASK_TYPES.get(t['m_iHTaskType'], t['m_iHTaskType'])} | "
            f"{t['m_iRequireInstanceID'] or '—'} | {timer_s} | "
            f"{t['m_iSUOutgoingTask'] or '—'} | {t['m_iFOutgoingTask'] or '—'} | "
            f"{t['m_iHNPCID'] or '—'} | {t['m_iHTerminatorNPCID'] or '—'} |"
        )

    lines.extend(["", "## Chain edges (from table)", ""])
    edges = []
    for t in tasks:
        if t["m_iSUOutgoingTask"]:
            edges.append(f"- Success: **{t['m_iHTaskID']}** → **{t['m_iSUOutgoingTask']}**")
        if t["m_iFOutgoingTask"]:
            edges.append(f"- Fail (err 1/12): **{t['m_iHTaskID']}** → **{t['m_iFOutgoingTask']}**")
    lines.extend(edges or ["- (no outgoing edges in table)"])

    for t in sorted(tasks, key=lambda x: x["m_iHTaskID"]):
        lines.extend([
            "",
            f"## Task {t['m_iHTaskID']}",
            "",
            f"**Tags:** {', '.join(classify_task(t)) or 'standard'}",
            "",
            "### How this task is received",
            "",
        ])
        if t["m_iHNPCID"]:
            lines.append(f"- Player accepts from NPC table ID **{t['m_iHNPCID']}** via journal (`cnEvent(12,5)`).")
        else:
            lines.append("- Started by auto-chain from prior task `ProcessEndSucc` → `RequestTaskStart`.")
        if t["m_iCSTTrigger"]:
            lines.append(f"- Requires active trigger task **{t['m_iCSTTrigger']}** before start.")
        for i, prereq in enumerate(t["m_iCSTReqMission"]):
            if prereq:
                lines.append(f"- Prerequisite completed mission **{prereq}**.")

        lines.extend(["", "### How this task must be completed", ""])
        lines.extend(f"- {r}" for r in completion_rules(t))

        lines.extend(["", "### Server packets", "", packet_block(t)])

        jid = t.get("m_iSTJournalIDAdd", 0)
        if jid:
            summary = journal_summary(journal, strings, jid)
            if summary:
                lines.extend(["", "### Journal summary", "", summary])

    path = OUT_DIR / f"MISSION-{mid}.md"
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def main() -> None:
    data, strings, journal = load_mission_table()
    OUT_DIR.mkdir(parents=True, exist_ok=True)

    by_mission: dict[int, list[dict]] = defaultdict(list)
    rows = []
    for raw in data:
        row = row_to_dict(raw)
        mid = row["m_iHMissionID"]
        if mid:
            by_mission[mid].append(row)
        rows.append(row)

    # CSV
    fieldnames = list(rows[1].keys()) if len(rows) > 1 else list(rows[0].keys())
    with CSV_PATH.open("w", newline="", encoding="utf-8") as f:
        w = csv.DictWriter(f, fieldnames=fieldnames, extrasaction="ignore")
        w.writeheader()
        for row in rows:
            flat = {}
            for k, v in row.items():
                flat[k] = json.dumps(v) if isinstance(v, list) else v
            w.writerow(flat)

    # JSON (for tooling)
    JSON_PATH.write_text(json.dumps(rows, indent=2), encoding="utf-8")

    # Per-mission markdown
    for mid in sorted(by_mission.keys()):
        write_mission_md(mid, by_mission[mid], strings, journal)

    # Index
    timer_missions = sorted(m for m, ts in by_mission.items() if any(t["m_iSTGrantTimer"] or t["m_iCSUCheckTimer"] for t in ts))
    instance_missions = sorted(m for m, ts in by_mission.items() if any(t["m_iRequireInstanceID"] for t in ts))

    index_lines = [
        "# Mission catalog (complete)",
        "",
        f"**Source:** `TableData.resourceFile` → `xdtdatas` → `m_pMissionTable`",
        f"**Extracted:** {len(rows)} tasks across **{len(by_mission)}** mission groups",
        "",
        "## Data files",
        "",
        f"- [`catalog/mission-table-full.csv`](catalog/mission-table-full.csv) — all tasks, all fields",
        f"- [`catalog/mission-table-full.json`](catalog/mission-table-full.json) — machine-readable",
        f"- [`catalog/MISSION-{{id}}.md`](catalog/) — one doc per mission (**{len(by_mission)}** files)",
        "",
        "## Statistics",
        "",
        f"| Metric | Count |",
        f"|--------|------:|",
        f"| Mission groups | {len(by_mission)} |",
        f"| Task rows | {len(rows)} |",
        f"| Missions with timer tasks | {len(timer_missions)} |",
        f"| Missions with instance tasks | {len(instance_missions)} |",
        "",
        "## Timer missions",
        "",
    ]
    for m in timer_missions:
        name = mission_name(strings, by_mission[m][0]["m_iHMissionName"])
        index_lines.append(f"- [Mission {m}](catalog/MISSION-{m}.md) — {name}")

    index_lines.extend(["", "## Instance / Fusion Lair / IZ missions", ""])
    for m in instance_missions:
        name = mission_name(strings, by_mission[m][0]["m_iHMissionName"])
        inst_ids = sorted({t["m_iRequireInstanceID"] for t in by_mission[m] if t["m_iRequireInstanceID"]})
        index_lines.append(f"- [Mission {m}](catalog/MISSION-{m}.md) — {name} (instance {inst_ids})")

    index_lines.extend(["", "## All missions (by ID)", ""])
    for m in sorted(by_mission.keys()):
        name = mission_name(strings, by_mission[m][0]["m_iHMissionName"])
        ntasks = len(by_mission[m])
        index_lines.append(f"- [Mission {m}](catalog/MISSION-{m}.md) — {name} ({ntasks} tasks)")

    INDEX_PATH.write_text("\n".join(index_lines) + "\n", encoding="utf-8")

    print(f"Wrote {len(by_mission)} mission docs to {OUT_DIR}")
    print(f"CSV: {CSV_PATH}")
    print(f"Index: {INDEX_PATH}")


if __name__ == "__main__":
    main()
