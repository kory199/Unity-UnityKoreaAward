using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private Dictionary<string, MonoBehaviour> playerSkills = new();

    public void AddPlayerSkills(SkillInfo skillInfo)
    {
        playerSkills.Add(skillInfo.SkillClassName, LoadScriptInstance(skillInfo));
    }

    public MonoBehaviour LoadScriptInstance(SkillInfo skillInfo)
    {
        Type scriptType = Type.GetType(skillInfo.SkillClassName);

        if (scriptType != null)
        {
            MonoBehaviour scriptInstance = gameObject.AddComponent(scriptType) as MonoBehaviour;
            return scriptInstance;
        }
        else
        {
            Debug.LogError(skillInfo.SkillClassName + "Script type not found");
            return null;
        }
    }


    // Å° ¸ÊÇÎ 1~5
}
