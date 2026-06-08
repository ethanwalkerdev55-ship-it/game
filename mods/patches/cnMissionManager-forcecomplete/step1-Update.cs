// Edit Method on Update: Ctrl+A, paste this entire file, Compile.

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
                if ((0 < pendingMe.m_iSTGrantTimer || 0 < pendingMe.m_iCSUCheckTimer) && pendingTask.m_fRemainTime <= 0f && this.IsExistTaskInActiveMission(pendingMe.m_iHTaskID) && pendingTask.GetMissionState() == 1)
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
