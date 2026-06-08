// Edit Method on ProcessEndFail

private void ProcessEndFail(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_END_FAIL sP_FE2CL_REP_PC_TASK_END_FAIL = (sP_FE2CL_REP_PC_TASK_END_FAIL)myevent[0];
        this.DelMissionTaskChecker(sP_FE2CL_REP_PC_TASK_END_FAIL.iTaskNum);
        Logger.Log("ProcessEndFail : " + sP_FE2CL_REP_PC_TASK_END_FAIL.iTaskNum.ToString() + " Error Code : " + sP_FE2CL_REP_PC_TASK_END_FAIL.iErrorCode.ToString());
        cnMissionNode task = this.GetTask(sP_FE2CL_REP_PC_TASK_END_FAIL.iTaskNum);
        if (task == null || task.GetMe() == null)
        {
            this.ClearForceCompleteChain();
            return;
        }
        if (this.bForceCompleteChain)
        {
            MissionElement me = task.GetMe();
            int forceErrorCode = sP_FE2CL_REP_PC_TASK_END_FAIL.iErrorCode;
            if (forceErrorCode == 13)
            {
                Logger.Log("ForceCompleteV2: abort fatal error 13 on task " + me.m_iHTaskID.ToString());
                this.ClearForceCompleteChain();
                return;
            }
            if (forceErrorCode == 1 || forceErrorCode == 11 || forceErrorCode == 12)
            {
                this.m_iForceCompleteRetryCount++;
                if (this.m_iForceCompleteRetryCount > 9)
                {
                    Logger.Log("ForceCompleteV2: max retries exceeded on task " + me.m_iHTaskID.ToString());
                    this.ClearForceCompleteChain();
                    return;
                }
                Logger.Log("ForceCompleteV2: retry task " + me.m_iHTaskID.ToString() + " err " + forceErrorCode.ToString() + " attempt " + this.m_iForceCompleteRetryCount.ToString());
                this.m_iForceCompletePendingTaskId = me.m_iHTaskID;
                this.PrepareTaskForForceComplete(task);
                if (0 < me.m_iSTGrantTimer || 0 < me.m_iCSUCheckTimer)
                {
                    task.SetRemainTime(0f);
                    task.m_fLifeTime = 0f;
                    if (this.IsExistTaskInActiveMission(me.m_iHTaskID) && task.GetMissionState() == 1)
                    {
                        this.m_iForceCompletePendingTaskId = 0;
                        this.RequestForceCompleteTaskEnd(task);
                        return;
                    }
                    this.m_iForceCompletePendingTaskId = me.m_iHTaskID;
                    return;
                }
                if (!this.IsExistTaskInActiveMission(me.m_iHTaskID) || task.GetMissionState() != 1)
                {
                    this.RequestTaskStart(me.m_iHTaskID, 0);
                }
                else
                {
                    this.RequestForceCompleteTaskEnd(task);
                }
                return;
            }
            Logger.Log("ForceCompleteV2: abort task " + me.m_iHTaskID.ToString() + " err " + forceErrorCode.ToString());
            this.ClearForceCompleteChain();
            return;
        }
        int iErrorCode = sP_FE2CL_REP_PC_TASK_END_FAIL.iErrorCode;
        if (iErrorCode != 1 && iErrorCode - 11 > 1)
        {
            if (iErrorCode != 13)
            {
                return;
            }
            cnSystemMessageManager.SendSystemMessageBox(null, 12, "", "", null);
        }
        else
        {
            if (task.GetMe().m_iHTaskType == 6 && 0 < task.GetMe().m_iCSUDEFNPCID)
            {
                this.m_NearList.Clear();
                this.GetNearList(this.myuser);
                Transform nearbyNPC = this.GetNearbyNPC(task.GetMe().m_iCSUDEFNPCID);
                if (nearbyNPC != null)
                {
                    Status status = nearbyNPC.GetComponent(typeof(Status)) as Status;
                    if (null != status)
                    {
                        sP_CL2FE_REQ_NPC_GROUP_KICK sP_CL2FE_REQ_NPC_GROUP_KICK = default(sP_CL2FE_REQ_NPC_GROUP_KICK);
                        sP_CL2FE_REQ_NPC_GROUP_KICK.iNPC_ID = status.iID;
                        cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_NPC_GROUP_KICK)), 318767237, sP_CL2FE_REQ_NPC_GROUP_KICK);
                    }
                }
            }
            task.SetMissionState(0);
            this.EliminateActiveMission(task.GetMe().m_iHMissionID);
            this.SetBubbleChat(eMissionProcess.Fail, task);
            this.SetMissionMessage(task.GetMe().m_iFMessageSendNPC, task.GetMe().m_iFMessageType, task.GetMe().m_iHMissionName, task.GetMe().m_iFMessageTextID, 9, 0);
            Logger.Log("Fail Outgoing Task : " + task.GetMe().m_iFOutgoingTask.ToString());
            if (0 < task.GetMe().m_iFOutgoingTask)
            {
                cnMissionNode task2 = this.GetTask(task.GetMe().m_iFOutgoingTask);
                if (task2 != null && task2.GetMe() != null)
                {
                    this.RequestTaskStart(task2.GetMe().m_iHTaskID, 0);
                    return;
                }
            }
            else if (task.IsFinalTask() && this.m_iMenualSelectedMissionID == task.GetMe().m_iHMissionID)
            {
                this.ResetSelectMission();
                return;
            }
        }
    }
