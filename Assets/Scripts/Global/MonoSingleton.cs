using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static volatile T _instance = null;
    private static object _lock = new();

    public static T Instance 
    {
        get
        {

            /*if (_instance == null && Time.timeScale != 0)
            {
                lock (_lock)
                {
                    _instance = GameManager.FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        DontDestroyOnLoad(_instance);
                    }
                }
            }

            return _instance;*/
            #region defualt
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
            }

            return _instance;
            #endregion
        }
    }
    protected virtual void Awake()
    {
        if (_instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Debug.Log("Success Create Instance : " + gameObject.name);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        gameObject.SetActive(false);
        DestroyImmediate(gameObject);
        _instance = null;
    }

    protected virtual void OnDisable()
    {
        _instance = null;
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

}
