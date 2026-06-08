using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class cnMissionManager : MonoBehaviour
{
    // Token: 0x0600053C RID: 1340 RVA: 0x0006204C File Offset: 0x0006024C
    private void Awake()
    {
        base.transform.name = cnMissionManager.OwnName;
        this.arrayCallFunc[0] = new cnMissionManager.ManagerFunc(this.ReceiveGetActiveMission);
        this.arrayCallFunc[1] = new cnMissionManager.ManagerFunc(this.ReceiveGetCompletedMission);
        this.arrayCallFunc[2] = new cnMissionManager.ManagerFunc(this.ReceiveCheckMissionKilledCount);
        this.arrayCallFunc[3] = new cnMissionManager.ManagerFunc(this.ReceiveCheckMissionItemCount);
        this.arrayCallFunc[4] = new cnMissionManager.ManagerFunc(this.ReceiveKillMissionNPC);
        this.arrayCallFunc[5] = new cnMissionManager.ManagerFunc(this.ReceiveRequestTaskStart);
        this.arrayCallFunc[6] = new cnMissionManager.ManagerFunc(this.ReceiveRequestTaskEnd);
        this.arrayCallFunc[7] = new cnMissionManager.ManagerFunc(this.ReceiveClearMissionSystemManagerData);
        this.arrayCallFunc[8] = new cnMissionManager.ManagerFunc(this.ReceiveCheckToActiveTask);
        this.arrayCallFunc[9] = new cnMissionManager.ManagerFunc(this.ReceiveGetToChargeChangingGuide);
        this.arrayCallFunc[10] = new cnMissionManager.ManagerFunc(this.ReceiveGetNpcMission);
        this.arrayCallFunc[11] = new cnMissionManager.ManagerFunc(this.SetAvatar);
        this.arrayCallFunc[12] = new cnMissionManager.ManagerFunc(this.ReceiveStartCondition);
        this.arrayCallFunc[13] = new cnMissionManager.ManagerFunc(this.ReceiveCompleteCondition);
        this.arrayCallFunc[14] = new cnMissionManager.ManagerFunc(this.ReceiveMissionCondition);
        this.arrayCallFunc[15] = new cnMissionManager.ManagerFunc(this.ReceiveGetTask);
        this.arrayCallFunc[16] = new cnMissionManager.ManagerFunc(this.ReceiveGetMission);
        this.arrayCallFunc[17] = new cnMissionManager.ManagerFunc(this.ReceiveGetSelectMission);
        this.arrayCallFunc[18] = new cnMissionManager.ManagerFunc(this.ReceiveSetSelectMission);
        this.arrayCallFunc[21] = new cnMissionManager.ManagerFunc(this.ProcessStartSucc);
        this.arrayCallFunc[22] = new cnMissionManager.ManagerFunc(this.ProcessStartFail);
        this.arrayCallFunc[23] = new cnMissionManager.ManagerFunc(this.ProcessEndSucc);
        this.arrayCallFunc[24] = new cnMissionManager.ManagerFunc(this.ProcessEndFail);
        this.arrayCallFunc[25] = new cnMissionManager.ManagerFunc(this.SetMissionAndTaskFlags);
        this.arrayCallFunc[26] = new cnMissionManager.ManagerFunc(this.DelActiveMissionSucc);
        this.arrayCallFunc[27] = new cnMissionManager.ManagerFunc(this.DelActiveMissionFail);
        this.arrayCallFunc[19] = new cnMissionManager.ManagerFunc(this.ReceiveStartGames);
        this.arrayCallFunc[20] = new cnMissionManager.ManagerFunc(this.CheckEscortQuest);
        this.arrayCallFunc[28] = new cnMissionManager.ManagerFunc(this.CheckNanoFreeTuning);
        this.arrayCallFunc[29] = new cnMissionManager.ManagerFunc(this.CheckWarpAllMision);
        this.arrayCallFunc[30] = new cnMissionManager.ManagerFunc(this.ChangeGuide);
        this.arrayCallFunc[31] = new cnMissionManager.ManagerFunc(this.ReceiveGetMissionNano);
        this.arrayCallFunc[32] = new cnMissionManager.ManagerFunc(this.ReceiveMissionClear);
        this.arrayCallFunc[33] = new cnMissionManager.ManagerFunc(this.ReceiveIsRepeatCompleated);
    }

    // Token: 0x0600053D RID: 1341 RVA: 0x0006232C File Offset: 0x0006052C
    private void SetAvatar(cnEvent myevent)
    {
        this.myuser = (Transform)myevent[0];
        this.ownstatus = this.myuser.GetComponent(typeof(cnOwnAvatarStatus)) as cnOwnAvatarStatus;
        this.status = this.myuser.GetComponent(typeof(Status)) as Status;
        this.avstatus = this.myuser.GetComponent(typeof(cnAvatarStatus)) as cnAvatarStatus;
        this.ReceiveStartGames(null);
    }

    // Token: 0x0600053E RID: 1342 RVA: 0x000049FF File Offset: 0x00002BFF
    private void LoadAssets()
    {
        this.Init();
    }

    // Token: 0x0600053F RID: 1343 RVA: 0x000623B4 File Offset: 0x000605B4
    private void Start()
    {
        cnMissionManager.instance = this;
        GameObject manager = GlobalManager.GetManager(4);
        this.pNpcContainer = manager.GetComponent(typeof(NpcContainer)) as NpcContainer;
    }

    // Token: 0x06000540 RID: 1344 RVA: 0x000623EC File Offset: 0x000605EC
    private bool LoadNPCTable()
    {
        bool flag;
        try
        {
            this.NpcTable = (NpcTableScript)TableContainer.GetTable(10);
            flag = true;
        }
        catch
        {
            flag = false;
        }
        return flag;
    }

    // Token: 0x06000541 RID: 1345 RVA: 0x00062428 File Offset: 0x00060628
    private void ChangeGuide(cnEvent myevnet)
    {
        int count = this.m_ActivateMissionList.Count;
        for (int i = 0; i < count; i++)
        {
            MissionElement me = ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe();
            if (me != null && me.m_iHMissionType == 1 && me.m_iCSTReqGuide != this.ownstatus.iGuide)
            {
                sP_CL2FE_REQ_PC_TASK_STOP sP_CL2FE_REQ_PC_TASK_STOP = default(sP_CL2FE_REQ_PC_TASK_STOP);
                sP_CL2FE_REQ_PC_TASK_STOP.iTaskNum = me.m_iHTaskID;
                cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_PC_TASK_STOP)), 318767122, sP_CL2FE_REQ_PC_TASK_STOP);
            }
        }
        this.ReceiveStartGames(null);
    }

    // Token: 0x06000542 RID: 1346 RVA: 0x000624C0 File Offset: 0x000606C0
    private void RefreshQuestSymbol()
    {
        for (int i = 0; i < this.m_NearList.iCount; i++)
        {
            NpcMoveController npcMoveController = this.m_NearList.SearchedList[i].GetComponent(typeof(NpcMoveController)) as NpcMoveController;
            if (npcMoveController.pTable.m_iNpcType != 0 && npcMoveController.pTable.m_iNpcType != 25)
            {
                bool flag = false;
                for (int j = 0; j < this.m_ActivateMissionList.Count; j++)
                {
                    cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[j];
                    if (cnMissionNode.GetMe().m_iSTGrantWayPoint == npcMoveController.pTable.m_iNpcNumber && cnMissionNode.GetMe().m_iHMissionID == this.m_iMenualSelectedMissionID)
                    {
                        flag = true;
                        break;
                    }
                }
                npcMoveController.SetSmartIndicator(flag);
                if (this.NPCTypeHasMissionAdvanceAvailable(npcMoveController.pTable.m_iNpcNumber))
                {
                    npcMoveController.SetQuestSymbol(669);
                }
                else if (this.NPCTypeHasMissionAvailable(npcMoveController.pTable.m_iNpcNumber))
                {
                    npcMoveController.SetQuestSymbol(686);
                }
                else
                {
                    npcMoveController.SetQuestSymbol(0);
                }
            }
        }
    }

    // Token: 0x06000543 RID: 1347 RVA: 0x00004A07 File Offset: 0x00002C07
    private void ReceiveIsRepeatCompleated(cnEvent myevent)
    {
        myevent[0] = this.IsRepeatCompleated((int)myevent[0]);
    }

    // Token: 0x06000544 RID: 1348 RVA: 0x000625D8 File Offset: 0x000607D8
    private bool IsRepeatCompleated(int repeatFlag)
    {
        int num = repeatFlag / 8;
        int num2 = repeatFlag % 8;
        if (num2 == 0)
        {
            num--;
            num2 = 8;
        }
        return num > -1 && ((this.m_iRepeatQuestFlag[num] >> num2 - 1) & 1L) != 0L;
    }

    // Token: 0x06000545 RID: 1349 RVA: 0x00062614 File Offset: 0x00060814
    private void AddRepeatFlag(int repeatFlag)
    {
        int num = repeatFlag / 8;
        int num2 = repeatFlag % 8;
        if (num2 == 0)
        {
            num--;
            num2 = 8;
        }
        if (num > -1)
        {
            this.m_iRepeatQuestFlag[num] |= 1L << ((num2 - 1) & 31);
        }
    }

    // Token: 0x06000546 RID: 1350 RVA: 0x00062650 File Offset: 0x00060850
    private void CheckEscortQuest(cnEvent myevent)
    {
        int num = (int)myevent[0];
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            MissionElement me = ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe();
            if (me != null && me.m_iHTaskType == 6 && me.m_iCSUDEFNPCID == num)
            {
                Logger.Log("Send Escort Error");
                this.RequestTaskComplete(me.m_iHTaskID, num, true);
                return;
            }
        }
    }

    // Token: 0x06000547 RID: 1351 RVA: 0x000626C8 File Offset: 0x000608C8
    private void CheckWarpAllMision(cnEvent myevent)
    {
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            MissionElement me = ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe();
            if (me != null && me.m_iRequireInstanceID > 0)
            {
                this.RequestTaskComplete(me.m_iHTaskID, 0, true);
            }
        }
    }

    // Token: 0x06000548 RID: 1352 RVA: 0x0006271C File Offset: 0x0006091C
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

    // Token: 0x06000549 RID: 1353 RVA: 0x00004A27 File Offset: 0x00002C27
    public void ReceiveEvent(cnEvent myevent)
    {
        this.arrayCallFunc[myevent.FuncNumber](myevent);
    }

    // Token: 0x0600054A RID: 1354 RVA: 0x00062C48 File Offset: 0x00060E48
    private void Init()
    {
        this.m_MissionTaskCheckerList.Clear();
        this.m_ActivateMissionList.Clear();
        this.m_CompletedMissionList.Clear();
        this.m_iMenualSelectedMissionID = 0;
        this.m_fLastRefreshedTime = 0f;
        this.m_iloopTemp = 0;
        this.MakeMissionGroup();
        this.TaskBindWithNPC();
    }

    // Token: 0x0600054B RID: 1355 RVA: 0x00062C9C File Offset: 0x00060E9C
    private void ResetSelectMission()
    {
        this.m_iMenualSelectedMissionID = 0;
        this.SelectMissionTask = null;
        if (this.m_ActivateMissionList.Count > 0)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[this.m_ActivateMissionList.Count - 1];
            this.m_iMenualSelectedMissionID = cnMissionNode.GetMe().m_iHMissionID;
            this.StartSelectMissionTask();
            return;
        }
        cnEvent cnEvent = new cnEvent(2, 4, 10);
        cnEvent[0] = 5;
        cnEvent[1] = false;
        cnEvent.SendEvent(cnEvent);
    }

    // Token: 0x0600054C RID: 1356 RVA: 0x00062D24 File Offset: 0x00060F24
    private void MakeMissionGroup()
    {
        this.m_MissionGroupList.Clear();
        this.missionscript = (MissionTableScript)TableContainer.GetTable(7);
        int length = this.missionscript.m_pMissionData.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            int count = this.m_MissionGroupList.Count;
            bool flag = false;
            for (int j = 0; j < count; j++)
            {
                if (this.missionscript.m_pMissionData[i].m_iHMissionID == ((cnMissionNode)this.m_MissionGroupList[j]).GetMissionID())
                {
                    ((cnMissionNode)this.m_MissionGroupList[j]).AddChild(this.missionscript.m_pMissionData[i]);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                cnMissionNode cnMissionNode = new cnMissionNode();
                cnMissionNode.Init();
                cnMissionNode.AddChild(this.missionscript.m_pMissionData[i]);
                this.m_MissionGroupList.Add(cnMissionNode);
            }
        }
    }

    // Token: 0x0600054D RID: 1357 RVA: 0x00062E18 File Offset: 0x00061018
    private void TaskBindWithNPC()
    {
        this.m_NPCAndMissionRelationList.Clear();
        MissionTableScript missionTableScript = (MissionTableScript)TableContainer.GetTable(7);
        NpcTableScript npcTableScript = (NpcTableScript)TableContainer.GetTable(10);
        int num = npcTableScript.m_pNpcData.Length;
        int num2 = missionTableScript.m_pMissionData.Length;
        for (int i = 0; i < npcTableScript.m_pNpcData.Length; i++)
        {
            cnNPCAndMissionRelationNode cnNPCAndMissionRelationNode = new cnNPCAndMissionRelationNode();
            cnNPCAndMissionRelationNode.Init();
            cnNPCAndMissionRelationNode.SetNPCID(npcTableScript.m_pNpcData[i].m_iNpcNumber);
            if (npcTableScript.m_pNpcData[i].m_iNpcNumber > 0)
            {
                for (int j = 1; j < num2; j++)
                {
                    if (npcTableScript.m_pNpcData[i].m_iNpcNumber == missionTableScript.m_pMissionData[j].m_iHNPCID)
                    {
                        cnNPCAndMissionRelationNode.AddGrantTaskID(missionTableScript.m_pMissionData[j]);
                    }
                    if (npcTableScript.m_pNpcData[i].m_iNpcNumber == missionTableScript.m_pMissionData[j].m_iHTerminatorNPCID)
                    {
                        cnNPCAndMissionRelationNode.AddTerminateTaskID(missionTableScript.m_pMissionData[j]);
                    }
                }
            }
            this.m_NPCAndMissionRelationList.Add(cnNPCAndMissionRelationNode);
        }
    }

    // Token: 0x0600054E RID: 1358 RVA: 0x00062F20 File Offset: 0x00061120
    private int CheckToStartTaskCondition(int iTaskID)
    {
        if (0 >= iTaskID)
        {
            return -1;
        }
        cnMissionNode task = this.GetTask(iTaskID);
        if (task == null)
        {
            return -1;
        }
        if (task.GetMe() == null)
        {
            return -1;
        }
        foreach (object obj in ((ArrayList)this.arrRequestStartTask))
        {
            int num = (int)obj;
            if (this.GetTask(num).GetMe().m_iHMissionID == task.GetMe().m_iHMissionID)
            {
                return 1;
            }
        }
        if (task.GetMe().m_iRepeatflag == 0)
        {
            if (this.IsExistMissionInCompletedMission(task.GetMe().m_iHMissionID))
            {
                return 5;
            }
        }
        else if (this.IsRepeatCompleated(task.GetMe().m_iRepeatflag))
        {
            return 5;
        }
        if (this.IsExistMissionInActieMission(task.GetMe().m_iHMissionID))
        {
            return 2;
        }
        for (int i = 0; i < 2; i++)
        {
            if (0 < task.GetMe().m_iCSTReqMission[i] && !this.IsExistMissionInCompletedMission(task.GetMe().m_iCSTReqMission[i]))
            {
                return 1;
            }
        }
        if ((int)localized.LevelExpansion == 1)
        {
            if (0 < task.GetMe().m_iKorStReqLvlMin && this.status.iLevel < task.GetMe().m_iKorStReqLvlMin)
            {
                return 1;
            }
        }
        else
        {
            if (0 < task.GetMe().m_iCTRReqLvMin && this.status.iLevel < task.GetMe().m_iCTRReqLvMin)
            {
                return 1;
            }
            if (0 < task.GetMe().m_iCTRReqLvMax && this.status.iLevel < task.GetMe().m_iCTRReqLvMax)
            {
                return 1;
            }
        }
        for (int j = 0; j < 5; j++)
        {
            if (0 < task.GetMe().m_iCSTRReqNano[j])
            {
                int num2 = 0;
                while ((long)num2 < (long)((ulong)csDefines.SIZEOF_NANO_BANK_SLOT) && !task.GetMe().m_iCSTRReqNano[j].Equals((int)this.ownstatus.aNanoBank[num2].iID))
                {
                    num2++;
                }
                if ((long)num2 >= (long)((ulong)csDefines.SIZEOF_NANO_BANK_SLOT))
                {
                    return 1;
                }
            }
        }
        if (0 < task.GetMe().m_iCSTReqGuide && !this.ownstatus.iGuide.Equals(task.GetMe().m_iCSTReqGuide))
        {
            return 1;
        }
        for (int k = 0; k < 2; k++)
        {
            if (0 < task.GetMe().m_iCSTReqMission[k] && !this.IsCompletedMission(task.GetMe().m_iCSTReqMission[k]))
            {
                return 1;
            }
        }
        for (int l = 0; l < 3; l++)
        {
            if (0 < task.GetMe().m_iCSTItemID[l] && this.SearchQuestItem(task.GetMe().m_iCSTItemID[l]) < task.GetMe().m_iCSTItemNumNeeded[l])
            {
                return 1;
            }
        }
        if (0 < task.GetMe().m_iCSTTrigger && !this.IsExistTaskInActiveMission(task.GetMe().m_iCSTTrigger))
        {
            return 1;
        }
        return 0;
    }

    // Token: 0x0600054F RID: 1359 RVA: 0x00063208 File Offset: 0x00061408
    private int CheckToCompleteTaskCondition(int iTaskID)
    {
        if (0 >= iTaskID)
        {
            return -1;
        }
        cnMissionNode task = this.GetTask(iTaskID);
        if (task == null)
        {
            return -1;
        }
        if (task.GetMe() == null)
        {
            return -1;
        }
        if (!this.IsExistTaskInActiveMission(task.GetMe().m_iHTaskID))
        {
            return 4;
        }
        Logger.Log("CheckToCompleteTaskCondition " + iTaskID.ToString());
        if (0 < task.GetMe().m_iCSUCheckTimer && 0f >= task.m_fRemainTime)
        {
            return 6;
        }
        for (int i = 0; i < 3; i++)
        {
            if (0 < task.GetMe().m_iCSUEnemyID[i] && 0 < task.m_aiCurrentRemainEnemyNum[i])
            {
                return 4;
            }
        }
        for (int j = 0; j < 3; j++)
        {
            if (0 < task.GetMe().m_iCSUItemID[j] && 0 < task.m_aiCurrentRemainItemNum[j])
            {
                return 4;
            }
        }
        return 3;
    }

    // Token: 0x06000550 RID: 1360 RVA: 0x000632D0 File Offset: 0x000614D0
    private void KillMissionNPC(int iNPCID)
    {
        bool[] array = new bool[3];
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            if (this.m_ActivateMissionList[i] != null && ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe() != null)
            {
                for (int j = 0; j < 3; j++)
                {
                    array[j] = false;
                    if (iNPCID.Equals(((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iCSUEnemyID[j]))
                    {
                        ((cnMissionNode)this.m_ActivateMissionList[i]).m_aiCurrentRemainEnemyNum[j]--;
                        if (0 >= ((cnMissionNode)this.m_ActivateMissionList[i]).m_aiCurrentRemainEnemyNum[j])
                        {
                            array[j] = true;
                        }
                    }
                    if (0 >= ((cnMissionNode)this.m_ActivateMissionList[i]).m_aiCurrentRemainEnemyNum[j])
                    {
                        array[j] = true;
                    }
                }
                bool flag = true;
                for (int k = 0; k < 3; k++)
                {
                    if (false.Equals(array[k]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (true.Equals(flag))
                {
                    int l = 0;
                    int num = 0;
                    while (l < 3)
                    {
                        num += ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iCSUNumToKill[l];
                        l++;
                    }
                    if (0 >= ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iHTerminatorNPCID && num > 0)
                    {
                        int num2 = (((int)localized.LevelExpansion != 1) ? ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iSUReward : ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iKorSuccRewardID);
                        if (0 < num2)
                        {
                            if (this.CheckToCompleteTaskCondition(((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iHTaskID) == 3)
                            {
                                cnEvent cnEvent = new cnEvent(2, 2);
                                cnEvent.SendEvent(cnEvent);
                                if ((int)cnEvent[0] != 7)
                                {
                                    cnEvent cnEvent2 = new cnEvent(2, 0);
                                    cnEvent2[0] = 7;
                                    cnEvent.SendEvent(cnEvent2);
                                    cnEvent cnEvent3 = new cnEvent(2, 4, 0);
                                    cnEvent3[0] = 7;
                                    cnEvent3[1] = eJournalMode.eJM_Reward;
                                    cnEvent3[2] = ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe();
                                    cnEvent3[3] = null;
                                    cnEvent.SendEvent(cnEvent3);
                                    return;
                                }
                                break;
                            }
                        }
                        else
                        {
                            this.RequestTaskComplete(((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iHTaskID, 0, false);
                        }
                    }
                }
            }
        }
    }

    // Token: 0x06000551 RID: 1361 RVA: 0x00063580 File Offset: 0x00061780
    private void RequestTaskComplete(int iTaskID, int iNPCID, bool bError)
    {
        if (0 >= iTaskID || 0 < this.SearchMissionTaskChecker(iTaskID))
        {
            return;
        }
        cnMissionNode task = this.GetTask(iTaskID);
        if (task == null || task.GetMe() == null)
        {
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
                Status status2 = nearbyNPC.GetComponent(typeof(Status)) as Status;
                if (null != status2)
                {
                    sP_CL2FE_REQ_PC_TASK_END.iEscortNPC_ID = status2.iID;
                }
            }
            else if (bError)
            {
                sP_CL2FE_REQ_PC_TASK_END.iEscortNPC_ID = -1;
            }
        }
        cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_PC_TASK_END)), 318767116, sP_CL2FE_REQ_PC_TASK_END);
        this.AddMissionTaskChecker(iTaskID);
    }

    // Token: 0x06000552 RID: 1362 RVA: 0x000636B8 File Offset: 0x000618B8
    private void AssignedQuestSlotNum(ref int iGuide, ref int iNano, ref int iNormal)
    {
        iNormal = 0;
        iNano = 0;
        iGuide = 0;
        int count = this.m_ActivateMissionList.Count;
        while (0 < count--)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[count];
            if (cnMissionNode != null && cnMissionNode.GetMe() != null)
            {
                switch (cnMissionNode.GetMe().m_iHMissionType)
                {
                case 1:
                    iGuide++;
                    break;
                case 2:
                    iNano++;
                    break;
                case 3:
                    iNormal++;
                    break;
                }
            }
        }
    }

    // Token: 0x06000553 RID: 1363 RVA: 0x0000229F File Offset: 0x0000049F
    private void CheckMissionKilledCount(int iNPCID)
    {
    }

    // Token: 0x06000554 RID: 1364 RVA: 0x0006373C File Offset: 0x0006193C
    private void CheckMissionItemCount(int iItemID)
    {
        bool[] array = new bool[3];
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode.GetMe() != null)
            {
                for (int j = 0; j < 3; j++)
                {
                    array[j] = false;
                    if (0 >= cnMissionNode.GetMe().m_iCSUItemID[j])
                    {
                        array[j] = true;
                    }
                    else
                    {
                        int num = this.SearchQuestItem(cnMissionNode.GetMe().m_iCSUItemID[j]);
                        cnMissionNode.m_aiCurrentRemainItemNum[j] = cnMissionNode.GetMe().m_iCSUItemNumNeeded[j] - num;
                        if (0 >= cnMissionNode.m_aiCurrentRemainItemNum[j])
                        {
                            array[j] = true;
                        }
                    }
                }
                bool flag = true;
                for (int k = 0; k < 3; k++)
                {
                    if (!array[k])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    int l = 0;
                    int num2 = 0;
                    while (l < 3)
                    {
                        num2 += cnMissionNode.GetMe().m_iCSUItemNumNeeded[l];
                        l++;
                    }
                    if (0 >= cnMissionNode.GetMe().m_iHTerminatorNPCID && num2 > 0)
                    {
                        int num3 = (((int)localized.LevelExpansion != 1) ? cnMissionNode.GetMe().m_iSUReward : cnMissionNode.GetMe().m_iKorSuccRewardID);
                        if (0 >= num3)
                        {
                            this.RequestTaskComplete(cnMissionNode.GetMe().m_iHTaskID, 0, false);
                            Logger.Log("Complete Check Mission Item : " + iItemID.ToString());
                            return;
                        }
                        if (this.CheckToCompleteTaskCondition(cnMissionNode.GetMe().m_iHTaskID) != 3)
                        {
                            break;
                        }
                        cnEvent cnEvent = new cnEvent(2, 2);
                        cnEvent.SendEvent(cnEvent);
                        if ((int)cnEvent[0] != 7)
                        {
                            cnEvent cnEvent2 = new cnEvent(2, 0);
                            cnEvent2[0] = 7;
                            cnEvent.SendEvent(cnEvent2);
                            cnEvent cnEvent3 = new cnEvent(2, 4, 0);
                            cnEvent3[0] = 7;
                            cnEvent3[1] = eJournalMode.eJM_Reward;
                            cnEvent3[2] = cnMissionNode.GetMe();
                            cnEvent3[3] = null;
                            cnEvent.SendEvent(cnEvent3);
                            return;
                        }
                        break;
                    }
                }
            }
        }
    }

    // Token: 0x06000555 RID: 1365 RVA: 0x0006393C File Offset: 0x00061B3C
    private int GetToChargeChangingGuide(int iGuideID)
    {
        cnMissionNode cnMissionNode = new cnMissionNode();
        new cnMissionNode();
        if (iGuideID <= 0)
        {
            return -1;
        }
        if (iGuideID >= 6)
        {
            return -1;
        }
        int num = 1;
        int num2 = 1;
        int iGuide = this.ownstatus.iGuide;
        if (iGuide.Equals(iGuideID))
        {
            return 0;
        }
        GuideElement guideElement = (GuideElement)TableContainer.GetTableData(4, 0, iGuideID);
        if (guideElement == null)
        {
            return -1;
        }
        for (int i = 0; i < this.m_CompletedMissionList.Count; i++)
        {
            cnMissionNode = (cnMissionNode)this.m_CompletedMissionList[i];
            if (cnMissionNode != null && cnMissionNode.GetMe() != null && iGuide.Equals(cnMissionNode.GetMe().m_iCSTReqGuide))
            {
                int num3 = (((int)localized.LevelExpansion != 1) ? cnMissionNode.GetMe().m_iCTRReqLvMin : cnMissionNode.GetMe().m_iKorStReqLvlMin);
                if (num < num3)
                {
                    num = num3;
                }
            }
        }
        bool flag = false;
        for (int j = 0; j < this.m_CompletedMissionList.Count; j++)
        {
            cnMissionNode = (cnMissionNode)this.m_CompletedMissionList[j];
            if (cnMissionNode != null && cnMissionNode.GetMe() != null)
            {
                int k = 0;
                while (k < guideElement.m_iQuest.Length)
                {
                    if (guideElement.m_iQuest[k].Equals(cnMissionNode.GetMe().m_iHMissionID))
                    {
                        if (cnMissionNode == null)
                        {
                            num2 = 1;
                            break;
                        }
                        if (cnMissionNode.GetMe() == null)
                        {
                            break;
                        }
                        int num4;
                        if ((int)localized.LevelExpansion == 1)
                        {
                            num4 = cnMissionNode.GetMe().m_iKorStReqLvlMin;
                            flag = true;
                        }
                        else
                        {
                            num4 = cnMissionNode.GetMe().m_iCTRReqLvMin;
                            flag = true;
                        }
                        if (num2 >= num4)
                        {
                            break;
                        }
                        num2 = num4;
                        if (num2 < 7)
                        {
                            flag = false;
                            break;
                        }
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
        }
        if (num2 == 1)
        {
            num2 = 4;
        }
        else if (flag)
        {
            num2++;
        }
        int num5 = num - num2 + 1;
        if (num5 <= 0)
        {
            num5 = 1;
        }
        num5 = this.ownstatus.iChangeGuideCount * num2 * 100 / num5;
        Logger.Log("ownstatus.iChangeGuideCount " + string.Concat(new string[]
        {
            this.ownstatus.iChangeGuideCount.ToString(),
            " iCurLv ",
            num.ToString(),
            " iTargetLv ",
            num2.ToString(),
            " * (int)eMissionConst.MENTOR_CHANGE_BASE_COST) ",
            100.ToString()
        }).ToString());
        Logger.Log("ownstatus.iChangeGuideCount * iTargetLv * (int)eMissionConst.MENTOR_CHANGE_BASE_COST) " + num5.ToString() + " (iCurLv - iTargetLv  + 1) " + (num - num2 + 1).ToString());
        return 0;
    }

    // Token: 0x06000556 RID: 1366 RVA: 0x00063BB8 File Offset: 0x00061DB8
    private void UpdateQuestTaskChecker()
    {
        int count = this.m_MissionTaskCheckerList.Count;
        while (0 < count--)
        {
            cnMissionTaskCheckerNode cnMissionTaskCheckerNode = (cnMissionTaskCheckerNode)this.m_MissionTaskCheckerList[count];
            if (cnMissionTaskCheckerNode != null)
            {
                if (0f >= cnMissionTaskCheckerNode.m_fLifeTime - (float)(DateTime.Now.Ticks - cnMissionTaskCheckerNode.m_lStartedTime) * 1E-07f)
                {
                    this.m_MissionTaskCheckerList.RemoveAt(count);
                }
                else
                {
                    cnMissionTaskCheckerNode.m_fRemainTime = cnMissionTaskCheckerNode.m_fLifeTime - (float)(DateTime.Now.Ticks - cnMissionTaskCheckerNode.m_lStartedTime) * 1E-07f;
                }
            }
        }
    }

    // Token: 0x06000557 RID: 1367 RVA: 0x00063C50 File Offset: 0x00061E50
    private cnMissionNode GetSelectedActiveMission()
    {
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode != null && cnMissionNode.GetMe() != null && this.m_iMenualSelectedMissionID == cnMissionNode.GetMe().m_iHMissionID)
            {
                return cnMissionNode;
            }
        }
        return null;
    }

    // Token: 0x06000558 RID: 1368 RVA: 0x00063CA8 File Offset: 0x00061EA8
    private void RequestTaskStart(int iTaskID, int iNPCID)
    {
        if (0 >= iTaskID || 0 < this.SearchMissionTaskChecker(iTaskID))
        {
            return;
        }
        this.arrRequestStartTask.Add(iTaskID);
        cnMissionNode task = this.GetTask(iTaskID);
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        sP_CL2FE_REQ_PC_TASK_START sP_CL2FE_REQ_PC_TASK_START = default(sP_CL2FE_REQ_PC_TASK_START);
        sP_CL2FE_REQ_PC_TASK_START.iTaskNum = task.GetMe().m_iHTaskID;
        if (0 < task.GetMe().m_iHNPCID)
        {
            Logger.Log("Mision NPC ID : " + task.GetMe().m_iHNPCID.ToString());
            Transform transform = this.pNpcContainer.SearchTableID(task.GetMe().m_iHNPCID);
            if (transform != null)
            {
                Status status = transform.GetComponent(typeof(Status)) as Status;
                sP_CL2FE_REQ_PC_TASK_START.iNPC_ID = status.iID;
                Logger.Log("mission npc id" + sP_CL2FE_REQ_PC_TASK_START.iNPC_ID.ToString());
            }
        }
        if (task.GetMe().m_iHTaskType == 6 && 0 < task.GetMe().m_iCSUDEFNPCID)
        {
            Transform nearbyNPC = this.GetNearbyNPC(task.GetMe().m_iCSUDEFNPCID);
            if (null != nearbyNPC)
            {
                Status status2 = nearbyNPC.GetComponent(typeof(Status)) as Status;
                sP_CL2FE_REQ_PC_TASK_START.iEscortNPC_ID = status2.iID;
            }
        }
        Logger.Log("Send Start Mission : " + iTaskID.ToString());
        task.InitializeBeforeStart();
        this.AddMissionTaskChecker(task.GetMe().m_iHTaskID);
        cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_PC_TASK_START)), 318767115, sP_CL2FE_REQ_PC_TASK_START);
    }

    // Token: 0x06000559 RID: 1369 RVA: 0x00004A3C File Offset: 0x00002C3C
    private void GetNearList(Transform tr)
    {
        this.pNpcContainer.GetNearListAnyTeamAnyType(this.m_NearList, tr.position, 5000f);
    }

    // Token: 0x0600055A RID: 1370 RVA: 0x00063E3C File Offset: 0x0006203C
    private Transform GetNearbyNPC(int iTableID)
    {
        if (0 >= iTableID)
        {
            return null;
        }
        for (int i = 0; i < this.m_NearList.iCount; i++)
        {
            Transform transform = this.m_NearList.SearchedList[i];
            if (!(transform == null))
            {
                Status status = transform.GetComponent(typeof(Status)) as Status;
                if (null != status && iTableID.Equals(status.iTableID))
                {
                    return transform;
                }
            }
        }
        return null;
    }

    // Token: 0x0600055B RID: 1371 RVA: 0x00063EB0 File Offset: 0x000620B0
    private int SearchMissionTaskChecker(int iTaskID)
    {
        for (int i = 0; i < this.m_MissionTaskCheckerList.Count; i++)
        {
            if (iTaskID.Equals(((cnMissionTaskCheckerNode)this.m_MissionTaskCheckerList[i]).GetTaskID()))
            {
                return i;
            }
        }
        return -1;
    }

    // Token: 0x0600055C RID: 1372 RVA: 0x00063EF8 File Offset: 0x000620F8
    private bool AddMissionTaskChecker(int iTaskID)
    {
        if (0 >= iTaskID)
        {
            return false;
        }
        if (this.IsExistMissionTaskChecker(iTaskID))
        {
            return false;
        }
        cnMissionTaskCheckerNode cnMissionTaskCheckerNode = new cnMissionTaskCheckerNode();
        cnMissionTaskCheckerNode.SetTaskID(iTaskID, 10f);
        this.m_MissionTaskCheckerList.Add(cnMissionTaskCheckerNode);
        return true;
    }

    // Token: 0x0600055D RID: 1373 RVA: 0x00063F38 File Offset: 0x00062138
    private bool DelMissionTaskChecker(int iTaskID)
    {
        Logger.Log("DelMissionTaskChecker TaskID: " + iTaskID.ToString());
        if (0 >= iTaskID)
        {
            return false;
        }
        for (int i = 0; i < this.m_MissionTaskCheckerList.Count; i++)
        {
            if (iTaskID.Equals(((cnMissionTaskCheckerNode)this.m_MissionTaskCheckerList[i]).GetTaskID()))
            {
                this.m_MissionTaskCheckerList.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    // Token: 0x0600055E RID: 1374 RVA: 0x00063FA8 File Offset: 0x000621A8
    private bool IsExistMissionTaskChecker(int iTaskID)
    {
        for (int i = 0; i < this.m_MissionTaskCheckerList.Count; i++)
        {
            if (iTaskID.Equals(((cnMissionTaskCheckerNode)this.m_MissionTaskCheckerList[i]).GetTaskID()))
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x0600055F RID: 1375 RVA: 0x00063FF0 File Offset: 0x000621F0
    private int SearchQuestItem(int iItemID)
    {
        cnEvent cnEvent = new cnEvent(5, 16, 7);
        cnEvent[0] = 2;
        cnEvent[1] = 0;
        cnEvent[2] = iItemID;
        cnEvent[3] = 8;
        cnEvent.SendEvent(cnEvent);
        int num = (int)cnEvent[0];
        if (num < 0)
        {
            return 0;
        }
        cnEvent cnEvent2 = new cnEvent(5, 16, 0);
        cnEvent2[0] = 2;
        cnEvent2[1] = 0;
        cnEvent2[2] = num;
        cnEvent.SendEvent(cnEvent2);
        return ((OCSlotEntity)cnEvent2[0]).Item.iItemOpt;
    }

    // Token: 0x06000560 RID: 1376 RVA: 0x000640A0 File Offset: 0x000622A0
    private bool IsExistTaskInActiveMission(int iTaskID)
    {
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode.GetMe() != null && iTaskID == cnMissionNode.GetMe().m_iHTaskID)
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06000561 RID: 1377 RVA: 0x000640F0 File Offset: 0x000622F0
    private bool IsExistTaskInCompletedMission(int iTaskID)
    {
        for (int i = 0; i < this.m_CompletedMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_CompletedMissionList[i];
            if (cnMissionNode.GetMe() != null && iTaskID == cnMissionNode.GetMe().m_iHTaskID)
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06000562 RID: 1378 RVA: 0x00064140 File Offset: 0x00062340
    private bool IsExistMissionInActieMission(int iMissionID)
    {
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode.GetMe() != null && iMissionID == cnMissionNode.GetMe().m_iHMissionID)
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06000563 RID: 1379 RVA: 0x00064190 File Offset: 0x00062390
    private bool IsExistMissionInCompletedMission(int iMissionID)
    {
        for (int i = 0; i < this.m_CompletedMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_CompletedMissionList[i];
            if (cnMissionNode.GetMe() != null && iMissionID == cnMissionNode.GetMe().m_iHMissionID)
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06000564 RID: 1380 RVA: 0x000641E0 File Offset: 0x000623E0
    private bool IsCompletedMission(int iMissionID)
    {
        for (int i = 0; i < this.m_CompletedMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_CompletedMissionList[i];
            if (cnMissionNode.GetMe() != null && iMissionID == cnMissionNode.GetMe().m_iHMissionID && cnMissionNode.IsFinalTask())
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06000565 RID: 1381 RVA: 0x00064238 File Offset: 0x00062438
    private void EliminateCompletedMission(int iMissionID)
    {
        for (int i = 0; i < this.m_CompletedMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_CompletedMissionList[i];
            if (cnMissionNode.GetMe() != null && iMissionID == cnMissionNode.GetMe().m_iHMissionID)
            {
                this.m_CompletedMissionList.Remove(cnMissionNode);
                return;
            }
        }
    }

    // Token: 0x06000566 RID: 1382 RVA: 0x00064290 File Offset: 0x00062490
    private void EliminateActiveMission(int iMissionID)
    {
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode.GetMe() != null && iMissionID == cnMissionNode.GetMe().m_iHMissionID)
            {
                this.m_ActivateMissionList.Remove(cnMissionNode);
                return;
            }
        }
    }

    // Token: 0x06000567 RID: 1383 RVA: 0x000642E8 File Offset: 0x000624E8
    private void EliminateActiveTask(int iTaskID)
    {
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
            if (cnMissionNode.GetMe() != null && iTaskID == cnMissionNode.GetMe().m_iHTaskID)
            {
                this.m_ActivateMissionList.Remove(cnMissionNode);
                return;
            }
        }
    }

    // Token: 0x06000568 RID: 1384 RVA: 0x00064340 File Offset: 0x00062540
    private void InsertActivateTask(int iTaskID)
    {
        if (this.IsExistTaskInActiveMission(iTaskID))
        {
            return;
        }
        cnMissionNode task = this.GetTask(iTaskID);
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        this.EliminateActiveMission(task.GetMe().m_iHMissionID);
        if (task.GetMe().m_iHMissionType == 2)
        {
            for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
            {
                cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[i];
                if (cnMissionNode.GetMe() != null && cnMissionNode.GetMe().m_iHMissionType == 2)
                {
                    this.m_ActivateMissionList.Remove(cnMissionNode);
                    break;
                }
            }
        }
        this.m_ActivateMissionList.Add(task);
    }

    // Token: 0x06000569 RID: 1385 RVA: 0x000643E4 File Offset: 0x000625E4
    private void InsertCompletedTask(int iTaskID)
    {
        if (!this.IsExistTaskInCompletedMission(iTaskID))
        {
            cnMissionNode task = this.GetTask(iTaskID);
            if (task != null && task.GetMe() != null)
            {
                this.EliminateCompletedMission(task.GetMe().m_iHMissionID);
                this.m_CompletedMissionList.Add(task);
            }
        }
    }

    // Token: 0x0600056A RID: 1386 RVA: 0x0006442C File Offset: 0x0006262C
    private cnMissionNode GetTask(int iTaskID)
    {
        for (int i = 0; i < this.m_MissionGroupList.Count; i++)
        {
            cnMissionNode childByTaskID = ((cnMissionNode)this.m_MissionGroupList[i]).GetChildByTaskID(iTaskID);
            if (childByTaskID != null)
            {
                return childByTaskID;
            }
        }
        Logger.Log("GetTask Fail" + iTaskID.ToString());
        return null;
    }

    // Token: 0x0600056B RID: 1387 RVA: 0x00064484 File Offset: 0x00062684
    private cnMissionNode GetMission(int iMissionID)
    {
        for (int i = 0; i < this.m_MissionGroupList.Count; i++)
        {
            if (iMissionID == ((cnMissionNode)this.m_MissionGroupList[i]).GetMissionID())
            {
                return (cnMissionNode)this.m_MissionGroupList[i];
            }
        }
        return null;
    }

    // Token: 0x0600056C RID: 1388 RVA: 0x000644D4 File Offset: 0x000626D4
    private void ReceiveStartCondition(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.CheckToStartTaskCondition(num);
    }

    // Token: 0x0600056D RID: 1389 RVA: 0x00064504 File Offset: 0x00062704
    private void ReceiveCompleteCondition(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.CheckToCompleteTaskCondition(num);
    }

    // Token: 0x0600056E RID: 1390 RVA: 0x00064534 File Offset: 0x00062734
    private void ReceiveMissionCondition(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.CheckToStartTaskCondition(num);
        myevent[1] = this.CheckToCompleteTaskCondition(num);
    }

    // Token: 0x0600056F RID: 1391 RVA: 0x00064574 File Offset: 0x00062774
    private void ReceiveGetNpcMission(cnEvent myevent)
    {
        int num = (int)myevent[0];
        if (this.m_NPCAndMissionRelationList.Count <= num)
        {
            myevent[0] = null;
            return;
        }
        myevent[0] = this.m_NPCAndMissionRelationList[num];
    }

    // Token: 0x06000570 RID: 1392 RVA: 0x000645B8 File Offset: 0x000627B8
    private void ReceiveGetTask(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.GetTask(num);
    }

    // Token: 0x06000571 RID: 1393 RVA: 0x000645E0 File Offset: 0x000627E0
    private void ReceiveGetMission(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.GetMission(num);
    }

    // Token: 0x06000572 RID: 1394 RVA: 0x00004A5A File Offset: 0x00002C5A
    private void ReceiveGetSelectMission(cnEvent myevent)
    {
        myevent[0] = this.SelectMissionTask;
    }

    // Token: 0x06000573 RID: 1395 RVA: 0x00004A69 File Offset: 0x00002C69
    private void ReceiveSetSelectMission(cnEvent myevent)
    {
        this.m_iMenualSelectedMissionID = (int)myevent[0];
        this.StartSelectMissionTask();
    }

    // Token: 0x06000574 RID: 1396 RVA: 0x00004A83 File Offset: 0x00002C83
    private void ReceiveGetActiveMission(cnEvent myevent)
    {
        myevent[0] = this.m_ActivateMissionList;
    }

    // Token: 0x06000575 RID: 1397 RVA: 0x00004A92 File Offset: 0x00002C92
    private void ReceiveGetCompletedMission(cnEvent myevent)
    {
        myevent[0] = this.m_CompletedMissionList;
    }

    // Token: 0x06000576 RID: 1398 RVA: 0x00064608 File Offset: 0x00062808
    private void ReceiveCheckMissionKilledCount(cnEvent myevent)
    {
        int num = (int)myevent[0];
        this.CheckMissionKilledCount(num);
    }

    // Token: 0x06000577 RID: 1399 RVA: 0x0006462C File Offset: 0x0006282C
    private void ReceiveCheckMissionItemCount(cnEvent myevent)
    {
        short num = (short)myevent[0];
        this.CheckMissionItemCount((int)num);
    }

    // Token: 0x06000578 RID: 1400 RVA: 0x00064650 File Offset: 0x00062850
    private void ReceiveKillMissionNPC(cnEvent myevent)
    {
        int num = (int)myevent[0];
        this.KillMissionNPC(num);
    }

    // Token: 0x06000579 RID: 1401 RVA: 0x00064674 File Offset: 0x00062874
    private void ReceiveRequestTaskStart(cnEvent myevent)
    {
        int num = (int)myevent[0];
        int num2 = (int)myevent[1];
        this.RequestTaskStart(num, num2);
    }

    // Token: 0x0600057A RID: 1402 RVA: 0x000646A4 File Offset: 0x000628A4
    private void ReceiveRequestTaskEnd(cnEvent myevent)
    {
        int num = (int)myevent[0];
        int num2 = (int)myevent[1];
        this.RequestTaskComplete(num, num2, false);
    }

    // Token: 0x0600057B RID: 1403 RVA: 0x000049FF File Offset: 0x00002BFF
    private void ReceiveClearMissionSystemManagerData(cnEvent myevent)
    {
        this.Init();
    }

    // Token: 0x0600057C RID: 1404 RVA: 0x000646D4 File Offset: 0x000628D4
    private void ReceiveCheckToActiveTask(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.IsExistTaskInActiveMission(num);
    }

    // Token: 0x0600057D RID: 1405 RVA: 0x00064704 File Offset: 0x00062904
    private void ReceiveGetToChargeChangingGuide(cnEvent myevent)
    {
        int num = (int)myevent[0];
        myevent[0] = this.GetToChargeChangingGuide(num);
    }

    // Token: 0x0600057E RID: 1406 RVA: 0x00064734 File Offset: 0x00062934
    private void SetBubbleChat(eMissionProcess eProcess, cnMissionNode node)
    {
        string text = "";
        int num = 0;
        int num2 = 0;
        switch (eProcess)
        {
        case eMissionProcess.Start:
            if (node.GetMe().m_iSTDialogBubble > 0 && this.missionscript.m_pMissionStringData.Length > node.GetMe().m_iSTDialogBubble)
            {
                text = this.missionscript.m_pMissionStringData[node.GetMe().m_iSTDialogBubble].m_pstrNameString;
                num = node.GetMe().m_iSTDialogBubbleNPCID;
            }
            num2 = node.GetMe().m_iHNPCID;
            break;
        case eMissionProcess.Success:
        case eMissionProcess.Complete:
            if (node.GetMe().m_iSUDialogBubble > 0 && this.missionscript.m_pMissionStringData.Length > node.GetMe().m_iSUDialogBubble)
            {
                text = this.missionscript.m_pMissionStringData[node.GetMe().m_iSUDialogBubble].m_pstrNameString;
                num = node.GetMe().m_iSUDialogBubbleNPCID;
            }
            num2 = node.GetMe().m_iHTerminatorNPCID;
            break;
        case eMissionProcess.Fail:
            if (node.GetMe().m_iFDialogBubble > 0 && this.missionscript.m_pMissionStringData.Length > node.GetMe().m_iFDialogBubble)
            {
                text = this.missionscript.m_pMissionStringData[node.GetMe().m_iFDialogBubble].m_pstrNameString;
                num = node.GetMe().m_iFDialogBubbleNPCID;
            }
            break;
        }
        if (num > 0 && text.Length > 1)
        {
            this.m_NearList.Clear();
            this.GetNearList(this.myuser);
            Transform nearbyNPC = this.GetNearbyNPC(num);
            if (nearbyNPC == null)
            {
                return;
            }
            PrintName printName = nearbyNPC.GetComponent(typeof(PrintName)) as PrintName;
            if (printName)
            {
                printName.SetChatBubble(text, eChatbubbleType.QuestChat);
            }
        }
        if (num2 <= 0)
        {
            Logger.Log("SetBubbleChat : iVoiceID == 0");
            return;
        }
        NpcTableElement npcTableElement = (NpcTableElement)TableContainer.GetTableData(10, 0, num2);
        if (npcTableElement == null)
        {
            return;
        }
        StringTableElement stringTableElement = (StringTableElement)TableContainer.GetTableData(10, 1, npcTableElement.m_iNpcName);
        if (stringTableElement == null)
        {
            return;
        }
        Transform nearbyNPC2 = this.GetNearbyNPC(num2);
        if (eProcess != eMissionProcess.Start)
        {
            if (eProcess != eMissionProcess.Complete)
            {
                return;
            }
            AvatarUtil.CallVoicePlay(nearbyNPC2, stringTableElement.m_strComment2, eVoice.QuestEnd);
            return;
        }
        else
        {
            if (node.GetMe().m_iHTaskType == 6)
            {
                AvatarUtil.CallVoicePlay(nearbyNPC2, stringTableElement.m_strComment2, eVoice.QuestStartEscort);
                return;
            }
            AvatarUtil.CallVoicePlay(nearbyNPC2, stringTableElement.m_strComment2, eVoice.QuestAcce);
            return;
        }
    }

    // Token: 0x0600057F RID: 1407 RVA: 0x00064964 File Offset: 0x00062B64
    private void SetMissionMessage(int iNpc, int iMessagetype, int iTextNameIndex, int MessageIndex, int BtnType, int iNano)
    {
        if (iTextNameIndex <= 0 || MessageIndex <= 0)
        {
            return;
        }
        if ((iMessagetype & 2) != 0)
        {
            NameStringElement nameStringElement = this.missionscript.m_pMissionStringData[MessageIndex];
            if (nameStringElement.m_pstrNameString.Length <= 1)
            {
                return;
            }
            cnGUINanocom.SendMissionMessageBox(base.gameObject, iNpc, iNano, nameStringElement.m_pstrNameString, BtnType, null, "", "", null);
        }
        if ((iMessagetype & 4) != 0)
        {
            cnEvent cnEvent = new cnEvent(2, 4, 19);
            cnEvent[0] = 5;
            cnEvent[1] = 0;
            cnEvent.SendEvent(cnEvent);
        }
    }

    // Token: 0x06000580 RID: 1408 RVA: 0x000649F0 File Offset: 0x00062BF0
    private void StartSelectMissionTask()
    {
        cnMissionNode selectMissionTask = this.SelectMissionTask;
        this.SelectMissionTask = this.GetSelectedActiveMission();
        string text = "StartSelectMissionTask ";
        cnMissionNode selectMissionTask2 = this.SelectMissionTask;
        Logger.Log(text + ((selectMissionTask2 != null) ? selectMissionTask2.ToString() : null));
        if (this.SelectMissionTask != null)
        {
            if (0 < this.SelectMissionTask.GetMe().m_iSTGrantWayPoint)
            {
                Vector3 clientNpcPos = WorldDataContainer.GetClientNpcPos(this.SelectMissionTask.GetMe().m_iSTGrantWayPoint);
                if (!clientNpcPos.Equals(Vector3.zero))
                {
                    cnEvent cnEvent = new cnEvent(2, 4, 10);
                    cnEvent[0] = 5;
                    cnEvent[1] = true;
                    cnEvent[2] = clientNpcPos;
                    cnEvent.SendEvent(cnEvent);
                }
            }
            else
            {
                cnEvent cnEvent2 = new cnEvent(2, 4, 10);
                cnEvent2[0] = 5;
                cnEvent2[1] = false;
                cnEvent.SendEvent(cnEvent2);
            }
            if (this.myuser)
            {
                this.m_NearList.Clear();
                this.GetNearList(this.myuser);
                this.RefreshQuestSymbol();
            }
            if (selectMissionTask != this.SelectMissionTask)
            {
                sP_CL2FE_REQ_PC_SET_CURRENT_MISSION_ID sP_CL2FE_REQ_PC_SET_CURRENT_MISSION_ID = default(sP_CL2FE_REQ_PC_SET_CURRENT_MISSION_ID);
                sP_CL2FE_REQ_PC_SET_CURRENT_MISSION_ID.iCurrentMissionID = this.SelectMissionTask.GetMe().m_iHMissionID;
                cnEvent.SendPacket(Marshal.SizeOf(typeof(sP_CL2FE_REQ_PC_SET_CURRENT_MISSION_ID)), 318767235, sP_CL2FE_REQ_PC_SET_CURRENT_MISSION_ID);
                return;
            }
        }
        else
        {
            cnEvent cnEvent3 = new cnEvent(2, 4, 10);
            cnEvent3[0] = 5;
            cnEvent3[1] = false;
            cnEvent.SendEvent(cnEvent3);
        }
    }

    // Token: 0x06000581 RID: 1409 RVA: 0x00064B78 File Offset: 0x00062D78
    private void ReceiveStartGames(cnEvent myevent)
    {
        if (!GameFrame.IsReadyForPlay())
        {
            this.bStartFlag = true;
            return;
        }
        if (cntutorialscript.bTutorial)
        {
            return;
        }
        int num = 0;
        int num2 = 0;
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            int iHMissionType = ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe().m_iHMissionType;
            if (iHMissionType != 1)
            {
                if (iHMissionType == 2)
                {
                    num2++;
                }
            }
            else
            {
                num++;
            }
        }
        int num3 = 0;
        MissionElement[] pMissionData = this.missionscript.m_pMissionData;
        int num4 = pMissionData.Length;
        for (int j = 0; j < num4; j++)
        {
            MissionElement missionElement = pMissionData[j];
            int num5 = 0;
            while (num5 < this.m_ActivateMissionList.Count && ((cnMissionNode)this.m_ActivateMissionList[num5]).GetMe().m_iHMissionID != missionElement.m_iHMissionID)
            {
                num3++;
                num5++;
            }
            if (this.m_ActivateMissionList.Count <= num3)
            {
                num3 = 0;
                int num6 = 0;
                while (num6 < this.m_CompletedMissionList.Count && ((cnMissionNode)this.m_CompletedMissionList[num6]).GetMe().m_iHMissionID != missionElement.m_iHMissionID)
                {
                    num3++;
                    num6++;
                }
                if (this.m_CompletedMissionList.Count <= num3)
                {
                    if (missionElement.m_iHMissionType == 1)
                    {
                        if (1 <= num)
                        {
                            goto IL_01BF;
                        }
                    }
                    else if (missionElement.m_iHMissionType != 2 || 1 <= num2)
                    {
                        goto IL_01BF;
                    }
                    int num7 = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        num7 += missionElement.m_iMentorEmailID[k];
                    }
                    if (0 < num7)
                    {
                        bool flag = this.CheckToStartTaskCondition(missionElement.m_iHTaskID) != 0;
                        this.CheckToCompleteTaskCondition(missionElement.m_iHTaskID);
                        if (!flag && (missionElement.m_iSTMessageType & 2) != 0)
                        {
                            if (missionElement.m_iHMissionType == 1)
                            {
                                num++;
                            }
                            else if (missionElement.m_iHMissionType == 2)
                            {
                                num2++;
                            }
                        }
                    }
                }
            }
            IL_01BF:;
        }
        cnEvent.SendEvent(new cnEvent(15, 7));
        cnEmailSystemManager cnEmailSystemManager = (cnEmailSystemManager)GlobalManager.GetManager(15).GetComponent(typeof(cnEmailSystemManager));
        bool flag2 = false;
        for (int l = 0; l < cnEmailSystemManager.EmailNodeList.Count; l++)
        {
            if (l < cnEmailSystemManager.EmailNodeList.Count && cnEmailSystemManager.EmailNodeList[l].iMode == 2)
            {
                flag2 = true;
                break;
            }
        }
        GuideTableScript guideTableScript = (GuideTableScript)TableContainer.GetTable(4);
        GuideElement guideElement = (GuideElement)guideTableScript.GetAt(0, this.ownstatus.iGuide);
        if (guideElement != null)
        {
            GuideStringElement guideStringElement = ((myevent != null) ? ((GuideStringElement)guideTableScript.GetAt(1, guideElement.m_iLevelUp)) : ((!flag2) ? ((GuideStringElement)guideTableScript.GetAt(1, guideElement.m_iLoginNomail)) : ((GuideStringElement)guideTableScript.GetAt(1, guideElement.m_iLoginMail))));
            if (!cnFirstUseSysManager.GetCheckCondition(46))
            {
                base.StartCoroutine(this.CallGuideMessage(base.gameObject, this.GuideNpcNum[this.ownstatus.iGuide - 1], guideStringElement.m_pszString));
                return;
            }
            cnGUINanocom.SendMessageBox(base.gameObject, this.GuideNpcNum[this.ownstatus.iGuide - 1], guideStringElement.m_pszString, 9, null, "", "", null);
        }
    }

    // Token: 0x06000582 RID: 1410 RVA: 0x00004AA1 File Offset: 0x00002CA1
    private IEnumerator CallGuideMessage(GameObject go, int npcNum, string str)
    {
        yield return new WaitForSeconds(8f);
        cnGUINanocom.SendMessageBox(go, npcNum, str, 9, null, "", "", null);
        yield break;
    }

    // Token: 0x06000583 RID: 1411 RVA: 0x00004ABE File Offset: 0x00002CBE
    private IEnumerator NanoTimeEffect()
    {
        AssetBundleRequest assetBundleRequest = EffectController.PreloadEffect(710);
        if (assetBundleRequest != null)
        {
            while (!assetBundleRequest.isDone)
            {
                yield return null;
            }
        }
        if (this.myuser == null)
        {
            yield break;
        }
        GameObject gameObject = EffectController.InstantiateEffect(710, this.myuser.position, this.myuser.rotation);
        int num = 0;
        while (gameObject == null && num <= 10)
        {
            gameObject = EffectController.InstantiateEffect(710, this.myuser.position, this.myuser.rotation);
            if (gameObject == null)
            {
                yield return new WaitForSeconds(0.1f);
                num++;
            }
        }
        if (gameObject != null)
        {
            gameObject.transform.parent = this.myuser;
        }
        yield break;
    }

    // Token: 0x06000584 RID: 1412 RVA: 0x00064EB0 File Offset: 0x000630B0
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

    // Token: 0x06000585 RID: 1413 RVA: 0x0006530C File Offset: 0x0006350C
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

    // Token: 0x06000586 RID: 1414 RVA: 0x00065350 File Offset: 0x00063550
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

    // Token: 0x06000587 RID: 1415 RVA: 0x0006561C File Offset: 0x0006381C
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

    // Token: 0x06000588 RID: 1416 RVA: 0x00004ACD File Offset: 0x00002CCD
    private void ReceiveMissionClear(cnEvent myevent)
    {
        this.m_ActivateMissionList.Clear();
    }

    // Token: 0x06000589 RID: 1417 RVA: 0x00065840 File Offset: 0x00063A40
    private void SetMissionAndTaskFlags(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_ENTER_SUCC sP_FE2CL_REP_PC_ENTER_SUCC = (sP_FE2CL_REP_PC_ENTER_SUCC)myevent[0];
        this.m_iMenualSelectedMissionID = 0;
        this.m_CompletedMissionList.Clear();
        this.m_ActivateMissionList.Clear();
        this.m_iRepeatQuestFlag = sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aRepeatQuestFlag;
        int num = Marshal.SizeOf(sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aQuestFlag[0]) * 8;
        for (int i = 0; i < 16; i++)
        {
            int j = 0;
            int num2 = 1;
            while (j < num)
            {
                cnMissionNode mission = this.GetMission(1 + i * num + j);
                if (mission == null)
                {
                    num2 *= 2;
                }
                else
                {
                    mission.SetMissionGroupState(0);
                    bool flag = ((sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aQuestFlag[i] >> j) & 1L) != 0L;
                    num2 *= 2;
                    if (flag)
                    {
                        mission.SetMissionGroupState(2);
                        cnMissionNode finalTask = mission.GetFinalTask();
                        if (finalTask != null && finalTask.GetMe() != null)
                        {
                            this.InsertCompletedTask(finalTask.GetMe().m_iHTaskID);
                        }
                    }
                }
                j++;
            }
        }
        int num3 = 0;
        while ((long)num3 < 9L)
        {
            if (sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aRunningQuest[num3].m_aCurrTaskID > 0)
            {
                cnMissionNode task = this.GetTask(sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aRunningQuest[num3].m_aCurrTaskID);
                Logger.Log(string.Concat(new string[]
                {
                    "active task id : ",
                    task.GetMe().m_iHTaskID.ToString(),
                    " mission id : ",
                    task.GetMe().m_iHMissionID.ToString(),
                    " slot : ",
                    num3.ToString()
                }));
                if (task.GetMe().m_iHMissionType == 2 && num3 != 0)
                {
                    Logger.LogError("nano mission must 0 slot.. wrong slot :" + num3.ToString());
                }
                else
                {
                    task.SetMissionState(1);
                    task.InitializeBeforeStart();
                    this.InsertActivateTask(sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aRunningQuest[num3].m_aCurrTaskID);
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (task.GetMe().m_iCSUEnemyID[k] == sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aRunningQuest[num3].m_aKillNPCID[l])
                            {
                                task.m_aiCurrentRemainEnemyNum[k] = sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.aRunningQuest[num3].m_aKillNPCCount[l];
                            }
                        }
                    }
                    for (int m = 0; m < 3; m++)
                    {
                        if (task.GetMe().m_iCSUItemID[m] > 0)
                        {
                            task.m_aiCurrentRemainItemNum[m] = task.GetMe().m_iCSUItemNumNeeded[m];
                            cnEvent cnEvent = new cnEvent(5, 16, 7);
                            cnEvent[0] = 2;
                            cnEvent[1] = 0;
                            cnEvent[2] = task.GetMe().m_iCSUItemID[m];
                            cnEvent[3] = 8;
                            cnEvent.SendEvent(cnEvent);
                            int num4 = (int)cnEvent[0];
                            if (num4 >= 0)
                            {
                                cnEvent cnEvent2 = new cnEvent(5, 16, 0);
                                cnEvent2[0] = 2;
                                cnEvent2[1] = 0;
                                cnEvent2[2] = num4;
                                cnEvent.SendEvent(cnEvent2);
                                OCSlotEntity ocslotEntity = (OCSlotEntity)cnEvent2[0];
                                task.m_aiCurrentRemainItemNum[m] -= ocslotEntity.Item.iItemOpt;
                            }
                            Logger.Log("remain item id : " + task.GetMe().m_iCSUItemID[m].ToString() + " count " + task.m_aiCurrentRemainItemNum[m].ToString());
                        }
                    }
                }
            }
            num3++;
        }
        if (sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.iCurrentMissionID != 0)
        {
            this.m_iMenualSelectedMissionID = sP_FE2CL_REP_PC_ENTER_SUCC.PCLoadData2CL.iCurrentMissionID;
            this.SelectMissionTask = this.GetSelectedActiveMission();
        }
        else if (this.m_ActivateMissionList.Count > 0)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)this.m_ActivateMissionList[this.m_ActivateMissionList.Count - 1];
            this.m_iMenualSelectedMissionID = cnMissionNode.GetMe().m_iHMissionID;
        }
        Logger.Log("StartSelectMission");
        this.StartSelectMissionTask();
    }

    // Token: 0x0600058A RID: 1418 RVA: 0x00065C7C File Offset: 0x00063E7C
    private void DelActiveMissionSucc(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_STOP_SUCC sP_FE2CL_REP_PC_TASK_STOP_SUCC = (sP_FE2CL_REP_PC_TASK_STOP_SUCC)myevent[0];
        this.DelMissionTaskChecker(sP_FE2CL_REP_PC_TASK_STOP_SUCC.iTaskNum);
        cnMissionNode task = this.GetTask(sP_FE2CL_REP_PC_TASK_STOP_SUCC.iTaskNum);
        if (task == null || task.GetMe() == null)
        {
            return;
        }
        this.EliminateCompletedMission(task.GetMe().m_iHMissionID);
        if (this.IsExistTaskInActiveMission(sP_FE2CL_REP_PC_TASK_STOP_SUCC.iTaskNum))
        {
            this.EliminateActiveTask(sP_FE2CL_REP_PC_TASK_STOP_SUCC.iTaskNum);
            Logger.Log(string.Concat(new string[]
            {
                "DelActiveMissionSucc ",
                this.m_iMenualSelectedMissionID.ToString(),
                " ",
                task.GetMe().m_iHMissionID.ToString(),
                " ",
                this.m_ActivateMissionList.Count.ToString()
            }));
            if (this.m_iMenualSelectedMissionID == task.GetMe().m_iHMissionID || this.m_ActivateMissionList.Count <= 0)
            {
                this.ResetSelectMission();
            }
        }
    }

    // Token: 0x0600058B RID: 1419 RVA: 0x00004ADA File Offset: 0x00002CDA
    private void DelActiveMissionFail(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_STOP_FAIL sP_FE2CL_REP_PC_TASK_STOP_FAIL = (sP_FE2CL_REP_PC_TASK_STOP_FAIL)myevent[0];
    }

    // Token: 0x0600058C RID: 1420 RVA: 0x00065D70 File Offset: 0x00063F70
    private void CheckNanoFreeTuning(cnEvent ev)
    {
        int num = 0;
        while ((long)num < (long)((ulong)csDefines.SIZEOF_NANO_BANK_SLOT))
        {
            if (this.ownstatus.aNanoBank[num].iID != 0 && this.ownstatus.aNanoBank[num].iSkillID == 0)
            {
                cnEvent cnEvent = new cnEvent(2, 0);
                cnEvent[0] = 22;
                cnEvent.SendEvent(cnEvent);
                cnEvent cnEvent2 = new cnEvent(2, 3, 0);
                cnEvent2[0] = num;
                cnEvent.SendEvent(cnEvent2);
                ev.iReturn = 1;
                return;
            }
            num++;
        }
        ev.iReturn = 0;
    }

    // Token: 0x0600058D RID: 1421 RVA: 0x00065E04 File Offset: 0x00064004
    private void ReceiveGetMissionNano(cnEvent ev)
    {
        foreach (object obj in this.m_ActivateMissionList)
        {
            cnMissionNode cnMissionNode = (cnMissionNode)obj;
            if (cnMissionNode.GetMe().m_iHMissionType == 2)
            {
                ev[0] = cnMissionNode;
                return;
            }
        }
        ev[0] = null;
    }

    // Token: 0x0600058E RID: 1422 RVA: 0x00065E78 File Offset: 0x00064078
    public bool NPCTypeHasMissionAvailable(int id)
    {
        if (this.NpcTable == null && !this.LoadNPCTable())
        {
            return false;
        }
        NpcTableElement npcTableElement = this.NpcTable.m_pNpcData[id];
        if (npcTableElement == null)
        {
            return false;
        }
        if (npcTableElement.m_iNpcType == 0 || npcTableElement.m_iNpcType == 25 || npcTableElement.m_iNpcType == 100 || npcTableElement.m_iNpcType == 101 || npcTableElement.m_iNpcType == 105 || npcTableElement.m_iNpcType == 110 || npcTableElement.m_iNpcType == 111)
        {
            return false;
        }
        cnNPCAndMissionRelationNode cnNPCAndMissionRelationNode = (cnNPCAndMissionRelationNode)this.m_NPCAndMissionRelationList[id];
        int i = 0;
        while (i < cnNPCAndMissionRelationNode.m_GrantTaskList.Count)
        {
            MissionElement missionElement = (MissionElement)cnNPCAndMissionRelationNode.m_GrantTaskList[i];
            if (missionElement.m_iRepeatflag == 0)
            {
                bool flag = false;
                for (int j = 0; j < this.m_CompletedMissionList.Count; j++)
                {
                    if (((cnMissionNode)this.m_CompletedMissionList[j]).GetMe().m_iHMissionID == missionElement.m_iHMissionID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    goto IL_00F8;
                }
            }
            else if (!this.IsRepeatCompleated(missionElement.m_iRepeatflag))
            {
                goto IL_00F8;
            }
            IL_0148:
            i++;
            continue;
            IL_00F8:
            for (int k = 0; k < this.m_ActivateMissionList.Count; k++)
            {
                if (((cnMissionNode)this.m_ActivateMissionList[k]).GetMe().m_iHTaskID == missionElement.m_iHTaskID)
                {
                    return true;
                }
            }
            if (this.CheckToStartTaskCondition(missionElement.m_iHTaskID) == 0)
            {
                return true;
            }
            goto IL_0148;
        }
        return false;
    }

    // Token: 0x0600058F RID: 1423 RVA: 0x00065FE4 File Offset: 0x000641E4
    public bool NPCTypeHasMissionAdvanceAvailable(int id)
    {
        if (this.NpcTable == null && !this.LoadNPCTable())
        {
            return false;
        }
        NpcTableElement npcTableElement = this.NpcTable.m_pNpcData[id];
        if (npcTableElement == null)
        {
            return false;
        }
        if (npcTableElement.m_iNpcType == 0 || npcTableElement.m_iNpcType == 25 || npcTableElement.m_iNpcType == 100 || npcTableElement.m_iNpcType == 101 || npcTableElement.m_iNpcType == 105 || npcTableElement.m_iNpcType == 110 || npcTableElement.m_iNpcType == 111)
        {
            return false;
        }
        for (int i = 0; i < this.m_ActivateMissionList.Count; i++)
        {
            MissionElement me = ((cnMissionNode)this.m_ActivateMissionList[i]).GetMe();
            foreach (MissionElement terminateTask in ((cnNPCAndMissionRelationNode)this.m_NPCAndMissionRelationList[npcTableElement.m_iNpcNumber]).m_TerminateTaskList)
            {
                if (terminateTask.m_iHTaskID == me.m_iHTaskID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Token: 0x06000591 RID: 1425 RVA: 0x00066190 File Offset: 0x00064390
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

    // Token: 0x06000592 RID: 1426 RVA: 0x000662DC File Offset: 0x000644DC
    private cnMissionNode GetNextTaskNode(cnMissionNode current)
    {
        cnMissionNode parents = current.GetParents();
        if (parents == null)
        {
            return null;
        }
        int num = -1;
        for (int i = 0; i < parents.GetChildSize(); i++)
        {
            if (parents.GetChildByIndex(i) == current)
            {
                num = i;
                break;
            }
        }
        if (num >= 0 && num + 1 < parents.GetChildSize())
        {
            return parents.GetChildByIndex(num + 1);
        }
        return null;
    }

    // Token: 0x04000837 RID: 2103
    public static string OwnName = "MissionManager";

    // Token: 0x04000838 RID: 2104
    public static cnMissionManager instance;

    // Token: 0x04000839 RID: 2105
    private cnMissionManager.ManagerFunc[] arrayCallFunc = new cnMissionManager.ManagerFunc[34];

    // Token: 0x0400083A RID: 2106
    private ArrayList m_MissionGroupList = new ArrayList();

    // Token: 0x0400083B RID: 2107
    private ArrayList m_NPCAndMissionRelationList = new ArrayList();

    // Token: 0x0400083C RID: 2108
    private ArrayList m_MissionTaskCheckerList = new ArrayList();

    // Token: 0x0400083D RID: 2109
    public ArrayList m_ActivateMissionList = new ArrayList();

    // Token: 0x0400083E RID: 2110
    public ArrayList m_CompletedMissionList = new ArrayList();

    // Token: 0x0400083F RID: 2111
    private SearchList m_NearList = new SearchList();

    // Token: 0x04000840 RID: 2112
    private float m_fLastRefreshedTime;

    // Token: 0x04000841 RID: 2113
    private int m_iloopTemp;

    // Token: 0x04000842 RID: 2114
    public int m_iMenualSelectedMissionID;

    // Token: 0x04000843 RID: 2115
    private Transform myuser;

    // Token: 0x04000844 RID: 2116
    private cnOwnAvatarStatus ownstatus;

    // Token: 0x04000845 RID: 2117
    private Status status;

    // Token: 0x04000846 RID: 2118
    private cnAvatarStatus avstatus;

    // Token: 0x04000847 RID: 2119
    private int[] GuideNpcNum = new int[] { 707, 728, 731, 732, 1171 };

    // Token: 0x04000848 RID: 2120
    public MissionTableScript missionscript;

    // Token: 0x04000849 RID: 2121
    public cnMissionNode SelectMissionTask;

    // Token: 0x0400084A RID: 2122
    private NpcContainer pNpcContainer;

    // Token: 0x0400084B RID: 2123
    public long[] m_iRepeatQuestFlag = new long[8];

    // Token: 0x0400084C RID: 2124
    private ArrayList arrRequestStartTask = new ArrayList();

    // Token: 0x0400084D RID: 2125
    private bool bStartFlag;

    // Token: 0x0400084E RID: 2126
    private NpcTableScript NpcTable;

    // Token: 0x0400281C RID: 10268
    private bool bForceCompleteChain;

    // Token: 0x0400281D RID: 10269
    private int m_iForceCompleteChainDepth;

    // Token: 0x0400281E RID: 10270
    private int m_iForceCompletePendingTaskId;

    // Token: 0x0400281F RID: 10271
    private int m_iForceCompleteRetryCount;

    // Token: 0x020000A4 RID: 164
    public enum FuncType
    {
        // Token: 0x04000850 RID: 2128
        GetActiveMission,
        // Token: 0x04000851 RID: 2129
        GetCompleteMission,
        // Token: 0x04000852 RID: 2130
        CheckMissionKillCount,
        // Token: 0x04000853 RID: 2131
        CheckMissionItemCount,
        // Token: 0x04000854 RID: 2132
        KillMissionNPC,
        // Token: 0x04000855 RID: 2133
        RequestTaskStart,
        // Token: 0x04000856 RID: 2134
        RequestTaskEnd,
        // Token: 0x04000857 RID: 2135
        ClearMissionManager,
        // Token: 0x04000858 RID: 2136
        CheckToActiveTask,
        // Token: 0x04000859 RID: 2137
        GetToChargeChangeGuide,
        // Token: 0x0400085A RID: 2138
        GetNpcMission,
        // Token: 0x0400085B RID: 2139
        SetAvatar,
        // Token: 0x0400085C RID: 2140
        GetStartCondition,
        // Token: 0x0400085D RID: 2141
        GetCompleteCondition,
        // Token: 0x0400085E RID: 2142
        GetMissionCondition,
        // Token: 0x0400085F RID: 2143
        GetMissionTask,
        // Token: 0x04000860 RID: 2144
        GetMission,
        // Token: 0x04000861 RID: 2145
        GetSelectMission,
        // Token: 0x04000862 RID: 2146
        SetSelectMission,
        // Token: 0x04000863 RID: 2147
        StartGame,
        // Token: 0x04000864 RID: 2148
        CheckEscortQuest,
        // Token: 0x04000865 RID: 2149
        ProcessStartSucc,
        // Token: 0x04000866 RID: 2150
        ProcessStartFail,
        // Token: 0x04000867 RID: 2151
        ProsessEndSucc,
        // Token: 0x04000868 RID: 2152
        ProsessEndFail,
        // Token: 0x04000869 RID: 2153
        SetMissionTaskFlag,
        // Token: 0x0400086A RID: 2154
        DelActiveMissionSucc,
        // Token: 0x0400086B RID: 2155
        DelActiveMissionFail,
        // Token: 0x0400086C RID: 2156
        ReceiveCheckNanoFreeTuning,
        // Token: 0x0400086D RID: 2157
        CheckWarpMission,
        // Token: 0x0400086E RID: 2158
        ChangeGuide,
        // Token: 0x0400086F RID: 2159
        GetMissionNano,
        // Token: 0x04000870 RID: 2160
        MissionClear,
        // Token: 0x04000871 RID: 2161
        IsRepeatCompleated,
        // Token: 0x04000872 RID: 2162
        Max
    }

    // Token: 0x020000A5 RID: 165
    // (Invoke) Token: 0x06000594 RID: 1428
    public delegate void ManagerFunc(cnEvent myevent);
}
