using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : MonsterBase
{
    [SerializeField] protected GameObject _boss;
    [SerializeField] protected Vector3 _initMoveVector;
    [SerializeField] protected List<Transform> _spawners;

    /// <summary>
    /// ����ü �߻��ϴ� �Լ�
    /// </summary>
    protected void ShootProjectile(string name)
    {
        //�߻� ���� ���
        //_initMoveVector = gameObject.transform.position - _boss.transform.position;
        foreach(var dir in _spawners)
        {
            //Ǯ���� ����ü ������
            GameObject projectiles = ObjectPooler.SpawnFromPool(name, dir.transform.position);

            //�߻� �ڷ�ƾ
            StartCoroutine(Co_ProjectileMove(projectiles, dir.position - _boss.transform.position));
        }


     /*   //Ǯ���� ����ü ������
        GameObject ball = ObjectPooler.SpawnFromPool(projectile, gameObject.transform.position);
        
        //�߻� �ڷ�ƾ
        StartCoroutine(Co_BulletMove(ball, _initMoveVector));*/
    }
    /// <summary>
    /// ����ü�� �������� ������ ���� , ���� 1�ܰ� ����
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
