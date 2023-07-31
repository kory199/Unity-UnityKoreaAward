using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    GameObject meleeMonster; // 스폰할 몬스터 프리팹
    GameObject rangedMonster; // 스폰할 몬스터 프리팹
    private int maxMonstersPerSpawnArea; // 한 스폰 지역당 최대 몬스터 스폰 개수
    public float spawnDelay; // 몬스터 스폰 간격
    public float spawnRadius; // 몬스터 스폰 반경
    public int meleeMonsterSpawnNum; // 몬스터 스폰 개수
    public int rangedMonsterSpawnNum; // 몬스터 스폰 개수




    private void Awake()
    {
        maxMonstersPerSpawnArea = 3;
        spawnDelay = 2f;
        spawnRadius = 5f;
        meleeMonsterSpawnNum = 3;
        rangedMonsterSpawnNum = 3;
    }

    private void Start()
    {
        meleeMonster = ObjectPooler.SpawnFromPool("MeleeMonster", gameObject.transform.position);
        rangedMonster = ObjectPooler.SpawnFromPool("RangedMonster", gameObject.transform.position);
        InvokeRepeating("SpawnMonsters", spawnDelay, spawnDelay);
    }

    private void SpawnMonsters(EnumTypes.MonsterType monster)
    {
        // 랜덤한 스폰 지역 중에서 스폰할 지역을 선택
        Transform randomSpawnArea = transform.GetChild(Random.Range(0, transform.childCount));

        // 해당 스폰 지역에서 랜덤한 위치를 선택하여 몬스터를 스폰
        for (int i = 0; i < maxMonstersPerSpawnArea; i++)
        {
            Vector2 randomPosition = (Vector2)randomSpawnArea.position + Random.insideUnitCircle * spawnRadius;

            switch (monster)
            {
                case EnumTypes.MonsterType.MeleeMonster:
                    ObjectPooler.SpawnFromPool("MeleeMonster", randomPosition);
                    break;
                case EnumTypes.MonsterType.RangedMonster:
                    ObjectPooler.SpawnFromPool("RangedMonster", randomPosition);
                    break;
                default:
                    break;
            }
        }
    }
}
