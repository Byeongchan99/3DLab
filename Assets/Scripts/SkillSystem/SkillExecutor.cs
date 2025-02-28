using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    // 스킬을 실행할 때
    public void ExecuteSkill(GameObject caster, GameObject target, PlayerSkillData skillData)
    {
        Debug.Log($"스킬 사용: {skillData.baseSkillData.skillName}");

        // 1) SkillUsageContext 준비
        SkillContext context = new SkillContext()
        {
            caster = caster,
            target = target,
            damage = skillData.finalDamage,
            damageCoefficient = skillData.finalCoefficient,
            cost = skillData.finalCost,
            cooldown = skillData.finalCooldown,
            castTime = skillData.finalCastTime,
            duration = skillData.finalDuration,
            range = skillData.finalRange
        };

        /*
        // 2) 각 이펙트 적용
        foreach (var effect in skillData.finalEffects)
        {
            effect.ApplyEffect(context);
        }
        */
    }
}
