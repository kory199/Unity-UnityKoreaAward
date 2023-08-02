using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SceneAndUIManager : MonoSingleton<SceneAndUIManager>
{/*
    private EventSystem _eventSystem;

    // ��Ÿ�� �ʱ�ȭ ������ SceneManager ����
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InstSceneManager()
    {
        var sceneAndUIManager = new GameObject().AddComponent<SceneAndUIManager>();
        sceneAndUIManager.name = "SceneAndUIManager";
    }
*/
    protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Unitask�� Ȱ���� �񵿱� �� �ε� �� �񵿱� UI ����
    public async UniTask LoadAsync(EnumTypes.ScenesType scene)
    {
        // UI �񵿱� ����
        UniTask creatUIUnitask = CreatUI(scene);

        // �� �񵿱� �ε�
        UniTask loadSceneUnitask = LoadScene(scene);

        // �ش� ���� �ε� �۾� �� UI���� �۾� �Ϸ� ��
        await UniTask.WhenAll(creatUIUnitask, loadSceneUnitask);
    }

    public async UniTask CreatUI(EnumTypes.ScenesType scene)
    {
        switch (scene)
        {
            case EnumTypes.ScenesType.SceneInGame:
                // SceneInGame UI ���� �Լ� ȣ��
                InitGameUI();
                break;
            case EnumTypes.ScenesType.SceneLobby:
                // SceneLobby UI ���� �Լ� ȣ��
                InitLobbyUI();
                break;
            case EnumTypes.ScenesType.SceneTitle:
                // SceneTitle UI ���� �Լ� ȣ��
                InitTitleUI();
                break;
            default:
                Debug.LogError("Require scene name resolution");
                break;
        }
        await UniTask.CompletedTask;
    }

    public async UniTask LoadScene(EnumTypes.ScenesType scene)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene.ToString());

        // Scene Load�� �Ϸ�� ������ ���
        while (!sceneLoad.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }


    private void InitGameUI()
    {
        UIManager.Instance.CreateUIObject("InGame", EnumTypes.LayoutType.Middle);
    }

    private void InitLobbyUI()
    {

    }

    private void InitTitleUI()
    {

    }
/*
    public override IProcess.NextProcess ProcessInput()
    {
        return IProcess.NextProcess.Continue;
    }
*/
}
