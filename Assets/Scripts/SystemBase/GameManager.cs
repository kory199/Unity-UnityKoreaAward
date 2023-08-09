using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum SceneState
{
    Title,
    Lobby,
    Game,
}
public class GameManager : MonoSingleton<GameManager>
{
    public int OnclickStageNum { get; private set; }
    public SceneState SceneState { get; set; }
    // private EventSystem _eventSystem;

    // Runtume init Gamemanager 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InstSceneManager()
    {
        var sceneAndUIManager = new GameObject().AddComponent<GameManager>();
        sceneAndUIManager.name = "GameManager";
    }

    protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetStageNum(int stageNum)
    {
        OnclickStageNum = stageNum;
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public async UniTask LoadAsync(EnumTypes.ScenesType scene)
    {
        UniTask loadSceneUnitask = LoadScene(scene);

        await UniTask.WhenAll(loadSceneUnitask);
    }

    public async UniTask LoadScene(EnumTypes.ScenesType scene)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene.ToString());

        while (!sceneLoad.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
