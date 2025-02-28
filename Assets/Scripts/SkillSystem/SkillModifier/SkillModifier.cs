using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier")]
public class SkillModifier : ScriptableObject
{
    // ��: ��ȭ�� �������
    public float damageUpValue;
    // ��: ��ȭ�� �߰� ����Ʈ(ȭ�� ��)
    public SkillEffect additionalEffect;

    // �ʿ��� �� �� �߰� ����

    public virtual void Apply(PlayerSkillData skillData)
    {
        // 1) ����� ����
        skillData.finalDamage += damageUpValue;

        // 2) ȿ�� �߰�
        if (additionalEffect != null && !skillData.extraEffects.Contains(additionalEffect))
        {
            skillData.extraEffects.Add(additionalEffect);
        }
    }
}
