using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonsterBase
{
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Attack()
    {
        transform.Rotate(0, 0, 30);

    }

    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }
}
