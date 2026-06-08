// Edit Method on ProcessStartSucc

private void ProcessStartSucc(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_START_SUCC sP_FE2CL_REP_PC_TASK_START_SUCC = (sP_FE2CL_REP_PC_TASK_START_SUCC)myevent[0];
        Logger.Log("ProcessStartSucc : " + sP_FE2CL_REP_PC_TASK_START_SUCC.iTaskNum.ToString());
        int iTaskNum = sP_FE2CL_REP_PC_TASK_START_SUCC.iTaskNum;
        cnFirstUseSysManager.CheckCondition(15);
        if (iTaskNum <= 488)
        {
            if (iTaskNum != 463)
            {
                if (iTaskNum == 488)
                {
                    cnFirstUseSysManager.CheckCondition(64);
                }
            }
            else
            {
                cnFirstUseSysManager.CheckCondition(18);
            }
        }
        else if (iTaskNum != 548)
        {
            if (iTaskNum == 572)
            {
                cnFirstUseSysManager.CheckCondition(17);
            }
        }
        else
        {
            cnFirstUseSysManager.CheckCondition(16);
        }
        this.DelMissionTaskChecker(iTaskNum);
        for (int i = 0; i < this.arrRequestStartTask.Count; i++)
        {
            if ((int)this.arrRequestStartTask[i] == iTaskNum)
            {
                this.arrRequestStartTask.RemoveAt(i);
                break;
            }
        }
        cnMissionNode task = this.GetTask(iTaskNum);
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        bool flag = false;
        task.InitializeBeforeStart();
        if (0 < task.GetMe().m_iSTGrantTimer)
        {
            task.SetRemainTime((float)sP_FE2CL_REP_PC_TASK_START_SUCC.iRemainTime);
        }
        this.SetBubbleChat(eMissionProcess.Start, task);
        this.InsertActivateTask(iTaskNum);
        if (task.GetMe().m_iHMissionType == 2 && task.GetParents().GetChildByIndex(0) == task)
        {
            this.SetMissionMessage(task.GetMe().m_iSTMessageSendNPC, task.GetMe().m_iSTMessageType, task.GetMe().m_iHMissionName, task.GetMe().m_iSTMessageTextID, 10, task.GetMe().m_iSTNanoID);
            this.ownstatus.UpdateAttrib(null);
            flag = true;
        }
        if (!flag)
        {
            this.SetMissionMessage(task.GetMe().m_iSTMessageSendNPC, task.GetMe().m_iSTMessageType, task.GetMe().m_iHMissionName, task.GetMe().m_iSTMessageTextID, 9, 0);
        }
        task.SetMissionState(1);
        if (!this.IsExistMissionInActieMission(this.m_iMenualSelectedMissionID))
        {
            this.m_iMenualSelectedMissionID = task.GetMe().m_iHMissionID;
        }
        this.StartSelectMissionTask();
        Logger.Log("ProcessStartSucc ");
        if ((task.m_ParentMissionNode != null && task.m_ParentMissionNode.GetMe() != null && this.m_iMenualSelectedMissionID == task.m_ParentMissionNode.GetMe().m_iHMissionID) || this.m_iMenualSelectedMissionID == task.GetMe().m_iHMissionID)
        {
            if (0 < task.GetMe().m_iSTGrantWayPoint)
            {
                Vector3 clientNpcPos = WorldDataContainer.GetClientNpcPos(task.GetMe().m_iSTGrantWayPoint);
                if (!clientNpcPos.Equals(Vector3.zero))
                {
                    cnEvent cnEvent = new cnEvent(2, 4, 10);
                    cnEvent[0] = 5;
                    cnEvent[1] = true;
                    cnEvent[2] = clientNpcPos;
                    string text = "TaskNode.GetMe().m_iSTGrantWayPoint ";
                    Vector3 vector = clientNpcPos;
                    Logger.Log(text + vector.ToString());
                    cnEvent.SendEvent(cnEvent);
                }
                else
                {
                    string text2 = "TaskNode.GetMe().m_iSTGrantWayPoint null";
                    Vector3 vector = clientNpcPos;
                    Logger.Log(text2 + vector.ToString());
                    cnEvent cnEvent2 = new cnEvent(2, 4, 10);
                    cnEvent2[0] = 5;
                    cnEvent2[1] = false;
                    cnEvent.SendEvent(cnEvent2);
                }
            }
            else
            {
                Logger.Log("TaskNode.GetMe().m_iSTGrantWayPoint = false ");
                cnEvent cnEvent3 = new cnEvent(2, 4, 10);
                cnEvent3[0] = 5;
                cnEvent3[1] = false;
                cnEvent.SendEvent(cnEvent3);
            }
        }
        for (int j = 0; j < 3; j++)
        {
            if (0 < task.GetMe().m_iCSUItemID[j])
            {
                int num = this.SearchQuestItem(task.GetMe().m_iCSUItemID[j]);
                task.m_aiCurrentRemainItemNum[j] = task.GetMe().m_iCSUItemNumNeeded[j] - num;
            }
        }
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
                    sP_CL2FE_REQ_NPC_GROUP_INVITE sP_CL2FE_REQ_NPC_GROUP_INVITE = default(sP_CL2FE_REQ_NPC_GROUP_INVITE);
                    sP_CL2FE_REQ_NPC_GROUP_INVITE.iNPC_ID = status.iID;
                    cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_NPC_GROUP_INVITE)), 318767236, sP_CL2FE_REQ_NPC_GROUP_INVITE);
                }
            }
        }
        if (this.m_ActivateMissionList.Count == 2)
        {
            cnFirstUseSysManager.CheckCondition(63);
        }
        if (this.bForceCompleteChain && this.m_iForceCompletePendingTaskId == iTaskNum)
        {
            this.PrepareTaskForForceComplete(task);
            MissionElement forceMe = task.GetMe();
            if (0 < forceMe.m_iSTGrantTimer || 0 < forceMe.m_iCSUCheckTimer)
            {
                task.SetRemainTime(0f);
                task.m_fLifeTime = 0f;
                this.m_iForceCompletePendingTaskId = iTaskNum;
                Logger.Log("ForceCompleteV2: wait timer after start " + iTaskNum.ToString());
                return;
            }
            this.m_iForceCompletePendingTaskId = 0;
            Logger.Log("ForceCompleteV2: complete after start " + iTaskNum.ToString());
            this.RequestForceCompleteTaskEnd(task);
        }
    }
