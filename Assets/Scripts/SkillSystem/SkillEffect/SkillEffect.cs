using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, GameObject source);
}

public abstract class SkillEffect : ScriptableObject
{
    public float duration = 0f;
    /// <summary>
    /// 효과를 적용할 때 호출되는 메서드
    /// </summary>
    public abstract void ApplyEffect(SkillContext context);
}
