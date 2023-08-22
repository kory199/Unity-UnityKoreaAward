using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyCall : MonoBehaviour
{
    UI_SceneLobby uI_SceneLobby;
    private void Start()
    {
        uI_SceneLobby = UIManager.Instance.CreateObject<UI_SceneLobby>
            ("UI_SceneLobby", EnumTypes.LayoutType.First);

        uI_SceneLobby.OnShow();
    }
}