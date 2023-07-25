using System.Collections.Generic;


[System.Serializable]
public class PassivSkills
{
    public int Id;
    public string Description;
    public string PassiveSkillType;
    public int UnlockLevel;
    public float Value;
    public int LimitUpgrade;
    public float Duration;
    public float TimeInterval;
    public static Dictionary<int, PassivSkills> table = new Dictionary<int, PassivSkills> ();   
}