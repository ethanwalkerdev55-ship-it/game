# Task types — completion mechanics

From `eTaskTypeProperty` (`mods/decompiled/Assembly-CSharp/eTaskTypeProperty.cs`).

---

## 1 — Talk

| Aspect | Detail |
|--------|--------|
| Complete | Usually at `m_iHTerminatorNPCID` via NPC interaction |
| Packet | `TASK_END` with terminator runtime `iNPC_ID` |
| Auto | Rare — player must talk to NPC |

---

## 2 — GotoLocation

| Aspect | Detail |
|--------|--------|
| Complete | `Update` loop: player within sight range of terminator NPC |
| Check | `CheckToCompleteTaskCondition == 3` |
| Log | `Location check : {taskId}` |
| Packet | `RequestTaskComplete(taskId, m_iHTerminatorNPCID, false)` |
| Reward | Opens journal reward UI if `m_iSUReward > 0` |

---

## 3 — UseItems

| Aspect | Detail |
|--------|--------|
| Progress | `m_aiCurrentRemainItemNum[j]` from inventory counts |
| Complete | When all `m_iCSUItemID` quotas met |
| Packet | `TASK_END` |

---

## 4 — Delivery

| Aspect | Detail |
|--------|--------|
| Similar | Item + NPC delivery combination |
| Complete | Item quotas + terminator proximity |

---

## 5 — Defeat

| Aspect | Detail |
|--------|--------|
| Progress | `KillMissionNPC` decrements `m_aiCurrentRemainEnemyNum` |
| Complete | All `m_iCSUEnemyID` quotas zero |
| Auto | If no terminator NPC and kills met → direct `TASK_END` or reward UI |
| Packet | `TASK_END` with `iNPC_ID=0` common |

---

## 6 — EscortDefence

| Aspect | Detail |
|--------|--------|
| Escort NPC | `m_iCSUDEFNPCID` |
| Start | `NPC_GROUP_INVITE` on `ProcessStartSucc` |
| Update | Re-invite if not in group |
| Complete | Escort within 5m of terminator (type 2/6 path) |
| **Death** | `CheckEscortQuest` → `RequestTaskComplete(taskId, deadNpcId, **true**)` |
| End | `NPC_GROUP_KICK` on success/fail |

**`bError=true`** is standard for escort failure — same flag used for instance warp abort.

---

## Completion packet summary by type

| Type | iNPC_ID | iEscortNPC_ID | bError common? |
|------|---------|---------------|----------------|
| Talk | Terminator runtime | 0 | false |
| GotoLocation | Terminator runtime | 0 | false |
| UseItems | 0 or terminator | 0 | false |
| Delivery | Terminator | 0 | false |
| Defeat | 0 | 0 | false |
| EscortDefence | Terminator | Escort runtime | true on escort death |
| Instance (any type) | varies | -1 on warp abort | true on warp |
