using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PingforServer : MonoBehaviour
{
    private const float _pingInterval = 120f;
    private bool _isPlayerQuit = false;
    private bool _isCrashed = false;
    private bool _isGameActive = true;

    private void Start()
    {
        SendPingToServerPriodically().Forget();
    }

    private async UniTaskVoid SendPingToServerPriodically()
    {
        while(_isGameActive)
        {
            SendExitStatusToServer(PingMessage.playGame.ToString());
            await UniTask.Delay((int)(_pingInterval * 1000));
        }
    }

    public void OnApplicationQuit()
    {
        _isGameActive = false;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            SendExitStatusToServer(PingMessage.InternerError.ToString());
        }
        else if (_isPlayerQuit)
        {
            SendExitStatusToServer(PingMessage.PlayerQuit.ToString());
        }
        else if (_isCrashed)
        {
            SendExitStatusToServer(PingMessage.Crash.ToString());
        }
        else
        {
            SendExitStatusToServer(PingMessage.NormalExit.ToString());
        }
    }

    public void SendExitStatusToServer(string status)
    {
        
    }

    public void OnPlayerQuit()
    {
        _isPlayerQuit = true;
    }
}