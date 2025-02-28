using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier")]
public class SkillModifier : ScriptableObject
{
    // 강화할 추가 이펙트(화상 등)
    public SkillEffect additionalEffect;

    public virtual void Apply(PlayerSkillData skillData)
    {
        // 효과 추가
        Debug.Log($"적용할 효과: {additionalEffect.name}");
        if (additionalEffect != null && !skillData.extraEffects.Contains(additionalEffect))
        {
            skillData.extraEffects.Add(additionalEffect);
            skillData.finalEffects.Add(additionalEffect);
        }
    }
}
