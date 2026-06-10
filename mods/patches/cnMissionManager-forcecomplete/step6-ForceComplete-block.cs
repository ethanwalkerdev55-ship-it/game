// Donor: public ForceCompleteCurrentTask — doc504h (NPC only if SearchTableID; else 0 — avoids RTC silent return)

	public void ForceCompleteCurrentTask()
	{
		Logger.Log("ForceCompleteV2: patch build 2026-06-09-doc504h");
		cnMissionNode target = this.SelectMissionTask;
		if (target == null || target.GetMe() == null)
		{
			target = this.GetSelectedActiveMission();
		}
		if (target == null || target.GetMe() == null)
		{
			if (this.m_ActivateMissionList.Count > 0)
			{
				target = (cnMissionNode)this.m_ActivateMissionList[this.m_ActivateMissionList.Count - 1];
			}
		}
		if (target == null || target.GetMe() == null)
		{
			return;
		}
		MissionElement me = target.GetMe();
		Logger.Log("ForceCompleteV2: hotkey target task " + me.m_iHTaskID.ToString());
		this.m_iloopTemp = 1;
		target.m_aiCurrentRemainEnemyNum[0] = 0;
		target.m_aiCurrentRemainEnemyNum[1] = 0;
		target.m_aiCurrentRemainEnemyNum[2] = 0;
		target.m_aiCurrentRemainItemNum[0] = 0;
		target.m_aiCurrentRemainItemNum[1] = 0;
		target.m_aiCurrentRemainItemNum[2] = 0;
		bool err = me.m_iRequireInstanceID == 12 && (this.ownstatus == null || this.ownstatus.iInsMapNum != 12);
		int npc = 0;
		if (!err && me.m_iHTerminatorNPCID != 0 && this.pNpcContainer.SearchTableID(me.m_iHTerminatorNPCID))
		{
			npc = me.m_iHTerminatorNPCID;
		}
		this.RequestTaskComplete(me.m_iHTaskID, npc, err);
	}
