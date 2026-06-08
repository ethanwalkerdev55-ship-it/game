using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public enum eGameMode { MainGame = 5 }
public enum eMissionProcess { Start, Success, Fail, Complete }
public enum eJournalMode { eJM_Reward }

public class MissionElement
{
    public int m_iHTaskID;
    public int m_iHMissionID;
    public int m_iRequireInstanceID;
    public int m_iSTGrantTimer;
    public int m_iCSUCheckTimer;
    public int m_iHTerminatorNPCID;
    public int m_iHTaskType;
    public int m_iCSUDEFNPCID;
    public int[] m_iCSUEnemyID = new int[3];
    public int[] m_iCSUItemID = new int[3];
    public int[] m_iCSUItemNumNeeded = new int[3];
    public int m_iSTGrantWayPoint;
    public int m_iSUOutgoingTask;
    public int m_iSUOutgoingMission;
    public int m_iFOutgoingTask;
    public int m_iRepeatflag;
    public int m_iHMissionType;
    public int m_iHMissionName;
    public int m_iSTMessageSendNPC;
    public int m_iSTMessageType;
    public int m_iSTMessageTextID;
    public int m_iSTNanoID;
    public int m_iSUMessageSendNPC;
    public int m_iSUMessageType;
    public int m_iSUMessagetextID;
    public int m_iFMessageSendNPC;
    public int m_iFMessageType;
    public int m_iFMessageTextID;
    public int[] m_iHBarkerTextID = new int[4];
    public int m_iSUReward;
    public int m_iKorSuccRewardID;
}

public class cnMissionNode
{
    public cnMissionNode m_ParentMissionNode;
    public float m_fRemainTime;
    public float m_fLifeTime;
    public int[] m_aiCurrentRemainEnemyNum = new int[3];
    public int[] m_aiCurrentRemainItemNum = new int[3];

    public MissionElement GetMe() => null;
    public int GetMissionState() => 0;
    public void SetRemainTime(float t) { }
    public void SetMissionState(int s) { }
    public bool IsFinalTask() => false;
    public void InitializeBeforeStart() { }
    public cnMissionNode GetParents() => this;
    public cnMissionNode GetChildByIndex(int i) => this;
    public static void UpdateRemainTime() { }
}

public class Status : Component { public int iID; }
public class NpcMoveController : Component { public NpcTable pTable = new NpcTable(); }
public class NpcTable { public int m_iSightRange; public int m_iBarkerType; }

public class cnOwnAvatarStatus : Component
{
    public int iInsMapNum;
    public void UpdateAttrib(object o) { }
}

public class NpcContainer
{
    public Transform SearchTableID(int id) => null;
}

public class NearList
{
    public int iCount;
    public Transform[] SearchedList = Array.Empty<Transform>();
    public void Clear() { }
}

public static class Logger
{
    public static void Log(string message) { }
    public static void Log(object message) { }
}

public class cnEvent
{
    public object this[int index]
    {
        get => null;
        set { }
    }

    public cnEvent() { }
    public cnEvent(int a) { }
    public cnEvent(int a, int b) { }
    public cnEvent(int a, int b, int c) { }

    public static void SendPacket(int size, int opcode, object packet) { }
    public static void SendEvent(cnEvent ev) { }
}

public struct sP_CL2FE_REQ_PC_TASK_END
{
    public int iTaskNum;
    public int iNPC_ID;
    public int iEscortNPC_ID;
}

public struct sP_CL2FE_REQ_NPC_GROUP_INVITE { public int iNPC_ID; }
public struct sP_CL2FE_REQ_NPC_GROUP_KICK { public int iNPC_ID; }
public struct sP_CL2FE_REQ_BARKER { public int iMissionTaskID; public int iNPC_ID; }

public struct sP_FE2CL_REP_PC_TASK_START_SUCC { public int iTaskNum; public int iRemainTime; }
public struct sP_FE2CL_REP_PC_TASK_START_FAIL { public int iTaskNum; }
public struct sP_FE2CL_REP_PC_TASK_END_SUCC { public int iTaskNum; }
public struct sP_FE2CL_REP_PC_TASK_END_FAIL { public int iTaskNum; public int iErrorCode; }

public static class GameFrame
{
    public static cnGameFrame myGameFrame = new cnGameFrame();
    public static bool IsReadyForPlay() => true;
}

public class cnGameFrame
{
    public bool IsGameMode(eGameMode mode) => true;
}

public static class cnFirstUseSysManager
{
    public static void CheckCondition(int id) { }
}

public static class SoundUtil
{
    public static void Playsound(string name) { }
}

public static class WorldDataContainer
{
    public static Vector3 GetClientNpcPos(int id) => Vector3.zero;
}

public static class cnSystemMessageManager
{
    public static void SendSystemMessageBox(object go, int id, string a, string b, object c) { }
}

public static class CoordUtil
{
    public static float Distance_SToC(int range) => 0f;
}

public static class localized
{
    public static int LevelExpansion;
}

public partial class cnMissionManagerV2Donor : MonoBehaviour
{
    private ArrayList m_ActivateMissionList = new ArrayList();
    private ArrayList m_CompletedMissionList = new ArrayList();
    private ArrayList arrRequestStartTask = new ArrayList();
    private NearList m_NearList = new NearList();
    private Transform myuser;
    private cnOwnAvatarStatus ownstatus;
    private NpcContainer pNpcContainer;
    public cnMissionNode SelectMissionTask;

    private bool bStartFlag;
    private float m_fLastRefreshedTime;
    private int m_iMenualSelectedMissionID;
    private int m_iloopTemp;

    private void ReceiveStartGames(object o) { }
    private void CheckNanoFreeTuning(cnEvent ev) { }
    private void UpdateQuestTaskChecker() { }
    private void RefreshQuestSymbol() { }
    private cnMissionNode GetTask(int id) { return null; }
    private void RequestTaskStart(int taskId, int npcId) { }
    private int SearchMissionTaskChecker(int id) { return -1; }
    private void AddMissionTaskChecker(int id) { }
    private void DelMissionTaskChecker(int id) { }
    private bool IsExistTaskInActiveMission(int id) { return false; }
    private void GetNearList(Transform t) { }
    private cnMissionNode GetSelectedActiveMission() { return null; }
    private Transform GetNearbyNPC(int id) { return null; }
    private cnMissionNode GetNextTaskNode(cnMissionNode n) { return null; }
    private int SearchQuestItem(int id) { return 0; }
    private int CheckToCompleteTaskCondition(int id) { return 0; }
    private void SetBubbleChat(eMissionProcess p, cnMissionNode n) { }
    private void SetMissionMessage(int a, int b, int c, int d, int e, int f) { }
    private void InsertActivateTask(int id) { }
    private void StartSelectMissionTask() { }
    private bool IsExistMissionInActieMission(int id) { return false; }
    private void EliminateActiveMission(int id) { }
    private void EliminateCompletedMission(int id) { }
    private void InsertCompletedTask(int id) { }
    private void AddRepeatFlag(int id) { }
    private void ResetSelectMission() { }
    private void KillMissionNPC(int id) { }
}
