// ========== STEP 0: FIELDS (add inside class, after NpcTableScript NpcTable) ==========
	private bool bForceCompleteChain;
	private int m_iForceCompleteChainDepth;
	private int m_iForceCompletePendingTaskId;
	private int m_iForceCompleteRetryCount;

// ========== STEP 1: Edit Method → Update ==========
    private void Update()
    {
        if (this.myuser == null || !GameFrame.IsReadyForPlay())
        {
            return;
        }
        if (this.bStartFlag && GameFrame.IsReadyForPlay() && GameFrame.myGameFrame.IsGameMode(eGameMode.MainGame))
        {
            this.ReceiveStartGames(null);
            cnEvent cnEvent = new cnEvent();
            this.CheckNanoFreeTuning(cnEvent);
            cnFirstUseSysManager.CheckCondition(46);
            this.bStartFlag = false;
        }
        this.UpdateQuestTaskChecker();
        if (1f > Time.time - this.m_fLastRefreshedTime)
        {
            return;
        }
        this.m_fLastRefreshedTime = Time.time;
        this.m_NearList.Clear();
        this.GetNearList(this.myuser);
        this.RefreshQuestSymbol();
        if (this.bForceCompleteChain && 0 < this.m_iForceCompletePendingTaskId)
        {
            cnMissionNode pendingTask = this.GetTask(this.m_iForceCompletePendingTaskId);
            if (pendingTask != null && pendingTask.GetMe() != null)
            {
                MissionElement pendingMe = pendingTask.GetMe();
                if (0 < pendingMe.m_iSTGrantTimer && pendingTask.m_fRemainTime <= 0f && this.IsExistTaskInActiveMission(pendingMe.m_iHTaskID) && pendingTask.GetMissionState() == 1)
                {
                    this.m_iForceCompletePendingTaskId = 0;
                    this.RequestForceCompleteTaskEnd(pendingTask);
                }
            }
        }
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            MissionElement me = cnMissionNode.GetMe();
            if (me != null)
            {
                if (0 < me.m_iSTGrantTimer)
                {
                    cnMissionNode.UpdateRemainTime();
                    if (0f >= cnMissionNode.m_fRemainTime)
                    {
                        Logger.Log("Send Time mission.");
                        this.RequestTaskComplete(cnMissionNode.GetMe().m_iHTaskID, 0, false);
                        goto IL_03C3;
                    }
                }
                if (cnMissionNode.GetMe().m_iHTaskType == 6 && 0 < cnMissionNode.GetMe().m_iCSUDEFNPCID)
                {
                    cnEvent cnEvent2 = new cnEvent(13, 17);
                    cnEvent.SendEvent(cnEvent2);
                    if ((int)cnEvent2[1] == 0)
                    {
                        Transform nearbyNPC = this.GetNearbyNPC(cnMissionNode.GetMe().m_iCSUDEFNPCID);
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
                }
                for (int j = 0; j < 3; j++)
                {
                    if (0 < me.m_iCSUItemID[j])
                    {
                        int num = this.SearchQuestItem(me.m_iCSUItemID[j]);
                        cnMissionNode.m_aiCurrentRemainItemNum[j] = me.m_iCSUItemNumNeeded[j] - num;
                    }
                }
                if (0 < me.m_iHTerminatorNPCID && (me.m_iHTaskType == 2 || me.m_iHTaskType == 6))
                {
                    Transform nearbyNPC2 = this.GetNearbyNPC(me.m_iHTerminatorNPCID);
                    if (!(null == nearbyNPC2) && CoordUtil.Distance_SToC((nearbyNPC2.GetComponent(typeof(NpcMoveController)) as NpcMoveController).pTable.m_iSightRange) > (nearbyNPC2.position - this.myuser.position).magnitude)
                    {
                        if (this.CheckToCompleteTaskCondition(me.m_iHTaskID) != 3)
                        {
                            return;
                        }
                        bool flag = false;
                        int num2 = (((int)localized.LevelExpansion != 1) ? me.m_iSUReward : me.m_iKorSuccRewardID);
                        if (0 >= num2)
                        {
                            if (me.m_iHTaskType == 6 && 0 < me.m_iCSUDEFNPCID)
                            {
                                Transform nearbyNPC3 = this.GetNearbyNPC(me.m_iCSUDEFNPCID);
                                if (null != nearbyNPC3)
                                {
                                    flag = true;
                                    if (5f > (nearbyNPC3.position - nearbyNPC2.position).magnitude)
                                    {
                                        this.RequestTaskComplete(me.m_iHTaskID, me.m_iHTerminatorNPCID, false);
                                    }
                                }
                            }
                            if (!flag)
                            {
                                Logger.Log("Location check : " + me.m_iHTaskID.ToString());
                                this.RequestTaskComplete(me.m_iHTaskID, me.m_iHTerminatorNPCID, false);
                            }
                            return;
                        }
                        cnEvent cnEvent3 = new cnEvent(2, 2);
                        cnEvent.SendEvent(cnEvent3);
                        if ((int)cnEvent3[0] != 7)
                        {
                            cnEvent cnEvent4 = new cnEvent(2, 0);
                            cnEvent4[0] = 7;
                            cnEvent.SendEvent(cnEvent4);
                            cnEvent cnEvent5 = new cnEvent(2, 4, 0);
                            cnEvent5[0] = 7;
                            cnEvent5[1] = eJournalMode.eJM_Reward;
                            cnEvent5[2] = me;
                            cnEvent5[3] = null;
                            cnEvent.SendEvent(cnEvent5);
                            break;
                        }
                        break;
                    }
                }
            }
            IL_03C3:;
        }
        this.m_iloopTemp++;
        if (this.m_iloopTemp < 20)
        {
            return;
        }
        this.m_iloopTemp = 0;
        if (0 >= this.m_CompletedMissionList.Count)
        {
            return;
        }
        cnMissionNode cnMissionNode2 = (cnMissionNode)this.m_CompletedMissionList[UnityEngine.Random.Range(0, this.m_CompletedMissionList.Count)];
        for (int k = 0; k < this.m_NearList.iCount; k++)
        {
            Transform transform = this.m_NearList.SearchedList[k];
            bool flag2 = false;
            NpcMoveController npcMoveController = transform.GetComponent(typeof(NpcMoveController)) as NpcMoveController;
            if (npcMoveController.pTable.m_iBarkerType > 0 && npcMoveController.pTable.m_iBarkerType <= 4)
            {
                if (0 < cnMissionNode2.GetMe().m_iHBarkerTextID[npcMoveController.pTable.m_iBarkerType - 1])
                {
                    Status status2 = transform.GetComponent(typeof(Status)) as Status;
                    sP_CL2FE_REQ_BARKER sP_CL2FE_REQ_BARKER = default(sP_CL2FE_REQ_BARKER);
                    sP_CL2FE_REQ_BARKER.iMissionTaskID = cnMissionNode2.GetMe().m_iHTaskID;
                    sP_CL2FE_REQ_BARKER.iNPC_ID = status2.iID;
                    cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_BARKER)), 318767205, sP_CL2FE_REQ_BARKER);
                    return;
                }
            }
            else if (flag2)
            {
                break;
            }
        }
    }

