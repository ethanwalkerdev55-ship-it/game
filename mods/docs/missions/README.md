# Per-mission documentation

Every mission group in the FusionFall mission table is documented here — **747 missions**, **2866 tasks**, extracted from `TableData.resourceFile`.

## Complete catalog

| Resource | Description |
|----------|-------------|
| [**MISSION-CATALOG.md**](MISSION-CATALOG.md) | Master index — all missions, timer list, instance list |
| [**catalog/MISSION-{id}.md**](catalog/) | One execution doc per mission (receive, complete, packets, chain) |
| [**catalog/mission-table-full.csv**](catalog/mission-table-full.csv) | All task rows, all `MissionElement` fields |
| [**catalog/mission-table-full.json**](catalog/mission-table-full.json) | Same data, machine-readable |

**Regenerate** after TableData changes:

```bat
python tools\export-mission-catalog.py
```

**Source bundle:** `6877b37c-e9cd-4826-b82c-5e8d3d5db744\tabledata_2eresourceFile\CustomAssetBundle-1dca92eecee4742d985b799d8226666d` (object 7 = `xdtdatas` → `m_pMissionTable`).

---

## Generic logic (applies to every mission)

| Doc | Content |
|-----|---------|
| [../MISSION-EXECUTION-LOGIC.md](../MISSION-EXECUTION-LOGIC.md) | `cnMissionManager` receive/start/complete flow |
| [../MISSION-ELEMENT-SCHEMA.md](../MISSION-ELEMENT-SCHEMA.md) | All table fields |
| [../MISSION-PACKET-PROTOCOL.md](../MISSION-PACKET-PROTOCOL.md) | Opcodes and packet layouts |

---

## By-type references

| Type | Doc | Count in table |
|------|-----|----------------|
| Timer missions | [by-type/TIMER-MISSIONS.md](by-type/TIMER-MISSIONS.md) | 73 mission groups |
| Instance / Fusion Lair / IZ | [by-type/INSTANCE-ZONE-MISSIONS.md](by-type/INSTANCE-ZONE-MISSIONS.md) | 99 mission groups |
| Task types | [by-type/TASK-TYPE-REFERENCE.md](by-type/TASK-TYPE-REFERENCE.md) | Talk, Defeat, Goto, … |

---

## Worked example with log evidence

| Mission | Catalog doc | Log analysis |
|---------|-------------|--------------|
| **504** — Practice Pranks | [catalog/MISSION-504.md](catalog/MISSION-504.md) | [MISSION-504.md](MISSION-504.md) (vanilla fail loop, tasks 466→468→463) |

Mission 504 has **8 tasks** (461–465, 466, 468, 667). The autocomplete test chain is **466 → 468 → 463** (instance 12, kill enemy 2513 ×4); fail on 463 restarts 466.

---

## Log field reference

```
active task id : {task} mission id : {mission} slot : {0-8}
Send Start Mission : {task}
ProcessStartSucc : {task}
ProcessEndFail : {task} Error Code : {code}
Fail Outgoing Task : {failOutgoingTask}
```
