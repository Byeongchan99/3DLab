using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "SkillEffect/DamageEffect")]
public class DamageEffect : SkillEffect
{
    public override void ApplyEffect(SkillContext context)
    {
        Debug.Log(context.caster + "가 " + context.target + "에게 " + context.damage);
        // context.finalDamage 이용해 데미지 주기
        IDamageable dmg = context.target.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(context.damage, context.caster);
        }
    }
}
