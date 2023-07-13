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

    // 추후 서버를 통해 받은 캐릭터명 혹은 몬스터명
    public string CharacterName;
    public int hp;
    public int mp;
    public int lv;


    protected virtual void Start()
    {
        // 자식에 의에 호출 됐을떄 초기화 작업
    }

    // player, monster 별로 개별 구현
    public abstract void Attack();

    public abstract void Move();


    // player, monster 별로 개별 구현
    protected abstract void Die();
}
