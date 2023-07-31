using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public override void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           // GameObject bullet = ObjectPooler.SpawnFromPool("Bullet", gameObject.transform.position);
        }
    }
    public void PlayerHit(int damageAmount)
    {
        playerCurHp -= damageAmount;

        Debug.Log($"Player Hit Cur HP : {playerCurHp}");

        if (playerCurHp <= 0)
        {
            Debug.Log("Player Die");
            Die();
        }
    }
}
