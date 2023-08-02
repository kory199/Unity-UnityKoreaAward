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
    // ���� Getset���� Ư�� UI �Ŵ����� ��ũ��Ʈ�� ������Ƽ�� ������ �� �ְ� ����� �˴ϴ�
    public StringLocalizer stringLocallizer { get; set; } // �̰� �Ŵ����� UI�� ���� ���� ���� ���� ����
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

        // Type�� Canvas ����
        foreach (var type in Enum.GetValues(typeof(LayoutType)))
        {
            CreateCanvas((LayoutType)type);
        }

        GetLanguageData();
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    // ������Ʈ�� ��ũ��Ʈ�� �־�� �մϴ�
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

    // GameObject�� ���� ��ȯ �մϴ�.
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

    // enum Ÿ�Կ� ���ǵ� �� ��ŭ ĵ������ �����մϴ�.
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
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

     protected void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public override IProcess.NextProcess ProcessInput()
    {
        return IProcess.NextProcess.Continue;
    }
}