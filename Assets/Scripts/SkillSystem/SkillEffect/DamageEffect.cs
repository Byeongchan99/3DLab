using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Skill/SkillEffect/DamageEffect")]
public class DamageEffect : SkillEffect
{
    public override void ApplyEffect(SkillContext context)
    {
        Debug.Log($"{context.caster.name}�� { context.target.name}���� {context.damage}�� ����");
        // context.finalDamage �̿��� ������ �ֱ�
        IDamageable dmg = context.target.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(context.damage, context.caster);
        }
    }
}
