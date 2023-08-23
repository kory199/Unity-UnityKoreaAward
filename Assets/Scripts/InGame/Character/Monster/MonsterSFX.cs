using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSFX : MonoBehaviour
{
    [SerializeField] private GameObject monsterHitVFX;
    [SerializeField] private GameObject monsterAttackVFX;

    private void Awake()
    {

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(monsterAttackVFX);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackSFX()
    {
        monsterAttackVFX = ObjectPooler.SpawnFromPool("Eff_MeleeMonster_Attack", gameObject.transform.position);
    }
    public void DisappearMonsterSFX()
    {

    }
}
