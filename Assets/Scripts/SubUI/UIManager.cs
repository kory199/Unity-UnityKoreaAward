using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    Scene,
    Popup,
    Global,
}

public class One_UIManager : MonoSingleton<One_UIManager>
{
    private Stack<GameObject> _popupStack = new Stack<GameObject>();
    public Stack<GameObject> PopupStack { get { return _popupStack; } set { _popupStack = value; } }

    private Dictionary<UIType, GameObject> _canvases = new();

    private string _basePath = "UI/";

    private int _sortingOrder = 0;

    #region 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InstUIManager()
    {
        GameObject uiManager = new GameObject().AddComponent<One_UIManager>();
        uiManager.name = "One_UIManager";
    }
    #endregion
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start() => SetUp();

    private void SetUp()
    {
        foreach (var type in Enum.GetValues(typeof(UIType)))
        {
            CreateCanvas((UIType)type);
        }
    }

    private void PushPopupStack(GameObject popup)
    {
        _popupStack.Push(popup);
    }

    private GameObject PopPopupStack()
    {
        if (_popupStack.Count == 0)
        {
            Debug.Log("Stack is null");
            return null;
        }
        GameObject popup = _popupStack.Pop();
        popup.SetActive(false);
        return popup;
    }

    public T CreateUIObject<T>(string prefabName, UIType type, Action action = null)
    {
        if (false == _canvases.ContainsKey(type))
        {
            CreateCanvas(type);
        }
        var targetCanvas = _canvases[type];
        string path = string.Concat(_basePath, prefabName);
        GameObject newUI = Resources.Load<GameObject>(path);
        GameObject newUIObject = Instantiate(newUI);
        newUIObject.transform.SetParent(targetCanvas.transform);

        newUIObject.GetComponent<IPopupBase>().Init(() =>
        {
            action();
            PopPopupStack();
        });
        PushPopupStack(newUIObject);
        T returnScript;
        newUIObject.TryGetComponent<T>(out returnScript);
        return returnScript;
    }
    public GameObject CreateUIObject(string prefabName, UIType type, Action action = null)
    {
        if (false == _canvases.ContainsKey(type))
        {
            CreateCanvas(type);
        }
        var targetCanvas = _canvases[type];
        string path = string.Concat(_basePath, prefabName);
        GameObject newUI = Resources.Load<GameObject>(path);
        GameObject newUIObject = Instantiate(newUI);
        newUIObject.transform.SetParent(targetCanvas.transform);

        newUIObject.GetComponent<IPopupBase>().Init(() =>
        {
            action();
            PopPopupStack();
        });
        PushPopupStack(newUIObject);
        return newUIObject;
    }
    private void CreateCanvas(UIType type)
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
}