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
    /// ȿ���� ������ �� ȣ��Ǵ� �޼���
    /// </summary>
    public abstract void ApplyEffect(SkillContext context);
}
