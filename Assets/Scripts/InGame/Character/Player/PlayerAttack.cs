using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public override void Attack()
    {
       /* if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject bullet = ObjectPooler.SpawnFromPool("Bullet", gameObject.transform.position);
        }*/
        if (Input.GetMouseButtonDown(0)) // A키가 Move와 겹쳐서 마우스 좌클릭으로 테스트 진행해봤어용
        {
            GameObject bullet = ObjectPooler.SpawnFromPool("Bullet", gameObject.transform.position);
        }
    }
}
