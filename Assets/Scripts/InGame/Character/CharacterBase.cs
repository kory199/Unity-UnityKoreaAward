using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInfo
{
    public string CharacterName;
    public int NowHP;
    public int MaxHp;
    public int NowMP;
    public int MaxMp;
    public int Lv;
}

public abstract class CharacterBase : MonoBehaviour
{
    private PlayerInfo _playerInfo;

    // ���� ������ ���� ���� ĳ���͸� Ȥ�� ���͸�
    protected string CharacterName;
    protected int hp;
    protected int mp;
    protected int lv;
    protected float moveSpeed;


    protected virtual void Start()
    {
        // �ڽĿ� �ǿ� ȣ�� ������ �ʱ�ȭ �۾�
    }

    protected virtual void OnDisable()
    {

    }

    // player, monster ���� ���� ����
    public abstract void Attack();

    public abstract void Move();

    public void Udate()
    {


    }

    // player, monster ���� ���� ����
    protected abstract void Die();
}
