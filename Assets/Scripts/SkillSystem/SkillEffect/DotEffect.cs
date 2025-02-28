using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DotEffect", menuName = "Skill/SkillEffect/DotEffect")]
public class DotEffect : SkillEffect
{
    // 1�ʸ��� �� �����, ��Ʈ ���� �ð� ���� �ν����Ϳ��� ���� ����
    public float damagePerSec = 10f;
    public float duration = 5f;

    public override void ApplyEffect(SkillContext context)
    {
        // Ÿ�ٿ��� DotComponent ���̱�
        Debug.Log($"{context.caster.name}�� { context.target.name}���� {damagePerSec}�� ���ظ� {duration}�� ���� �ݴϴ�.");
        DotComponent dotComp = context.target.AddComponent<DotComponent>();
        dotComp.Init(damagePerSec, duration, context.caster);
    }
}
