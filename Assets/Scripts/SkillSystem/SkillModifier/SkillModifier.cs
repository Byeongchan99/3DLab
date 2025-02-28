using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Skill/SkillModifier")]
public class SkillModifier : ScriptableObject
{
    // ��: ��ȭ�� �߰� ����Ʈ(ȭ�� ��)
    public SkillEffect additionalEffect;

    // �ʿ��� �� �� �߰� ����

    public virtual void Apply(PlayerSkillData skillData)
    {
        // ȿ�� �߰�
        if (additionalEffect != null && !skillData.extraEffects.Contains(additionalEffect))
        {
            skillData.extraEffects.Add(additionalEffect);
        }
    }
}
