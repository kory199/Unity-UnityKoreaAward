using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public enum SceneState
{
    Title,
    Lobby,
    Game,
}

public class GameManager : MonoSingleton<GameManager>
{
    public SceneState SceneState { get; set; }

    public int StageNum { get; set; }
    public int score { get; set; } 

    Texture2D _cursorImg;


    protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        _cursorImg = Resources.Load<Texture2D>("Sprites/cursor");
        Cursor.SetCursor(_cursorImg, Vector2.zero, CursorMode.Auto);

        // === Json data load ===
        JsonLoader.Instance.Load();
    }

    public async void EndStage(int stageNum)
    {
        //브레이크 이미지 띄우고 씬이동
        Debug.Log("승리");
        await MoveSceneWithAction(EnumTypes.ScenesType.SceneLobby);
    }
  
    public async UniTask MoveSceneWithAction(EnumTypes.ScenesType scene, Action actionBeforeLoad = null)
    {
        if (actionBeforeLoad != null)
        {
            actionBeforeLoad.Invoke();
        }

        try
        {
            var sceneLoad = SceneManager.LoadSceneAsync(scene.ToString());

            await UniTask.WaitUntil(() => sceneLoad.isDone);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading scene: {e.Message}");
        }
    }
}