// ========== STEP 2: Edit Method → ProcessStartSucc ==========
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
        if (this.bForceCompleteChain && (this.m_iForceCompletePendingTaskId == iTaskNum || this.m_iForceCompleteChainDepth > 0))
        {
            this.m_iForceCompletePendingTaskId = 0;
            this.PrepareTaskForForceComplete(task);
            if (0 < task.GetMe().m_iSTGrantTimer || 0 < task.GetMe().m_iCSUCheckTimer)
            {
                task.SetRemainTime(0f);
                task.m_fLifeTime = 0f;
            }
            Logger.Log("ForceCompleteV2: complete after start " + iTaskNum.ToString());
            this.m_iForceCompleteRetryCount = 0;
            this.RequestForceCompleteTaskEnd(task);
        }
    }

// ========== STEP 3: Edit Method → ProcessStartFail ==========
    private void ProcessStartFail(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_START_FAIL sP_FE2CL_REP_PC_TASK_START_FAIL = (sP_FE2CL_REP_PC_TASK_START_FAIL)myevent[0];
        Logger.Log("ProcessStartFail tasknumber : " + sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum.ToString());
        this.DelMissionTaskChecker(sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum);
        if (this.bForceCompleteChain && sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum == this.m_iForceCompletePendingTaskId)
        {
            cnMissionNode task = this.GetTask(sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum);
            if (task != null && task.GetMe() != null && this.IsExistTaskInActiveMission(task.GetMe().m_iHTaskID) && task.GetMissionState() == 1)
            {
                this.m_iForceCompletePendingTaskId = 0;
                this.PrepareTaskForForceComplete(task);
                if (0 < task.GetMe().m_iSTGrantTimer || 0 < task.GetMe().m_iCSUCheckTimer)
                {
                    task.SetRemainTime(0f);
                    task.m_fLifeTime = 0f;
                }
                this.RequestForceCompleteTaskEnd(task);
            }
            else
            {
                this.ClearForceCompleteChain();
            }
        }
    }

