using System.Collections;
using UnityEngine;

public partial class Player
{
    public override void Attack()
    {
        GameObject pullBullet = ObjectPooler.SpawnFromPool("Bullet2D", gameObject.transform.position);
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.PlayerAttack);

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

        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.PlayerHit);

        if (playerCurHp <= 0)
        {
            IsDeath = true;
            Die();
        }
    }

    private void UIControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // uI_SceneGame.OnShow();
            uI_Enhance.OnClick_ReturnGame();
        }
    }

    public void Reward(int exp, int score)
    {
        playerCurExp += exp;
        // GameManager.Instance.playerData.score += score;
        playerScore += score;
        InitPlayerUI();
        uI_SceneGame.SetScore(playerScore);
        GameManager.Instance.score = playerScore;

        if (playerCurExp >= playerMaxExp)
        {
            LevelyUp();
            playerCurExp = 0;
        }
    }

    protected override void Die()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.PlayerDeath);
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

       // uI_SceneGame.OnHide();
        // uI_Enhance.GetSkillPoint(1);
        // uI_Enhance.OnShow();
        // Time.timeScale = 0;
    }

    private IEnumerator MonveAble()
    {
        isMoveable = false;
        yield return moveAble;
        isMoveable = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && isMoveable)
        {
            gameObject.transform.position *= -1;
            StartCoroutine("MonveAble");
        }
    }
}
