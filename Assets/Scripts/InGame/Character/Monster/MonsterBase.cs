using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIModels;

public class Monster : CharacterBase
{
    float monsterSpeed;
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
    }

    public override void Move()
    {
    }

    public override void Attack()
    {
        // ¸ó½ºÅÍ °ø°Ý ±¸Çö
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}