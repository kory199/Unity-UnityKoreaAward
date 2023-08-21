using System;

public static partial class EnumTypes
{
    public enum LanguageType // 테이블
    {
        Kor = 0,
        Eng = 1
    }
    public enum LayoutType
    {
        First,
        Middle,
        Global,  // popup
    }

    public enum InGameParamType
    {
        Player,
        Monster,
        Stage,
        MAX
    }

    public enum PlayerStateType
    {
        Death,
        LevelUp,
        MAX //마지막 Enum => Const느낌
    }
    public enum PlayerSkiilsType
    {
        DoubleShot,
        TripleShot,
        MultiShot,
        MAX
    }

    public enum EffectSoundType
    {
        None,
        Attack,
        AttackSkill,
        Defence,
        DefenceSkill,
        Hit,
        Die,
        Button,
    }

    public enum StageBGMType
    {
        Title,
        Lobby,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
    }

    public enum BossBGMType
    {
        Stage1,
        Stage2,
        Stage3,
    }

    public enum SFXType
    {
        PlayerAttack,
        PlayerDeath,
        PlayerHit,
        MonsterHit,
        MonsterDeath,
        Skills,
        StageChange,
        StageClear,
        //StageFile,
        Button,
        EseKey,
        ResultWindow,
        Typing,
        QuitGame,
    }

    public enum EffectType
    {
        Button,
        Bullet,
    }

    public enum ScenesType
    { 
        SceneTitle,
        SceneLobby,
        SceneInGame
    }

    public enum StageStateType
    {
        Awake,
        Start, //시작
        Next, //다음
        End, //끝 => 보스맵
        Max
    }

    public enum MonsterType
    {
        MeleeMonster,
        RangedMonster
    }

    public enum FSMMonsterStateType
    {
        Idle,
        Chasing,
        Attacking
    }

    public enum MonsterStateType
    {
        None,   // 데이터 초기화 등의 작업
        Move,   // 플레이어추적 or 무작위 이동
        Attack, // 공격
        Death,  // 죽음
        Dance,  // 플레이어 죽었을때 킹받게 하기용 State
        Phase1,
        Phase2,
        Phase3,
        Max     
    }
}
public static partial class EnumTypes
{
    public static int GetEnumNumber<TEnum>(TEnum tenum)
    {
        Type enumType = typeof(TEnum);
        int idx = -1;
        foreach (var type in Enum.GetValues(enumType))
        {
            idx++;
            if (type.ToString() == tenum.ToString()) break;
        }
        return idx;
    }
}