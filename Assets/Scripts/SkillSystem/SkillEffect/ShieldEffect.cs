using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Skill/SkillEffect/ShieldEffect")]
public class ShieldEffect : SkillEffect
{
    public float shieldAmount = 50f;
    
    public override void ApplyEffect(SkillContext context)
    {
        shieldAmount = context.damage;
        duration = context.duration;
        Debug.Log($"{context.target.name}에게 {shieldAmount}의 보호막을 {duration}초 동안 적용합니다.");

        ShieldComponent shieldComp = context.target.AddComponent<ShieldComponent>();
        Character targetChar = context.target.GetComponent<Character>();
        shieldComp.Init(shieldAmount, duration, targetChar);
        targetChar.AddShield(shieldComp);
    }
}
