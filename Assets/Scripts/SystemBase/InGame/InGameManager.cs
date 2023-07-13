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
    }

    public void InvokeCallBacks(EnumTypes.InGameParamType _type, int _callBakcIndex)
    {
        InGameParamBase _param = null;

        switch (_type)
        {
            case EnumTypes.InGameParamType.Player:
                if(_parameters.TryGetValue(_type, out _param) == false)
                {
                    return;
                }
                InGamePlayerParams _players = _param as InGamePlayerParams;
                _players.InvokeCallBack(_callBakcIndex);
                break;
        }
    }
}


