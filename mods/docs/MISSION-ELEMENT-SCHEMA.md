# MissionElement — complete field schema

**Class:** `MissionElement` in `Assembly - CSharp - first pass.dll`  
**Table:** `MissionTableScript.m_pMissionData[]` (TableContainer index **7**)  
**Decompiled:** `mods/docs/_tmp_firstpass/MissionElement.decompiled.cs`

Each array element is **one task** (one step). Tasks with the same `m_iHMissionID` form a mission group (ordered children in `cnMissionNode` tree).

---

## Identity

| Field | Type | Description |
|-------|------|-------------|
| `m_iHMissionType` | int | 1=Guide, 2=Nano, 3=Normal (`eMissionTypeProperty`) |
| `m_iHMissionID` | int | Mission group ID |
| `m_iHMissionName` | int | String table index → `m_pMissionStringData` |
| `m_iHTaskType` | int | 1=Talk, 2=GotoLocation, 3=UseItems, 4=Delivery, 5=Defeat, 6=EscortDefence |
| `m_iHTaskID` | int | **Unique task ID** (used in all packets) |
| `m_iHNPCID` | int | Grant NPC table ID (start) |
| `m_iHJournalNPCID` | int | Journal display NPC |
| `m_iHTerminatorNPCID` | int | Complete-at NPC table ID (0 = auto/remote complete) |
| `m_iHDifficultyType` | int | Difficulty classification |
| `m_iHBarkerTextID` | int[4] | Barker dialog per NPC barker type |
| `m_iHCurrentObjective` | int | UI objective index |
| `m_iRequireInstanceID` | int | **>0 = instance/Fusion Lair/IZ task** (server validates zone) |
| `m_iRepeatflag` | int | Repeat quest bit flag |

---

## Start conditions (CST*)

| Field | Description |
|-------|-------------|
| `m_iKorStReqLvlMin` | Min level (Korean expansion) |
| `m_iCTRReqLvMin` | Min level |
| `m_iCTRReqLvMax` | Max level |
| `m_iCSTRReqNano` | int[5] — required nano IDs in bank |
| `m_iCSTReqGuide` | Required guide/mentor ID |
| `m_iCSTReqMission` | int[2] — prerequisite **mission IDs** completed |
| `m_iCSTEntranceGroupMin` | Group size min |
| `m_iCSTEntranceGroupMax` | Group size max |
| `m_iCSTItemID` | int[3] — required items to start |
| `m_iCSTItemNumNeeded` | int[3] — counts |
| `m_iCSTTrigger` | Task ID that must be active to unlock start |

---

## Success conditions (CSU*)

| Field | Description |
|-------|-------------|
| `m_iCSUCheckTimer` | **Completion timer gate** — must wait before complete allowed |
| `m_iCSUEnemyID` | int[3] — enemy NPC table IDs to kill |
| `m_iCSUNumToKill` | int[3] — kill counts |
| `m_iCSUItemID` | int[3] — collect items |
| `m_iCSUItemNumNeeded` | int[3] — item counts |
| `m_iCSUDEFNPCID` | Escort NPC table ID (type 6) |
| `m_iCSUDEFNPCAI` | Escort AI |
| `m_iCSUDEPNPCFollow` | Follow behavior |

---

## Start grant (ST*)

| Field | Description |
|-------|-------------|
| `m_iSTGrantTimer` | **>0 = timed mission** — server sends `iRemainTime` on start |
| `m_iSTItemID` | int[3] — items granted on start |
| `m_iSTItemNumNeeded` | int[3] |
| `m_iSTItemDropRate` | int[3] |
| `m_iSTGrantWayPoint` | Waypoint NPC number for map marker |
| `m_iSTSpawnMonsterID` | Spawn on start |
| `m_iSTSpwanLocation` | Spawn location |
| `m_iSTMessageType` | Start message flags (bit 2 = email offer) |
| `m_iSTMessageTextID` | Message string ID |
| `m_iSTMessageSendNPC` | Sender NPC |
| `m_iSTDialogBubble` | Bubble text ID |
| `m_iSTDialogBubbleNPCID` | Bubble NPC |
| `m_iSTJournalIDAdd` | Journal summary string via `m_pJournalData` |
| `m_pstrSTScript` | Script string |
| `m_iSTNanoID` | Nano mission nano ID |

---

## Success outgoing (SU*)

| Field | Description |
|-------|-------------|
| `m_iKorSuccRewardID` | Reward table (KR) |
| `m_iSUReward` | Reward table ID |
| `m_iSUOutgoingMission` | Next mission group ID |
| `m_iSUOutgoingTask` | **Next task ID** on success (`ProcessEndSucc` chains here) |
| `m_iSUItem` | int[3] — reward items |
| `m_iSUInstancename` | int[3] — instance name refs |
| `m_iSUMessageType` | Success message type |
| `m_iSUMessagetextID` | Success text |
| `m_iSUMessageSendNPC` | Sender |
| `m_iSUDialogBubble` | Bubble |
| `m_iSUDialogBubbleNPCID` | Bubble NPC |
| `m_iSUJournaliDAdd` | Journal entry |

---

## Fail outgoing (F*)

| Field | Description |
|-------|-------------|
| `m_iFOutgoingMission` | Fail → other mission |
| `m_iFOutgoingTask` | **Restart task on ProcessEndFail err 1/12** |
| `m_iFItemID` | int[3] |
| `m_iFItemNumNeeded` | int[3] |
| `m_iFMessageType` | Fail message |
| `m_iFMessageTextID` | Fail text |
| `m_iFMessageSendNPC` | Sender |
| `m_iFDialogBubble` | Bubble |
| `m_iFDialogBubbleNPCID` | Bubble NPC |
| `m_iFJournalIDAdd` | Journal |

---

## Other

| Field | Description |
|-------|-------------|
| `m_iDelItemID` | int[4] — items removed on complete |
| `m_iMentorEmailID` | int[5] — per-guide email offer IDs |

---

## How to classify any mission task

| Customer symptom | Check these fields |
|------------------|-------------------|
| Timer / “return to NPC” | `m_iSTGrantTimer > 0` and/or `m_iCSUCheckTimer > 0` |
| Fusion Lair / IZ loop | `m_iRequireInstanceID > 0` |
| Kill quest | `m_iHTaskType == 5`, `m_iCSUEnemyID[]` |
| Escort | `m_iHTaskType == 6`, `m_iCSUDEFNPCID` |
| Location | `m_iHTaskType == 2`, `m_iHTerminatorNPCID` |
| Chain | `m_iSUOutgoingTask`, `m_iFOutgoingTask` |

---

## MissionTableScript sub-tables

| Index | Type | Content |
|-------|------|---------|
| 0 | `MissionElement[]` | Task data |
| 1 | `NameStringElement[]` | Names / strings |
| 2 | `RewardElement[]` | Rewards |
| 3 | `JournalElement[]` | Journal summaries |
