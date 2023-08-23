using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LodingBar : UIBase
{
    [SerializeField] Slider slider = null;
    [SerializeField] EnumTypes.ScenesType scenesType;

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
            yield return null;
        }

        slider.value = 1f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenesType.ToString());
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnHide();
    }
}