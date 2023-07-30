using UnityEngine;
using UnityEngine.SceneManagement;
using static EnumTypes;

public class TitleManager_T : MonoBehaviour
{
    private UIManager _uiManager;
    private const string PrefabPath = "Resources/UI/Title_Canvas";

    private void Start()
    {
        _uiManager = UIManager.Instance;

        // 타이틀 씬이 로드될 때 UI를 생성합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == nameof(ScenesType.SceneTitle))
        {
            _uiManager.CreateUIObject("Title_Canvas", LayoutType.Title);
        }
    }
}