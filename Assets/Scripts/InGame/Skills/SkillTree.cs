using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public ParentSkillNode parentSkill;
    public ChildrenSkillNode chilrenSkill1, childrenSkill2;

    private void Start()
    {
        DefinitionSkillNode();
    }

    private void DefinitionSkillNode()
    {
        ActiveSkills();
        PassiveSkills();
    }

    private void ActiveSkills()
    {
        parentSkill = new ParentSkillNode(EnumTypes.PlayerSkiils.DoubleShot, 10, 0);

        chilrenSkill1 = new ChildrenSkillNode(EnumTypes.PlayerSkiils.DoubleShot, 1, 5, parentSkill);
        parentSkill.children.Add(chilrenSkill1);
        childrenSkill2 = new ChildrenSkillNode(EnumTypes.PlayerSkiils.MultiShot, 1, 5, parentSkill);
        parentSkill.children.Add(childrenSkill2);
    }

    private void PassiveSkills()
    {

    }
}
