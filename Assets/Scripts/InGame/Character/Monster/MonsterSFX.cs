using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSFX : MonoBehaviour
{
    [SerializeField] private GameObject inputMonsterHitVFX;
    [SerializeField] private GameObject inputMonsterAttackVFX;

    private GameObject monsterHitVFX;
    private GameObject monsterAttackVFX;
    private float lifeTime;

    private void Start()
    {
        InitSFX();
    }

    public void InitSFX()
    {
        inputMonsterAttackVFX = Resources.Load<GameObject>("Effect/Eff_MeleeMonster_Attack");
        inputMonsterHitVFX = Resources.Load<GameObject>("Effect/Eff_MeleeMonster_Hit");

        lifeTime = 2f;
    }

    public void MonsterStateSFX(Transform meleeMonsterTransform, EnumTypes.MonsterStateType state)
    {
        switch (state)
        {
            case EnumTypes.MonsterStateType.Attack:
                monsterAttackVFX = Instantiate(inputMonsterAttackVFX, meleeMonsterTransform);
                monsterAttackVFX.transform.SetParent(null);
                Destroy(monsterAttackVFX, lifeTime);
                break;
            case EnumTypes.MonsterStateType.Hit:
                monsterHitVFX = Instantiate(inputMonsterHitVFX, meleeMonsterTransform);
                monsterHitVFX.transform.SetParent(null);
                Destroy(monsterAttackVFX, lifeTime);
                break;
            case EnumTypes.MonsterStateType.Death:
                monsterAttackVFX = Instantiate(inputMonsterAttackVFX, meleeMonsterTransform);
                monsterAttackVFX.transform.SetParent(null);
                Destroy(monsterAttackVFX, lifeTime);
                break;
            default:
                break;
        }
    }
}
