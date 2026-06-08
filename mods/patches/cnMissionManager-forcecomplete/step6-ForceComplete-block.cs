// Donor: public ForceCompleteCurrentTask — same entry path as client's dnSpy patch (hotkey callvirt target).

	public void ForceCompleteCurrentTask()
	{
		cnMissionNode forceCompleteTarget = this.GetForceCompleteTarget();
		if (forceCompleteTarget == null || forceCompleteTarget.GetMe() == null)
		{
			Logger.Log("ForceCompleteV2: no active mission task - select mission in journal first");
			return;
		}
		int taskId = forceCompleteTarget.GetMe().m_iHTaskID;
		Logger.Log("ForceCompleteV2: patch build 2026-06-07-fct-restore");
		Logger.Log("ForceCompleteV2: start chain task " + taskId.ToString());
		this.DelMissionTaskChecker(taskId);
		this.bForceCompleteChain = true;
		this.m_iForceCompleteChainDepth = 0;
		this.m_iForceCompletePendingTaskId = 0;
		this.m_iForceCompleteRetryCount = 0;
		this.TryForceCompleteChainTask(forceCompleteTarget);
	}
