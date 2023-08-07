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
    [SerializeField] private float _SpawnRandomFactor = 1f;
    [SerializeField] private bool _isBoss = false;
    WaitForSeconds spawnDelay;

    private Coroutine _monsterSpawnRoutine = null;

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
    public void SettingMonsterSpawnNum(int melee, int ranged, bool isBoss = false, string name = null)
    {
        _isBoss = isBoss;
        _meleeMonsterNum = melee;
        _rangedMonsterNum = ranged;
        _spawnMonsterTotalNum = melee + ranged;

        // ===
        if (_monsterSpawnRoutine != null)
        {
            StopCoroutine(_monsterSpawnRoutine);
        }

        StartCoroutine(Co_MonsterSpawn());
    }
    IEnumerator Co_MonsterSpawn(string name = null)
    {
        //Ǯ���� ���� ������
        while (!_isBoss)
        {
            if (_meleeMonsterNum > 0 || _rangedMonsterNum > 0)
            {
                SpawnMonsters();
                yield return spawnDelay;
            }
            else
            {
                break; 
            }
        }
        if (_isBoss)
        {
            GameObject Boss = ObjectPooler.SpawnFromPool(name, Vector3.up * 10);
        }

        _monsterSpawnRoutine = null;
    }

    #region �ߺ����� ���� �̱�
    private void SpawnMonsters(string name = null)
    {
        List<int> tempNum = new List<int>();
        for (int i = 0; i < _spawnPos.Count; i++)
        {
            tempNum.Add(i);
        }
        //���� ����
        for (int i = 0; i < _meleeMonsterNum; i++)
        {
            int num = RandomChoose(tempNum.Count - 1);
            tempNum.Remove(num);

            Vector3 spawnRandomArea = _spawnPos[tempNum[num]] + (Vector3)Random.insideUnitCircle * _SpawnRandomFactor;
            GameObject monster = ObjectPooler.SpawnFromPool("BasicMeleeMonster", spawnRandomArea);
        }
    }
    private int RandomChoose(int gap) => Random.Range(0, gap);
    #endregion
}
