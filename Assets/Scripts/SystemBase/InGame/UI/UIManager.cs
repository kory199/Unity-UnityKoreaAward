using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static EnumTypes;

public class UIManager : UIBase
{
    public static UIManager Instance { get { return _instance; } }
    public static bool isExistence = false;
    public ProcessManager processManager { get { return _processManager; } }

    #region Language ETC
    public Action onChangeLanguage { get; set; } = null;
    public LanguageType language { get; set; } = LanguageType.Eng;
    #endregion

    #region GET UI OBJECTS
    // 여기 Getset으로 특정 UI 매니저급 스크립트는 프로퍼티로 가져갈 수 있게 만들면 됩니당
    public StringLocalizer stringLocallizer { get; set; } // 이건 매니저급 UI에 대한 예시 추후 삭제 예정
    #endregion

    private static UIManager _instance = null;
    private ProcessManager _processManager = null;
    private Dictionary<LayoutType, GameObject> _canvases = new();

    private const string _languageKey = "LANGUAGE";

    private int     _sortingOrder = 0;
    private string  _basePath = "UI/";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InstUIManager()
    {
        JsonLoader.Instance.Load();

        var uiManager = new GameObject().AddComponent<UIManager>();
        uiManager.name = "UIManager";
    }

    protected override void Awake()
    {
        if (_instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        isExistence = true;

        DontDestroyOnLoad(this.gameObject);
    }

    protected override void Start() => SetUp();       

    private void SetUp()
    {
        _processManager = this.gameObject.AddComponent<ProcessManager>();
        _processManager.processingUIStack.Push(this);

        // Type별 Canvas 생성
        foreach (var type in Enum.GetValues(typeof(LayoutType)))
        {
            CreateCanvas((LayoutType)type);
        }

        GetLanguageData();
    }

    private void GetLanguageData()
    {
        int languagePrefData = PlayerPrefs.GetInt(_languageKey, -1);
        if (-1 == languagePrefData)
        {
            language = LanguageType.Eng;
            //Debug.LogError($"languagePrefData is -1 - language set {nameof(language)}");
            return;
        }
        language = (LanguageType)languagePrefData;
    }

    // 오브젝트에 스크립트가 있어야 합니다
    public T CreateObject<T>(string argPath, LayoutType type)
    {
        if (false == _canvases.ContainsKey(type))
        {
            CreateCanvas(type);
        }
        var targetCanvas = _canvases[type];
        string path = string.Concat(_basePath, argPath);
        GameObject newUI = Resources.Load<GameObject>(path);
        GameObject newUIInstance = Instantiate(newUI);
        newUIInstance.transform.SetParent(targetCanvas.transform);
        var targetComponent = newUIInstance.GetComponent<T>();
        return newUIInstance.GetComponent<T>();
    }

    // GameObject를 생성 반환 합니다.
    public GameObject CreateUIObject(string argPath, LayoutType type)
    {
        if (false == _canvases.ContainsKey(type))
        {
            CreateCanvas(type);
        }
        var targetCanvas = _canvases[type];
        string path = string.Concat(_basePath, argPath);
        GameObject newUI = Resources.Load<GameObject>(path);
        GameObject newUIInstance = Instantiate(newUI);
        newUIInstance.transform.SetParent(targetCanvas.transform);
        return newUIInstance;
    }

    // enum 타입에 정의된 수 만큼 캔버스를 생성합니다.
    private void CreateCanvas(LayoutType type)
    {
        if (true == _canvases.ContainsKey(type))
            return;

        Canvas newCanvas;
        newCanvas = new GameObject().AddComponent<Canvas>();
        newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        newCanvas.sortingOrder = _sortingOrder;
        ++_sortingOrder;
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GraphicRaycaster>();
        newCanvas.name = string.Concat(type, " Canvas");
        newCanvas.transform.SetParent(transform);
        _canvases.Add(type, newCanvas.gameObject);
    }

    private void ShowUI(LayoutType layoutType)
    {
        if (_canvases.ContainsKey(layoutType))
        {
            _canvases[layoutType].SetActive(true);
        }
    }

    private void HideUI(LayoutType layoutType)
    {
        if (_canvases.ContainsKey(layoutType))
        {
            _canvases[layoutType].SetActive(false);
        }
    }

    public override IProcess.NextProcess ProcessInput()
    {
        return IProcess.NextProcess.Continue;

    }
}