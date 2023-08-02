using System;

public static partial class EnumTypes
{
    public enum LanguageType // ���̺�
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
        MAX //������ Enum => Const����
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
        Start, //����
        Next, //����
        End, //�� => ������
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
        None,   // ������ �ʱ�ȭ ���� �۾�
        Move,   // �÷��̾����� or ������ �̵�
        Attack, // ����
        Death,  // ����
        Dance,  // �÷��̾� �׾����� ŷ�ް� �ϱ�� State
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