using Unity.VisualScripting;
using UnityEngine;

public class MonoSingleton<T>  : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;
    private static bool isQuitting = false;

    public static T Instacne 
    {
        get 
        {
            if(_instance == null)
            {
                _instance = GameManager.FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).ToString(), typeof(T));
                    _instance = singleton.GetComponent<T>();
                }
            }
            
            return _instance;
        }
    }

    protected virtual void OnDestroy()
    {
        if (isQuitting)
        {
            _instance = null;
        }
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        isQuitting = true;

        if(_instance != null)
        {
            Destroy(_instance.gameObject);
        }
    }
#endif
}