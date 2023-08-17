using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using APIModels;
using System.Collections;

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

    public PlayerData playerData { get; set; }
    public int StageNum { get; set; }
    // private EventSystem _eventSystem;

    // === Cursor
    Texture2D _cursorImg;

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

        _cursorImg = Resources.Load<Texture2D>("cursor");
        Cursor.SetCursor(_cursorImg, Vector2.zero, CursorMode.Auto);
    }

    public void SetStageNum(int stageNum)
    {
        OnclickStageNum = stageNum;
    }
    public void EndStage(int stageNum)
    {
        //∫Í∑π¿Ã≈© ¿ÃπÃ¡ˆ ∂ÁøÏ∞Ì æ¿¿Ãµø
        Debug.Log("Ω¬∏Æ");
        StartCoroutine(Co_EndStage());
    }
    IEnumerator Co_EndStage()
    {
        yield return new WaitForSeconds(3);
        MoveScene("SceneLobby");
    }
    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public async UniTask LoadAsync(EnumTypes.ScenesType scene)
    {
        UniTask loadSceneUnitask = LoadScene(scene);

        await loadSceneUnitask;
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
