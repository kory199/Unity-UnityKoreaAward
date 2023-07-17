using UnityEngine;
using UnityEngine.SceneManagement;
using static EnumTypes;
public class Global : MonoBehaviour
{
    #region Singleton
    public static Global Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                        _instance = new Global();
                }
            }
            return _instance;
        }
    }

    private static Global _instance;
    private static readonly object _instanceLock = new object();
    private Global() { }
    #endregion

    public bool isSceneTitle { get { return SceneManager.GetActiveScene().name == nameof(Scenes.SceneTitle); } }
    public bool isSceneLobby { get { return SceneManager.GetActiveScene().name == nameof(Scenes.SceneLobby); } }
    public bool isSceneInGame { get { return SceneManager.GetActiveScene().name == nameof(Scenes.SceneInGame); } }

}
