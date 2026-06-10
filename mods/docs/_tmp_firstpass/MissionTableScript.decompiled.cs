using System;

[Serializable]
public class MissionTableScript : BaseTableObject
{
	public enum Type
	{
		Data,
		String,
		Reward,
		Journal,
		Max
	}

	public JournalElement[] m_pJournalData;

	public MissionElement[] m_pMissionData;

	public NameStringElement[] m_pMissionStringData;

	public RewardElement[] m_pRewardData;

	public override BaseTableElement GetAt(int iTable, int iIndex)
	{
		switch (iTable)
		{
		case 0:
			return m_pMissionData[iIndex];
		case 1:
			return m_pMissionStringData[iIndex];
		case 2:
			return m_pRewardData[iIndex];
		case 3:
			return m_pJournalData[iIndex];
		default:
			Logger.Log("cannot find table data [MissionTableScript : " + iTable + "]");
			return null;
		}
	}
}
