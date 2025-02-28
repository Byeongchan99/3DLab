using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier/DotModifier")]
public class DotModifier : SkillModifier
{
    public override void Apply(PlayerSkillData skillData)
    {
        Debug.Log($"{skillData.baseSkillData.skillName}에 {name} 효과 적용");
        base.Apply(skillData);
    }
}
