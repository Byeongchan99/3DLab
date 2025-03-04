using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier/DamageModifier")]
public class DamageModifier : SkillModifier
{
    public float damageValue;

    public override void Apply(PlayerSkillData skillData)
    {
        skillData.finalDamage += damageValue;
        Debug.Log($"{skillData.baseSkillData.skillName}에 {name} 효과 적용");
    }
}
