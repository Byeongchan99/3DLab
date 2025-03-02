using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldModifier", menuName = "Skill/SkillModifier/ShieldModifier")]
public class ShieldModifier : SkillModifier
{
    public override void Apply(PlayerSkillData skillData)
    {
        Debug.Log($"{skillData.baseSkillData.skillName}에 {name} 효과 적용");
        base.Apply(skillData);
    }
}
