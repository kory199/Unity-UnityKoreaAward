using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LodingTest_W : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    
    void Awake()
    {
        StartCoroutine(LoadAsyncSceneCoroutine());
    }

    IEnumerator LoadAsyncSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("SceneInGame");
        operation.allowSceneActivation = false;

        float time = 0f;

        while (!operation.isDone)
        {
            time += Time.time;

            slider.value = time / 10f;

            if (time >= 20)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}