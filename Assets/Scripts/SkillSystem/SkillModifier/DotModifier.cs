using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier/DotModifier")]
public class DotModifier : SkillModifier
{
    public override void Apply(PlayerSkillData skillData)
    {
        Debug.Log($"{skillData.baseSkillData.skillName}�� {name} ȿ�� ����");
        base.Apply(skillData);
    }
}
