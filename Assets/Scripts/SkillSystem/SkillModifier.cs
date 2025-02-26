using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModifier : MonoBehaviour
{
    public void ApplyHellFireballUpgrade(PlayerSkillData skillData, SkillEffect skillEffect)
    {
        // 1) ����� ����
        skillData.finalDamage += 50f;

        // 2) DoT ȿ�� �߰�
        if (!skillData.finalEffects.Contains(skillEffect))
        {
            skillData.finalEffects.Add(skillEffect);
        }
    }
}
