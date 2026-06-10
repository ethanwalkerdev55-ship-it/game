using System;

[Serializable]
public class MissionElement : BaseTableElement
{
	public int m_iHMissionType;

	public int m_iHMissionID;

	public int m_iHMissionName;

	public int m_iHTaskType;

	public int m_iHTaskID;

	public int m_iHNPCID;

	public int m_iHJournalNPCID;

	public int m_iHTerminatorNPCID;

	public int m_iHDifficultyType;

	public int[] m_iHBarkerTextID = new int[4];

	public int m_iHCurrentObjective;

	public int m_iRequireInstanceID;

	public int m_iRepeatflag;

	public int m_iKorStReqLvlMin;

	public int m_iCTRReqLvMin;

	public int m_iCTRReqLvMax;

	public int[] m_iCSTRReqNano = new int[5];

	public int m_iCSTReqGuide;

	public int[] m_iCSTReqMission = new int[2];

	public int m_iCSTEntranceGroupMin;

	public int m_iCSTEntranceGroupMax;

	public int[] m_iCSTItemID = new int[3];

	public int[] m_iCSTItemNumNeeded = new int[3];

	public int m_iCSTTrigger;

	public int m_iCSUCheckTimer;

	public int[] m_iCSUEnemyID = new int[3];

	public int[] m_iCSUNumToKill = new int[3];

	public int[] m_iCSUItemID = new int[3];

	public int[] m_iCSUItemNumNeeded = new int[3];

	public int m_iCSUDEFNPCID;

	public int m_iCSUDEFNPCAI;

	public int m_iCSUDEPNPCFollow;

	public int m_iSTGrantTimer;

	public int[] m_iSTItemID = new int[3];

	public int[] m_iSTItemNumNeeded = new int[3];

	public int[] m_iSTItemDropRate = new int[3];

	public int m_iSTGrantWayPoint;

	public int m_iSTSpawnMonsterID;

	public int m_iSTSpwanLocation;

	public int m_iSTMessageType;

	public int m_iSTMessageTextID;

	public int m_iSTMessageSendNPC;

	public int m_iSTDialogBubble;

	public int m_iSTDialogBubbleNPCID;

	public int m_iSTJournalIDAdd;

	public string m_pstrSTScript;

	public int m_iSTNanoID;

	public int m_iKorSuccRewardID;

	public int m_iSUReward;

	public int m_iSUOutgoingMission;

	public int m_iSUOutgoingTask;

	public int[] m_iSUItem = new int[3];

	public int[] m_iSUInstancename = new int[3];

	public int m_iSUMessageType;

	public int m_iSUMessagetextID;

	public int m_iSUMessageSendNPC;

	public int m_iSUDialogBubble;

	public int m_iSUDialogBubbleNPCID;

	public int m_iSUJournaliDAdd;

	public int m_iFOutgoingMission;

	public int m_iFOutgoingTask;

	public int[] m_iFItemID = new int[3];

	public int[] m_iFItemNumNeeded = new int[3];

	public int m_iFMessageType;

	public int m_iFMessageTextID;

	public int m_iFMessageSendNPC;

	public int m_iFDialogBubble;

	public int m_iFDialogBubbleNPCID;

	public int m_iFJournalIDAdd;

	public int[] m_iDelItemID = new int[4];

	public int[] m_iMentorEmailID = new int[5];
}