// ========== STEP 4: Edit Method → ProcessEndSucc ==========
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

// ========== STEP 5: Edit Method → ProcessEndFail ==========
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
            cnSystemMessageManager.SendSystemMessageBox(base.gameObject, 12, "", "", null);
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

// ========== STEP 6: Edit Class partial paste (ForceCompleteCurrentTask through TryForceCompleteChainTask) ==========
// Replaces old ForceCompleteCurrentTask and inserts 7 new helper methods before GetNextTaskNode.
    public void ForceCompleteCurrentTask()
    {
        cnMissionNode forceCompleteTarget = this.GetForceCompleteTarget();
        if (forceCompleteTarget == null || forceCompleteTarget.GetMe() == null)
        {
            Logger.Log("ForceCompleteV2: no active mission task");
            return;
        }
        int taskId = forceCompleteTarget.GetMe().m_iHTaskID;
        Logger.Log("ForceCompleteV2: start chain task " + taskId.ToString());
        this.DelMissionTaskChecker(taskId);
        this.bForceCompleteChain = true;
        this.m_iForceCompleteChainDepth = 0;
        this.m_iForceCompletePendingTaskId = 0;
        this.m_iForceCompleteRetryCount = 0;
        this.TryForceCompleteChainTask(forceCompleteTarget);
    }

    // Token: 0x060010F3 RID: 4339
    private cnMissionNode GetForceCompleteTarget()
    {
        if (this.SelectMissionTask != null && this.SelectMissionTask.GetMe() != null && this.IsExistTaskInActiveMission(this.SelectMissionTask.GetMe().m_iHTaskID))
        {
            return this.SelectMissionTask;
        }
        cnMissionNode selectedActiveMission = this.GetSelectedActiveMission();
        if (selectedActiveMission != null && selectedActiveMission.GetMe() != null)
        {
            return selectedActiveMission;
        }
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode != null && cnMissionNode.GetMe() != null)
            {
                return cnMissionNode;
            }
        }
        return null;
    }

    // Token: 0x060010F4 RID: 4340
    private void PrepareTaskForForceComplete(cnMissionNode task)
    {
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            task.m_aiCurrentRemainEnemyNum[i] = 0;
            task.m_aiCurrentRemainItemNum[i] = 0;
        }
        MissionElement me = task.GetMe();
        if (me.m_iSTGrantTimer > 0 || me.m_iCSUCheckTimer > 0)
        {
            task.m_fRemainTime = 0f;
            task.m_fLifeTime = 0f;
        }
    }

    // Token: 0x060010F5 RID: 4341
    // Token: 0x060010F9 RID: 4345
    private bool NeedsForceCompleteTaskStart(MissionElement me, cnMissionNode task)
    {
        if (me == null || task == null)
        {
            return false;
        }
        if (0 < me.m_iSTGrantTimer || 0 < me.m_iCSUCheckTimer)
        {
            return !this.IsExistTaskInActiveMission(me.m_iHTaskID) || task.GetMissionState() != 1;
        }
        if (0 < me.m_iRequireInstanceID)
        {
            return !this.IsExistTaskInActiveMission(me.m_iHTaskID);
        }
        return false;
    }

    // Token: 0x060010F6 RID: 4342
    private int ResolveForceCompleteTerminatorNpcId(MissionElement me)
    {
        if (me == null || me.m_iHTerminatorNPCID == 0)
        {
            return 0;
        }
        if (this.pNpcContainer.SearchTableID(me.m_iHTerminatorNPCID))
        {
            return me.m_iHTerminatorNPCID;
        }
        if (this.myuser != null)
        {
            this.m_NearList.Clear();
            this.GetNearList(this.myuser);
            if (this.GetNearbyNPC(me.m_iHTerminatorNPCID) != null)
            {
                return me.m_iHTerminatorNPCID;
            }
        }
        return 0;
    }

    // Token: 0x060010F7 RID: 4343
    private void ClearForceCompleteChain()
    {
        this.bForceCompleteChain = false;
        this.m_iForceCompleteChainDepth = 0;
        this.m_iForceCompletePendingTaskId = 0;
        this.m_iForceCompleteRetryCount = 0;
    }

    // Token: 0x060010FA RID: 4346
    private void RequestForceCompleteTaskEnd(cnMissionNode task)
    {
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        MissionElement me = task.GetMe();
        int num = this.m_iForceCompleteRetryCount / 3;
        if (num <= 0)
        {
            this.RequestTaskComplete(me.m_iHTaskID, this.ResolveForceCompleteTerminatorNpcId(me), false);
            return;
        }
        if (num == 1)
        {
            Logger.Log("ForceCompleteV2: retry without npc task " + me.m_iHTaskID.ToString());
            this.RequestTaskComplete(me.m_iHTaskID, 0, false);
            return;
        }
        Logger.Log("ForceCompleteV2: retry with error flag task " + me.m_iHTaskID.ToString());
        this.RequestTaskComplete(me.m_iHTaskID, 0, true);
    }

    // Token: 0x060010F8 RID: 4344
    private void TryForceCompleteChainTask(cnMissionNode task)
    {
        if (task == null || task.GetMe() == null)
        {
            this.ClearForceCompleteChain();
            return;
        }
        if (this.m_iForceCompleteChainDepth >= 16)
        {
            this.ClearForceCompleteChain();
            return;
        }
        MissionElement me = task.GetMe();
        this.PrepareTaskForForceComplete(task);
        if (this.NeedsForceCompleteTaskStart(me, task))
        {
            this.m_iForceCompletePendingTaskId = me.m_iHTaskID;
            this.m_iForceCompleteRetryCount = 0;
            this.RequestTaskStart(me.m_iHTaskID, 0);
            return;
        }
        if (0 < me.m_iSTGrantTimer || 0 < me.m_iCSUCheckTimer)
        {
            task.SetRemainTime(0f);
            task.m_fLifeTime = 0f;
            if (0 < me.m_iSTGrantTimer && this.IsExistTaskInActiveMission(me.m_iHTaskID) && task.GetMissionState() == 1)
            {
                this.m_iForceCompletePendingTaskId = me.m_iHTaskID;
                return;
            }
        }
        this.m_iForceCompleteChainDepth++;
        this.m_iForceCompleteRetryCount = 0;
        Logger.Log("ForceCompleteV2: request end " + me.m_iHTaskID.ToString());
        this.RequestForceCompleteTaskEnd(task);
    }

