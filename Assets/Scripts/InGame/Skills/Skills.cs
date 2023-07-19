using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public string skillName;
    public int curSkillLevel;
    public int maxSkillLevel;
    public int requiredUnlockSkillLevel;

    public Skills(EnumTypes.PlayerSkiils skillName, int maxLv, int reqLv)
    {
        this.skillName = skillName.ToString();
        curSkillLevel = 0;
        maxSkillLevel = maxLv;
        requiredUnlockSkillLevel = reqLv;
    }

    // 스킬 해방 여부 반환
    public virtual bool CanSkillOpen()
    {
        return (curSkillLevel <= maxSkillLevel) && curSkillLevel > 0;
    }

    public void LevelUp()
    {
        if (CanSkillOpen())
            curSkillLevel++;
    }
}
