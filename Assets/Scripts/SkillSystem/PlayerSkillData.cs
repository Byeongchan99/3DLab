using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillData
{
    // 어떤 스킬의 강화 버전인지 구분하기 위해 참조
    public BaseSkillData baseSkillRef;

    // 강화 후 최종적으로 계산된 값
    public float finalDamage;
    public float finalCoefficient;
    public float finalCost;
    public float finalCooldown;
    public float finalCastTime;
    public float finalDuration;
    public float finalRange;

    // 추가로 붙은 효과(DoT, 버프 등) 목록
    // 만약 BaseSkillData에 있는 effects + 새로 붙은 effects를 합칠 수도 있고,
    // 최종적으로 하나의 리스트에 통합 저장할 수도 있음.
    public List<SkillEffect> finalEffects;

    // 필요하다면 “스킬 레벨”이나 “강화 등급” 등도 여기에
    public int skillLevel;
}
