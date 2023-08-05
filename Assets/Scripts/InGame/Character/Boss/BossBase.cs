using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : MonsterBase
{
    [SerializeField] protected GameObject _boss;
    [SerializeField] protected Vector3 _initMoveVector;
    [SerializeField] protected List<Transform> _spawners;

    /// <summary>
    /// 투사체 발사하는 함수
    /// </summary>
    protected void ShootProjectile(string name)
    {
        //발사 방향 계산
        //_initMoveVector = gameObject.transform.position - _boss.transform.position;
        foreach(var dir in _spawners)
        {
            //풀에서 투사체 꺼내기
            GameObject projectiles = ObjectPooler.SpawnFromPool(name, dir.transform.position);

            //발사 코루틴
            StartCoroutine(Co_ProjectileMove(projectiles, dir.position - _boss.transform.position));
        }


     /*   //풀에서 투사체 꺼내기
        GameObject ball = ObjectPooler.SpawnFromPool(projectile, gameObject.transform.position);
        
        //발사 코루틴
        StartCoroutine(Co_BulletMove(ball, _initMoveVector));*/
    }
    /// <summary>
    /// 투사체를 직선으로 날리는 로직 , 보스 1단계 패턴
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    IEnumerator Co_ProjectileMove(GameObject ball, Vector3 dir)
    {
        int i = 0;
        while (i < Utils.PROJECTILE_MAXDISTANCE)
        {
            yield return null;
            ball.transform.Translate(dir.normalized * Utils.PROJECTILE_SPEED); 
            i++;
        }
        ball.gameObject.SetActive(false);
    }
}
