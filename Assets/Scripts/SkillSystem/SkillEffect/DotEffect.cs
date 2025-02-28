using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DotEffect", menuName = "Skill/SkillEffect/DotEffect")]
public class DotEffect : SkillEffect
{
    // 1초마다 줄 대미지, 도트 유지 시간 등을 인스펙터에서 조정 가능
    public float damagePerSec = 10f;
    public float duration = 5f;

    public override void ApplyEffect(SkillContext context)
    {
        // 타겟에게 DotComponent 붙이기
        Debug.Log($"{context.caster.name}가 { context.target.name}에게 {damagePerSec}의 피해를 {duration}초 동안 줍니다.");
        DotComponent dotComp = context.target.AddComponent<DotComponent>();
        dotComp.Init(damagePerSec, duration, context.caster);
    }
}
