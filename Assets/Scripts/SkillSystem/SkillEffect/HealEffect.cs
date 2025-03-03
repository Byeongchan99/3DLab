using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Skill/SkillEffect/HealEffect")]
public class HealEffect : SkillEffect
{
    public float healAmount = 100f;

    public override void ApplyEffect(SkillContext context)
    {
        Debug.Log($"{context.target.name}에게 {healAmount}만큼의 체력을 회복시킵니다.");       
        Character targetChar = context.target.GetComponent<Character>();      
        targetChar.Heal(healAmount);
    }
}
