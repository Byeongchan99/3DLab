using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "SkillEffect/DamageEffect")]
public class DamageEffect : SkillEffect
{
    public override void ApplyEffect(SkillContext context)
    {
        // context.finalDamage 이용해 데미지 주기
        IDamageable dmg = context.target.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(context.finalDamage, context.caster);
        }
    }
}
