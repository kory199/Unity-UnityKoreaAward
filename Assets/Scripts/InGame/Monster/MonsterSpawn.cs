using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] GameObject monsterPrefab; // 스폰할 몬스터 프리팹
    [SerializeField] private int maxMonstersPerSpawnArea = 3; // 한 스폰 지역당 최대 몬스터 스폰 개수
    [SerializeField] private float spawnDelay = 2f; // 몬스터 스폰 간격
    [SerializeField] private float spawnRadius = 5f; // 몬스터 스폰 반경

    GameObject meleeMonster;
    GameObject rangedMonster;
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
            Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
        }
    }
}
