# Mission system documentation

Comprehensive analysis of FusionFall client mission execution from decompiled `main.unity3d`.

## Start here

| Document | Description |
|----------|-------------|
| [MISSION-EXECUTION-LOGIC.md](MISSION-EXECUTION-LOGIC.md) | **Master reference** — receive, start, complete, timer, instance, chaining |
| [MISSION-ELEMENT-SCHEMA.md](MISSION-ELEMENT-SCHEMA.md) | All `MissionElement` table fields |
| [MISSION-PACKET-PROTOCOL.md](MISSION-PACKET-PROTOCOL.md) | CL2FE / FE2CL packet layouts and opcodes |
| [missions/MISSION-CATALOG.md](missions/MISSION-CATALOG.md) | **Complete catalog** — 747 missions, 2866 tasks |
| [missions/README.md](missions/README.md) | Per-mission index |

## By topic

| Topic | Path |
|-------|------|
| Timer missions | [missions/by-type/TIMER-MISSIONS.md](missions/by-type/TIMER-MISSIONS.md) |
| Instance / Fusion Lair / IZ | [missions/by-type/INSTANCE-ZONE-MISSIONS.md](missions/by-type/INSTANCE-ZONE-MISSIONS.md) |
| Task types (Talk, Defeat, …) | [missions/by-type/TASK-TYPE-REFERENCE.md](missions/by-type/TASK-TYPE-REFERENCE.md) |
| Mission 504 (test case) | [missions/MISSION-504.md](missions/MISSION-504.md) |

## Source artifacts

| Artifact | Location |
|----------|----------|
| Decompiled game logic | `mods/decompiled/Assembly-CSharp/` (local, gitignored) |
| `MissionElement` class | `mods/docs/_tmp_firstpass/MissionElement.decompiled.cs` |
| Mission table data | `TableData.resourceFile` → `catalog/mission-table-full.csv` |
| Catalog export tool | `tools/export-mission-catalog.py` |
| UDP test logs | `_inspect_udp_listener/fusionfall_log.txt` |

## Regenerate decompiled code

```bat
tools\FFPatch\ingest-client-bundle.bat
```

Extracts `main.unity3d`, syncs DLLs, exports C# to `mods/decompiled/`.
