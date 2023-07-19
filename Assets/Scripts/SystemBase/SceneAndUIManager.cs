using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SceneAndUIManager : UIBase
{
    private EventSystem _eventSystem;

    // 런타임 초기화 시점에 SceneManager 생성
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InstSceneManager()
    {
        var sceneAndUIManager = new GameObject().AddComponent<SceneAndUIManager>();
        sceneAndUIManager.name = "SceneAndUIManager";
    }

    protected override void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Unitask를 활용한 비동기 씬 로드 및 비동기 UI 생성
    public async UniTask LoadAsync(EnumTypes.ScenesType scene)
    {
        // UI 비동기 생성
        UniTask creatUIUnitask = CreatUI(scene);

        // 씬 비동기 로드
        UniTask loadSceneUnitask = LoadScene(scene);

        // 해당 씬의 로드 작업 및 UI생성 작업 완료 시
        await UniTask.WhenAll(creatUIUnitask, loadSceneUnitask);
    }

    private async UniTask CreatUI(EnumTypes.ScenesType scene)
    {
        switch (scene)
        {
            case EnumTypes.ScenesType.SceneInGame:
                // SceneInGame UI 생성 함수 호출
                InitGameUI();
                break;
            case EnumTypes.ScenesType.SceneLobby:
                // SceneLobby UI 생성 함수 호출
                InitLobbyUI();
                break;
            case EnumTypes.ScenesType.SceneTitle:
                // SceneTitle UI 생성 함수 호출
                InitTitleUI();
                break;
            default:
                Debug.LogError("Require scene name resolution");
                break;
        }
        await UniTask.CompletedTask;
    }

    private async UniTask LoadScene(EnumTypes.ScenesType scene)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene.ToString());

        // Scene Load가 완료될 때까지 대기
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

    public override IProcess.NextProcess ProcessInput()
    {
        return IProcess.NextProcess.Continue;
    }
}
