using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UI_LodingBar : UIBase
{
    [SerializeField] Slider slider = null;

    bool result = true;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    protected override void Start()
    {
        StartCoroutine(LoadAsyncSceneCoroutine());
    }

    IEnumerator LoadAsyncSceneCoroutine()
    {
        float duration = 2f;
        float currentTime = 0f;

        while (currentTime <= duration)
        {
            float value = currentTime / duration;
            slider.value = value;
            currentTime += Time.deltaTime;

            if (currentTime >= 1f)
            {
                break;
            }

            yield return null;
        }

        yield return GetPlayerDataCoroutine();

        while (currentTime <= duration)
        {
            float value = currentTime / duration;
            slider.value = value;
            currentTime += Time.deltaTime;
            yield return null;
        }

        if(result)
        {
            slider.value = 1f;
            OnHide();
            MoveScene();
        }
    }

    private IEnumerator GetPlayerDataCoroutine()
    {
        var task = GetPlayerData();
        yield return task.ToCoroutine();
    }

    private async UniTask GetPlayerData()
    {
        result = await APIManager.Instance.GetStageAPI();
        if (result == false)
        {
            Debug.LogWarning($"GetStageAPI Error");
        }
    }

    private async void MoveScene()
    {
        await GameManager.Instance.MoveSceneWithAction(EnumTypes.ScenesType.SceneLobby);
    }
}