using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, GameObject source);
}

public abstract class SkillEffect : ScriptableObject
{
    /// <summary>
    /// 효과를 적용할 때 호출되는 메서드
    /// </summary>
    /// <param name="caster">스킬 시전자</param>
    /// <param name="target">효과를 받을 대상</param>
    public abstract void ApplyEffect(SkillContext context);
}
