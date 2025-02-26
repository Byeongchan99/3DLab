using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModifier : MonoBehaviour
{
    public void ApplyHellFireballUpgrade(PlayerSkillData skillData, SkillEffect skillEffect)
    {
        // 1) 대미지 증가
        skillData.finalDamage += 50f;

        // 2) DoT 효과 추가
        if (!skillData.finalEffects.Contains(skillEffect))
        {
            skillData.finalEffects.Add(skillEffect);
        }
    }
}
