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
    /// ȿ���� ������ �� ȣ��Ǵ� �޼���
    /// </summary>
    /// <param name="caster">��ų ������</param>
    /// <param name="target">ȿ���� ���� ���</param>
    public abstract void ApplyEffect(SkillContext context);
}
