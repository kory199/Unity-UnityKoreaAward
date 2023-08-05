using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
    const string INFO = 
        " 풀링될 오브젝트의 OnDisable() 안에 다음을 적으세용~ " +
        "\nvoid OnDisable()\n" +
        "{\nObjectPooler.ReturnToPool(gameobject); \n" +
        "CancelInvoke(); //invoke 함수를 사용하는 경우적어주세요\n}";
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif

[System.Serializable]
public class Pool
{
    public string Name;
    public GameObject Prefab;
    public int Number;
    public bool IsUi;
}

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    [SerializeField] private RectTransform _uiCanvasPooler;
    [SerializeField] private Pool[] _pools;
    private List<GameObject> _spawnObjects;
    private Dictionary<string, Queue<GameObject>> _dictionaryPool;
    //나중에 필요시 초기화 값으로 변경할 수 있음
    readonly string INFO = 
        " 풀링될 오브젝트의 OnDisable() 안에 다음을 적으세용~ " +
        "\nvoid OnDisable()\n" +
        "{\nObjectPooler.ReturnToPool(gameobject); \n" +
        "CancelInvoke(); //invoke 함수를 사용하는 경우적어주세요\n}";

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _spawnObjects = new List<GameObject>();
        _dictionaryPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in _pools)
        {
            _dictionaryPool.Add(pool.Name, new Queue<GameObject>());
            for (int i = 0; i < pool.Number; i++)
            {
                GameObject obj;
                //Ingame 오브젝트와 UI 오브젝트 구분
                if (pool.IsUi == false)
                    obj = CreateNewObject(pool.Name, pool.Prefab);
                else
                    obj = CreateNewObject(pool.Name, pool.Prefab, _uiCanvasPooler);

                if (pool.IsUi == false)
                    ArrangePool(obj);
                else
                    ArrangePool(obj, _uiCanvasPooler);
            }
            //OnDisable에 ReturnToPool 구현 여부중복 구현 검사
            if (_dictionaryPool[pool.Name].Count <= 0)
                Debug.LogError($"{pool.Name}{INFO}");
            else if (_dictionaryPool[pool.Name].Count != pool.Number)
                Debug.LogError($"{pool.Name}에 returnToPool이 중복됩니다");
        }
    }

    //변수에 따른 오버로딩
    public static GameObject SpawnFromPool(string name, Vector3 position) =>
        Instance._SpawnFromPool(name, position, Quaternion.identity);
    public static GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation) =>
        Instance._SpawnFromPool(name, position, rotation);

    public static T SpawnFromPool<T>(string name, Vector3 position) where T : Component
    {
        GameObject obj = Instance._SpawnFromPool(name, position, Quaternion.identity);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    public static T SpawnFromPool<T>(string name, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = Instance._SpawnFromPool(name, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    private GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!_dictionaryPool.ContainsKey(name))
            throw new Exception($"pool with name {name} doesn't exist");
        //큐에 없으면 새로 추가
        Queue<GameObject> poolQueue = _dictionaryPool[name];
        if (poolQueue.Count == 0)
        {
            Pool pool = Array.Find(_pools, x => x.Name == name);
            GameObject obj;
            if (pool.IsUi == false)
                obj = CreateNewObject(pool.Name, pool.Prefab);
            else
                obj = CreateNewObject(pool.Name, pool.Prefab, _uiCanvasPooler);
            if (pool.IsUi == false)
                ArrangePool(obj);
            else
                ArrangePool(obj, _uiCanvasPooler);

        }
        //큐에서 꺼내서 사용
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }
    public static List<GameObject> GetAllPools(string name)
    {
        if (!Instance._dictionaryPool.ContainsKey(name))
            throw new Exception($"Pool with name {name} doesn't exist");

        return Instance._spawnObjects.FindAll(x => x.name == name);
    }

    public static List<T> GetAllPools<T>(string name) where T : Component
    {
        List<GameObject> objects = GetAllPools(name);
        if (objects[0].TryGetComponent(out T component))
            throw new Exception("Component not Fount");
        return objects.ConvertAll(x => x.GetComponent<T>());
    }
    public static void ReturnToPool(GameObject obj)
    {
        if (Instance._dictionaryPool.ContainsKey(obj.name) == false)
            throw new Exception($"Pool with name{obj.name} dosen't exist");

        Instance._dictionaryPool[obj.name].Enqueue(obj);
    }

    //풀의 오브젝트 정보를 보기위해서 에디터에 버튼 만드는 것
    [ContextMenu("GetSpawnObjectsInfo")]
    private void GetSpawnObjectInfo()
    {
        foreach (var pool in _pools)
        {
            int count = _spawnObjects.FindAll(x => x.name == pool.Name).Count; //리스트의 개수 
            Debug.Log($"{pool.Name} Count : {count}");
        }
    }

    private GameObject CreateNewObject(string name, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = name;
        obj.SetActive(false); //비활성화시 ReturnToPool을 하기 때문에 enqueue된다.
        return obj;
    }
    private GameObject CreateNewObject(string name, GameObject prefab, RectTransform trans)
    {
        var obj = Instantiate(prefab, trans);
        obj.name = name;
        obj.SetActive(false); //비활성화시 ReturnToPool을 하기 때문에 enqueue된다.
        return obj;
    }
    #region ArrangePool overroding
    /// <summary>
    /// 오브젝트 풀링에 의해 생성된 객체들을 이름별로 하이어라키뷰에 정리해주는 함수
    /// </summary>
    /// <param name="obj"></param>
    private void ArrangePool(GameObject obj)
    {
        //추가된 오브젝트 묶어서 정렬
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1) //마지막인 경우
            {
                obj.transform.SetSiblingIndex(i);
                _spawnObjects.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                _spawnObjects.Insert(i, obj);
            }
        }
    }

    private void ArrangePool(GameObject obj, RectTransform parent)
    {
        //추가된 오브젝트 묶어서 정렬
        bool isFind = false;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (i == parent.transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                _spawnObjects.Insert(i, obj);
                break;
            }
            else if (parent.transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                _spawnObjects.Insert(i, obj);
            }
        }
    }
    #endregion
}
