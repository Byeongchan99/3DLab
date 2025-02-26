using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    // 스킬을 실행할 때
    public void ExecuteSkill(GameObject caster, GameObject target, PlayerSkillData skillData)
    {
        // 1) SkillUsageContext 준비
        SkillContext context = new SkillContext()
        {
            caster = caster,
            target = target,
            finalDamage = skillData.finalDamage,
            finalCost = skillData.finalCost,
            finalCooldown = skillData.finalCooldown
        };

        // 2) 각 이펙트 적용
        foreach (var effect in skillData.finalEffects)
        {
            effect.ApplyEffect(context);
        }
    }
}
