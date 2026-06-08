// Edit Method on RequestTaskComplete

	private void RequestTaskComplete(int iTaskID, int iNPCID, bool bError)
	{
		Logger.Log("ForceCompleteV2: RequestTaskComplete enter task " + iTaskID.ToString() + " chain " + this.bForceCompleteChain.ToString());
		if (!this.bForceCompleteChain && (0 >= iTaskID || 0 < this.SearchMissionTaskChecker(iTaskID)))
		{
			return;
		}
		if (this.bForceCompleteChain && 0 < this.SearchMissionTaskChecker(iTaskID))
		{
			Logger.Log("ForceCompleteV2: complete bypass checker task " + iTaskID.ToString());
		}
        cnMissionNode task = this.GetTask(iTaskID);
        if (task == null || task.GetMe() == null)
        {
            if (this.bForceCompleteChain)
            {
                Logger.Log("ForceCompleteV2: complete blocked missing task " + iTaskID.ToString());
            }
            return;
        }
        sP_CL2FE_REQ_PC_TASK_END sP_CL2FE_REQ_PC_TASK_END = default(sP_CL2FE_REQ_PC_TASK_END);
        sP_CL2FE_REQ_PC_TASK_END.iTaskNum = iTaskID;
        if (bError)
        {
            sP_CL2FE_REQ_PC_TASK_END.iEscortNPC_ID = -1;
        }
        if (iNPCID != 0)
        {
            Transform transform = this.pNpcContainer.SearchTableID(iNPCID);
            if (transform == null && !bError)
            {
                if (this.bForceCompleteChain)
                {
                    Logger.Log("ForceCompleteV2: complete blocked npc lookup task " + iTaskID.ToString() + " npc " + iNPCID.ToString());
                }
                return;
            }
            if (transform)
            {
                Status status = transform.GetComponent(typeof(Status)) as Status;
                if (null != status)
                {
                    sP_CL2FE_REQ_PC_TASK_END.iNPC_ID = status.iID;
                }
            }
        }
        if (task.GetMe().m_iHTaskType == 6 && 0 < task.GetMe().m_iCSUDEFNPCID)
        {
            Transform nearbyNPC = this.GetNearbyNPC(task.GetMe().m_iCSUDEFNPCID);
            if (nearbyNPC)
            {
                Status status = nearbyNPC.GetComponent(typeof(Status)) as Status;
                if (null != status)
                {
                    sP_CL2FE_REQ_PC_TASK_END.iEscortNPC_ID = status.iID;
                }
            }
            else if (bError)
            {
                sP_CL2FE_REQ_PC_TASK_END.iEscortNPC_ID = -1;
            }
        }
        Logger.Log("ForceCompleteV2: sent complete packet task " + iTaskID.ToString() + " npc " + iNPCID.ToString() + " err " + bError.ToString());
        cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_PC_TASK_END)), 318767116, sP_CL2FE_REQ_PC_TASK_END);
        this.AddMissionTaskChecker(iTaskID);
    }
