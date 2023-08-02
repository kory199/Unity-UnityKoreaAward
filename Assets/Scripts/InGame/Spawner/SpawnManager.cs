using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Vector3> _spawnPos = new List<Vector3>();
    [SerializeField] private int _meleeMonsterNum;
    [SerializeField] private int _rangedMonsterNum;
    [SerializeField] private int _spawnMonsterTotalNum;
    [SerializeField] private float _spawnTime = 3f;
    [SerializeField] private float _SpawnRandomFactor = 2f;
    WaitForSeconds spawnDelay;

    #region Unity Life Cycle
    // Start is called before the first frame update
    void Start()
    {
        spawnDelay = new WaitForSeconds(_spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
    public void SettingMonsterSpawnNum(int melee, int ranged)
    {
        _meleeMonsterNum = melee;
        _rangedMonsterNum = ranged;
        _spawnMonsterTotalNum = melee + ranged;
        StopCoroutine(Co_MonsterSpawn());
        StartCoroutine(Co_MonsterSpawn());
    }
    IEnumerator Co_MonsterSpawn()
    {
        //Ǯ���� ���� ������
        while (!Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("���� ������");
            SpawnMonsters();
            yield return spawnDelay;
        }
    }


    private void SpawnMonsters()
    {
        List<int> tempNum = new List<int>();
        for (int i = 0; i < _spawnPos.Count; i++)
        {
            tempNum.Add(i);
        }

        //���� ����
        for (int i = 0; i < _spawnMonsterTotalNum; i++)
        {
            int num = RandomChoose(tempNum.Count - 1);
            tempNum.Remove(num);
            Debug.Log(num);
            Debug.Log(tempNum[num]);

            Vector3 spawnRandomArea = _spawnPos[tempNum[num]] + (Vector3)Random.insideUnitCircle * _SpawnRandomFactor;

            GameObject monster = ObjectPooler.SpawnFromPool("BasicMeleeMonster", spawnRandomArea);
        }

    }
    private int RandomChoose(int gap) => Random.Range(0, gap);
}
