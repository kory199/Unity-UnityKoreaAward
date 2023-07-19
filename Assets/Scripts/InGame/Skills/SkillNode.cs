using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ParentSkillNode : Skills
{
    public List<ChildrenSkillNode> children;

    // 입력받은 매개 변수를 기준으로 부모 클래스의 생성자 초기화
    public ParentSkillNode(EnumTypes.PlayerSkiils skiils, int maxLv, int reqLv) : base(skiils, maxLv, reqLv)
    {
        children = new List<ChildrenSkillNode>();
    }
}

public partial class ChildrenSkillNode : Skills
{
    public ParentSkillNode parent;

    // 부모 클래스 생성자 초기화 및 ParentSkillNode 연결
    public ChildrenSkillNode(EnumTypes.PlayerSkiils skiils, int maxLv, int reqLv, ParentSkillNode parentNode) : base(skiils, maxLv, reqLv)
    {
        parent = parentNode;
    }

    // 스킬 해금 조건 판단
    public override bool CanSkillOpen()
    {
        // 부모 노드에서 정의된 해금조건 (ex 각 스킬의 부모 노드에 정의된 해금 레벨 조건)
        return parent.curSkillLevel >= requiredUnlockSkillLevel && base.CanSkillOpen();
    }
}