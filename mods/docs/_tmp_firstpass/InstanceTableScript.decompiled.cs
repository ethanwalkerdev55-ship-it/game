using System;

[Serializable]
public class InstanceTableScript : BaseTableObject
{
	public enum Type
	{
		Map,
		Warp,
		String,
		Max
	}

	public InstanceElement[] m_pInstanceData;

	public WarpElement[] m_pWarpData;

	public NameStringElement[] m_pWarpNameData;

	public override BaseTableElement GetAt(int iTable, int iIndex)
	{
		switch (iTable)
		{
		case 0:
			return m_pInstanceData[iIndex];
		case 1:
			return m_pWarpData[iIndex];
		case 2:
			return m_pWarpNameData[iIndex];
		default:
			Logger.Log("cannot find table data [InstanceTableScript : " + iTable + "]");
			return null;
		}
	}
}
