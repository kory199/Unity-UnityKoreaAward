using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player
{
    public override void Attack()
    {
        GameObject pullBullet = ObjectPooler.SpawnFromPool("Bullet2D", gameObject.transform.position);

        // Bullet에 발사 정보 전달
        if (pullBullet.TryGetComponent<Bullet>(out Bullet bull))
        {
            bullet = bull;
        }
        else
        {
            bullet = pullBullet.AddComponent<Bullet>();
        }

        bullet.SetShooter(gameObject.name);

        if (pullBullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            bulletRb = rb;
        }
        else
        {
            bulletRb = pullBullet.AddComponent<Rigidbody2D>();
        }

        // 속도 가중치는 서버 데이터 업로드 후 변경
        bulletRb.velocity = targetDirection * playerProjectileSpeed;
    }
    public void PlayerHit(float damageAmount)
    {
        playerCurHp -= damageAmount;
        InitPlayerUI();

        if (playerCurHp <= 0)
        {
            IsDeath = true;
            Die();
        }
    }

    public void Reward(int exp, int score)
    {
        playerCurExp += exp;
        GameManager.Instance.playerData.score += score;
        InitPlayerUI();

        if (playerCurExp >= playerMaxExp)
        {
            LevelyUp();
            playerCurExp = 0;
        }
    }

    protected override void Die()
    {
        Debug.LogError("Player Die");
        StageManager.Instance.PlayerDeath();
    }

    public void LevelyUp()
    {
        // 스크립터블 오브젝트로 부터 새로운 정보를 받아와 player setting
        // InitPlayer();

        playerLv++;
        if (playerLv > 50)
        {
            return;
        }
        retrunPlayerInfo(playerLv);
        InitPlayerUI();
    }

    private IEnumerator MonveAble()
    {
        isMoveable = false;
        yield return moveAble;
        isMoveable = true;
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" && isMoveable)
        {
            gameObject.transform.position *= -1;
            StartCoroutine("MonveAble");
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && isMoveable)
        {
            gameObject.transform.position *= -1;
            StartCoroutine("MonveAble");
        }
    }
}
