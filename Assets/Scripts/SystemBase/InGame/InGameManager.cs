using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumTypes;

public class InGameManager : MonoSingleton<InGameManager>
{
    private Dictionary<EnumTypes.InGameParamType, InGameParamBase> _parameters = new();

    private void Awake()
    {
        //
        RegisterParams(EnumTypes.InGameParamType.Player, (int)EnumTypes.PlayerSkiilsType.MAX);
    }

    public void RegisterParams(EnumTypes.InGameParamType paramType, int maxTypeNum)
    {
        if (_parameters.ContainsKey(paramType)) return;
        _parameters.Add(paramType, new InGamePlayerParams(maxTypeNum));
    }
    public void InvokeCallBacks(EnumTypes.InGameParamType type, int callBackIndex)
    {
        InGameParamBase param = null;
        switch (type)
        {
            case EnumTypes.InGameParamType.Player:
                if (_parameters.TryGetValue(type, out param) == false)
                {
                    return;
                }
                InGamePlayerParams players = param as InGamePlayerParams;
                players.InvokeCallBack(callBackIndex);
                break;
            case EnumTypes.InGameParamType.Stage:
                if (_parameters.TryGetValue(type, out param) == false)
                {
                    return;
                }
                InGamePlayerParams stages = param as InGamePlayerParams;
                stages.InvokeCallBack(callBackIndex);
                break;
        }
    }
    /// <summary>
    /// 게임 변수 타입에 맞춰 action 지정
    /// </summary>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public void AddActionType<TEnum>(EnumTypes.InGameParamType param, TEnum actionType, UnityAction action)
    {
        int idx = GetEnumNumber(actionType);
        if (idx < 0)
        {
            Debug.LogError("This Enum Type is not exist");
            return;
        }
        //Debug.Log("actionType : " + actionType.ToString()+" " + _parameters[param].GetActionCount());
        _parameters[param].AddCallBack(idx, action);
    }
    private void Start()
    {
        // AddActionType(EnumTypes.InGameParamType.Player, PlayerSkiilsType.DoubleShot, () => Debug.Log("더블샷"));
        // _parameters[EnumTypes.InGameParamType.Player].InvokeCallBack(EnumTypes.PlayerSkiilsType.DoubleShot);
    }
}