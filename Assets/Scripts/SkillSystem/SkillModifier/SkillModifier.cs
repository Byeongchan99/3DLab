using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier")]
public class SkillModifier : ScriptableObject
{
    // 예: 강화할 대미지량
    public float damageUpValue;
    // 예: 강화할 추가 이펙트(화상 등)
    public SkillEffect additionalEffect;

    // 필요한 것 더 추가 가능

    public virtual void Apply(PlayerSkillData skillData)
    {
        // 1) 대미지 증가
        skillData.finalDamage += damageUpValue;

        // 2) 효과 추가
        if (additionalEffect != null && !skillData.extraEffects.Contains(additionalEffect))
        {
            skillData.extraEffects.Add(additionalEffect);
        }
    }
}
