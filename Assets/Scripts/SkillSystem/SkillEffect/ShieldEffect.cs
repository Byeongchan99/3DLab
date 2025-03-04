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
        Debug.Log($"{context.target.name}���� {shieldAmount}�� ��ȣ���� {duration}�� ���� �����մϴ�.");

        ShieldComponent shieldComp = context.target.AddComponent<ShieldComponent>();
        Character targetChar = context.target.GetComponent<Character>();
        shieldComp.Init(shieldAmount, duration, targetChar);
        targetChar.AddShield(shieldComp);
    }
}
