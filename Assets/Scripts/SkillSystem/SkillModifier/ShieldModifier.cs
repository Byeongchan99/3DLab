using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldModifier", menuName = "Skill/SkillModifier/ShieldModifier")]
public class ShieldModifier : SkillModifier
{
    public override void Apply(PlayerSkillData skillData)
    {
        Debug.Log($"{skillData.baseSkillData.skillName}�� {name} ȿ�� ����");
        base.Apply(skillData);
    }
}
