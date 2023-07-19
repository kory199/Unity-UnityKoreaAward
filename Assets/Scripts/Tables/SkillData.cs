using System.Collections.Generic;
using static EnumTypes;

[System.Serializable]
public class SkillData
{
    public int Id;
    public string Description;
    public string Name;
    public PlayerSkiilsType PlayerSkiilsType;
    public int UnlockLevel;
    public static Dictionary<int, SkillData> table = new Dictionary<int, SkillData> ();   
}