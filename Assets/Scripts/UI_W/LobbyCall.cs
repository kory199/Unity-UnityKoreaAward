using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCall : MonoBehaviour
{
    private void Start()
    {
        UI_SceneLobby uI_SceneLobby = UIManager.Instance.CreateObject
            <UI_SceneLobby>("UI_SceneLobby",EnumTypes.LayoutType.First);

        uI_SceneLobby.OnShow();
    }
}
