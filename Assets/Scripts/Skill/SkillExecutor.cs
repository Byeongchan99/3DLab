using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    // ��ų�� ������ ��
    public void ExecuteSkill(GameObject caster, GameObject target, PlayerSkillData skillData)
    {
        // 1) SkillUsageContext �غ�
        SkillContext context = new SkillContext()
        {
            caster = caster,
            target = target,
            finalDamage = skillData.finalDamage,
            finalCost = skillData.finalCost,
            finalCooldown = skillData.finalCooldown
        };

        // 2) �� ����Ʈ ����
        foreach (var effect in skillData.finalEffects)
        {
            effect.ApplyEffect(context);
        }
    }
}
