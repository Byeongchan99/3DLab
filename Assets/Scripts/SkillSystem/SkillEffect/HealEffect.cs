using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Skill/SkillEffect/HealEffect")]
public class HealEffect : SkillEffect
{
    public float healAmount = 100f;

    public override void ApplyEffect(SkillContext context)
    {
        Debug.Log($"{context.target.name}���� {healAmount}��ŭ�� ü���� ȸ����ŵ�ϴ�.");       
        Character targetChar = context.target.GetComponent<Character>();      
        targetChar.Heal(healAmount);
    }
}
