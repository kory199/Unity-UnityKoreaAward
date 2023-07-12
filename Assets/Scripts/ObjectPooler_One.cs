using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPooler_One))]
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
    public string name;
    public GameObject prefab;
    public int number;
    public bool isUi;
}
public class ObjectPooler_One : MonoBehaviour
{
    private static ObjectPooler_One inst;
    private void Awake()
    {
        inst = this;
    }
    [SerializeField] private RectTransform Ui_CanvasPooler;
    [SerializeField] private Pool[] pools;
    private List<GameObject> spawnObjcets;
    Dictionary<string, Queue<GameObject>> dictionaryPool;
    //지울 부분
    readonly string INFO = 
        " 풀링될 오브젝트의 OnDisable() 안에 다음을 적으세용~ " +
        "\nvoid OnDisable()\n" +
        "{\nObjectPooler.ReturnToPool(gameobject); \n" +
        "CancelInvoke(); //invoke 함수를 사용하는 경우적어주세요\n}";

    private void Start()
    {
        spawnObjcets = new List<GameObject>();
        dictionaryPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            dictionaryPool.Add(pool.name, new Queue<GameObject>());
            for (int i = 0; i < pool.number; i++)
            {
                GameObject obj;
                //Ingame 오브젝트와 UI 오브젝트 구분
                if (pool.isUi == false)
                    obj = CreateNewObject(pool.name, pool.prefab);
                else
                    obj = CreateNewObject(pool.name, pool.prefab, Ui_CanvasPooler);

                if (pool.isUi == false)
                    ArrangePool(obj);
                else
                    ArrangePool(obj, Ui_CanvasPooler);
            }
            //OnDisable에 ReturnToPool 구현 여부중복 구현 검사
            if (dictionaryPool[pool.name].Count <= 0)
                Debug.LogError($"{pool.name}{INFO}");
            else if (dictionaryPool[pool.name].Count != pool.number)
                Debug.LogError($"{pool.name}에 returnToPool이 중복됩니다");
        }
    }

    //변수에 따른 오버로딩
    public static GameObject SpawnFromPool(string name, Vector3 position) =>
        inst._SpawnFromPool(name, position, Quaternion.identity);
    public static GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation) =>
        inst._SpawnFromPool(name, position, rotation);

    public static T SpawnFromPool<T>(string name, Vector3 position) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(name, position, Quaternion.identity);
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
        GameObject obj = inst._SpawnFromPool(name, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!dictionaryPool.ContainsKey(name))
            throw new Exception($"pool with name {name} doesn't exist");
        //큐에 없으면 새로 추가
        Queue<GameObject> poolQueue = dictionaryPool[name];
        if (poolQueue.Count == 0)
        {
            Pool pool = Array.Find(pools, x => x.name == name);
            GameObject obj;
            if (pool.isUi == false)
                obj = CreateNewObject(pool.name, pool.prefab);
            else
                obj = CreateNewObject(pool.name, pool.prefab, Ui_CanvasPooler);
            if (pool.isUi == false)
                ArrangePool(obj);
            else
                ArrangePool(obj, Ui_CanvasPooler);

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
        if (!inst.dictionaryPool.ContainsKey(name))
            throw new Exception($"Pool with name {name} doesn't exist");

        return inst.spawnObjcets.FindAll(x => x.name == name);
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
        if (inst.dictionaryPool.ContainsKey(obj.name) == false)
            throw new Exception($"Pool with name{obj.name} dosen't exist");

        inst.dictionaryPool[obj.name].Enqueue(obj);
    }

    [ContextMenu("GetSpawnObjectsInfo")]
    void GetSpawnObjectInfo()
    {
        foreach (var pool in pools)
        {
            int count = spawnObjcets.FindAll(x => x.name == pool.name).Count; //리스트의 개수 
            Debug.Log($"{pool.name} Count : {count}");
        }
    }

    GameObject CreateNewObject(string name, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = name;
        obj.SetActive(false); //비활성화시 ReturnToPool을 하기 때문에 enqueue된다.
        return obj;
    }
    GameObject CreateNewObject(string name, GameObject prefab, RectTransform trans)
    {
        var obj = Instantiate(prefab, trans);
        obj.name = name;
        obj.SetActive(false); //비활성화시 ReturnToPool을 하기 때문에 enqueue된다.
        return obj;
    }
    #region ArrangePool overroding
    private void ArrangePool(GameObject obj)
    {
        //추가된 오브젝트 묶어서 정렬
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjcets.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjcets.Insert(i, obj);
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
                spawnObjcets.Insert(i, obj);
                break;
            }
            else if (parent.transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjcets.Insert(i, obj);
            }
        }
    }
    #endregion
}
