using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Skill/SkillEffect/DamageEffect")]
public class DamageEffect : SkillEffect
{
    public override void ApplyEffect(SkillContext context)
    {
        Debug.Log($"{context.caster.name}가 { context.target.name}에게 {context.damage}의 피해");
        // context.finalDamage 이용해 데미지 주기
        IDamageable dmg = context.target.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(context.damage, context.caster);
        }
    }
}
