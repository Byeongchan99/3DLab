using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    // ��ų�� ������ ��
    public void ExecuteSkill(GameObject caster, GameObject target, PlayerSkillData skillData)
    {
        Debug.Log($"��ų ���: {skillData.baseSkillData.skillName}");

        // 1) SkillUsageContext �غ�
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
        // 2) �� ����Ʈ ����
        foreach (var effect in skillData.finalEffects)
        {
            effect.ApplyEffect(context);
        }
        */
    }
}
