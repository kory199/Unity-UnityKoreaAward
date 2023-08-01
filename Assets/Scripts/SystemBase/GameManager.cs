using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
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
