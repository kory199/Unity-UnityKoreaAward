using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSFX : MonoBehaviour
{
    [SerializeField] private GameObject monsterHitVFX;
    [SerializeField] private GameObject monsterAttackVFX;

    WaitForSeconds waitForSeconds;

    private void Awake()
    {
        InitAttackSFX();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void InitAttackSFX()
    {
        monsterAttackVFX = ObjectPooler.SpawnFromPool("Eff_MeleeMonster_Attack", gameObject.transform.position);

        waitForSeconds = new WaitForSeconds(2f);
    }

    public void AttackSFX()
    {
        monsterAttackVFX.SetActive(true);
        StartCoroutine(DisappearMonsterSFX());
    }

    private IEnumerator DisappearMonsterSFX()
    {
        yield return waitForSeconds;

        gameObject.SetActive(false);
    }
}
