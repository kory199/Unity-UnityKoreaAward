using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TermsofUser
{
    public int useGameService;
    public int getUserInfo;
    public int userGetEvent;
    public int userNightGetEvent;
}

public class TermsofUserCode : MonoBehaviour
{
    public TermsofUser termsofUser = new TermsofUser();

    public int SetUseGameService(int isCheck) => termsofUser.useGameService = isCheck;
    public int GetUseGameSetvice() => termsofUser.useGameService;

    public int SetGetUserInfo(int isCheck) => termsofUser.getUserInfo = isCheck;
    public int GetUesrInfo() => termsofUser.getUserInfo;

    public int SetUserGetEvent(int isCheck) => termsofUser.userGetEvent = isCheck;
    public int GetUserGetEvent() => termsofUser.userGetEvent;

    public int SetUserNightGetEvent(int isCheck) => termsofUser.userNightGetEvent = isCheck;
    public int GetUserNightGetEvent() => termsofUser.userNightGetEvent;

    public TermsofUser GetUserCheck()
    {
        TermsofUser userCheck = new TermsofUser();
        userCheck.useGameService = GetUseGameSetvice();
        userCheck.getUserInfo = GetUesrInfo();
        userCheck.userGetEvent = GetUserGetEvent();
        userCheck.userNightGetEvent = GetUserNightGetEvent();

        print($"userCheck 1 : {userCheck.useGameService}, 2 : {userCheck.getUserInfo}, " +
            $"3 : {userCheck.userGetEvent}, 4 : {userCheck.userNightGetEvent}");

        if(userCheck.useGameService == 0 && userCheck.getUserInfo == 0)
        {
            return null;
        }

        return userCheck;
    }
}

public enum isCheck
{
    check = 0,
    uncheck = 1
}