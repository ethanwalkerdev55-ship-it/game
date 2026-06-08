// Edit Method on ProcessStartFail

private void ProcessStartFail(cnEvent myevent)
    {
        sP_FE2CL_REP_PC_TASK_START_FAIL sP_FE2CL_REP_PC_TASK_START_FAIL = (sP_FE2CL_REP_PC_TASK_START_FAIL)myevent[0];
        Logger.Log("ProcessStartFail tasknumber : " + sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum.ToString());
        this.DelMissionTaskChecker(sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum);
        if (this.bForceCompleteChain)
        {
            this.ForceCompleteOnStartFail(sP_FE2CL_REP_PC_TASK_START_FAIL.iTaskNum);
        }
    }
