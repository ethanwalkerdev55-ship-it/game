// Edit Method on ProcessEndSucc

private void ProcessEndSucc(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_END_SUCC sP_FE2CL_REP_PC_TASK_END_SUCC = (sP_FE2CL_REP_PC_TASK_END_SUCC)myevent[0];
        Logger.Log("ProcessStartFail tasknum : " + sP_FE2CL_REP_PC_TASK_END_SUCC.iTaskNum.ToString());
        if (sP_FE2CL_REP_PC_TASK_END_SUCC.iTaskNum == 616)
        {
            cnFirstUseSysManager.CheckCondition(62);
        }
        if (sP_FE2CL_REP_PC_TASK_END_SUCC.iTaskNum == 461)
        {
            cnFirstUseSysManager.CheckCondition(11);
        }
        this.DelMissionTaskChecker(sP_FE2CL_REP_PC_TASK_END_SUCC.iTaskNum);
        cnMissionNode task = this.GetTask(sP_FE2CL_REP_PC_TASK_END_SUCC.iTaskNum);
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        task.SetMissionState(2);
        this.EliminateActiveMission(task.GetMe().m_iHMissionID);
        this.EliminateCompletedMission(task.GetMe().m_iHMissionID);
        if (task.IsFinalTask() || task.GetMe().m_iSUOutgoingTask == 0)
        {
            this.InsertCompletedTask(sP_FE2CL_REP_PC_TASK_END_SUCC.iTaskNum);
            if (task.GetMe().m_iRepeatflag != 0)
            {
                this.AddRepeatFlag(task.GetMe().m_iRepeatflag);
            }
            this.SetBubbleChat(eMissionProcess.Complete, task);
            SoundUtil.Playsound("Mission_Completed");
            if (task.GetMe().m_iHMissionID == this.m_iMenualSelectedMissionID)
            {
                this.ResetSelectMission();
            }
        }
        else
        {
            SoundUtil.Playsound("Task_Completed");
            this.SetBubbleChat(eMissionProcess.Success, task);
            cnEvent cnEvent = new cnEvent(2, 4, 27);
            cnEvent[0] = 5;
            cnEvent.SendEvent(cnEvent);
        }
        this.SetMissionMessage(task.GetMe().m_iSUMessageSendNPC, task.GetMe().m_iSUMessageType, task.GetMe().m_iHMissionName, task.GetMe().m_iSUMessagetextID, 9, 0);
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
                    Logger.Log("kick npc : " + status.iID.ToString());
                }
            }
        }
        if (this.bForceCompleteChain)
        {
            cnMissionNode nextChainTask = null;
            if (0 < task.GetMe().m_iSUOutgoingTask)
            {
                nextChainTask = this.GetTask(task.GetMe().m_iSUOutgoingTask);
            }
            if (nextChainTask == null)
            {
                nextChainTask = this.GetNextTaskNode(task);
            }
            if (nextChainTask != null && nextChainTask.GetMe() != null)
            {
                Logger.Log("ForceCompleteV2: advance to task " + nextChainTask.GetMe().m_iHTaskID.ToString());
                this.TryForceCompleteChainTask(nextChainTask);
                return;
            }
            Logger.Log("ForceCompleteV2: chain finished");
            this.ClearForceCompleteChain();
            return;
        }
        if (0 < task.GetMe().m_iSUOutgoingTask)
        {
            cnMissionNode task2 = this.GetTask(task.GetMe().m_iSUOutgoingTask);
            if (task2 != null && task2.GetMe() != null)
            {
                this.RequestTaskStart(task2.GetMe().m_iHTaskID, 0);
                return;
            }
        }
        else if (0 < task.GetMe().m_iSUOutgoingMission)
        {
            cnMissionNode task3 = this.GetTask(task.GetMe().m_iSUOutgoingTask);
            if (task3 != null && task3.GetMe() != null)
            {
                Logger.Log("send outgoing mission task");
                this.RequestTaskStart(task3.GetMe().m_iHTaskID, 0);
            }
        }
    }
