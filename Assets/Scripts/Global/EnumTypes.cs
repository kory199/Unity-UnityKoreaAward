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

    public enum ScenesType
    { 
        SceneTitle,
        SceneLobby,
        SceneInGame
    }

    public enum StageStateType
    {
        Start, //시작
        Next, //다음
        End, //끝 => 보스맵
        Max
    }

    public enum MonsterStateType
    {
        None,   // 데이터 초기화 등의 작업
        Move,   // 플레이어추적 or 무작위 이동
        Attack, // 공격
        Death,  // 죽음
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