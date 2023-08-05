using Unity.VisualScripting;
using UnityEngine;

public class MonoSingleton<T>  : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;

    public static T Instance 
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

    protected virtual void Awake()
    {
        //Debug.Log("******After Awake " + typeof(T).Name);
    }

    protected virtual void OnDestroy()
    {
        //Debug.Log($"*****Destroying singleton: {typeof(T).Name}");
    }
}