using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameManager : MonoSingleton<InGameManager>
{
    private Dictionary<EnumTypes.InGameParamType, InGameParamBase> _parameters = new();

    private void Awake()
    {
        RegisterParams();
    }

    private void RegisterParams()
    {
        _parameters.Add(EnumTypes.InGameParamType.Player, new InGamePlayerParams());
        _parameters[EnumTypes.InGameParamType.Player].AddCallBack((int)EnumTypes.PlayerStateType.Death,()=> { });
    }
    public void InvokeCallBacks(EnumTypes.InGameParamType type, int callBackIndex)
    {
        InGameParamBase param = null;

        switch (type)
        {
            case EnumTypes.InGameParamType.Player:
                if(_parameters.TryGetValue(type, out param) == false)
                {
                    return;
                }
                InGamePlayerParams players = param as InGamePlayerParams;
                players.InvokeCallBack(callBackIndex);
                break;
        }
    }
    public void AddActionType(EnumTypes.InGameParamType type,UnityAction action)
    {
        _parameters[EnumTypes.InGameParamType.Player].AddCallBack((int)EnumTypes.PlayerStateType.Death, action);
    }

}


