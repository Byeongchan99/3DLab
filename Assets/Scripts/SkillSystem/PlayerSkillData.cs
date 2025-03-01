using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillData
{
    // 어떤 스킬의 강화 버전인지 구분하기 위해 참조
    public BaseSkillData baseSkillData;
    public GameObject skillPrefab; // 투사체 프리팹

    // 강화 후 최종적으로 계산된 값
    public float finalDamage;
    public float finalCoefficient;
    public float finalCost;
    public float finalCooldown;
    public float finalCastTime;
    public float finalDuration;
    public float finalRange;

    // 필요하다면 “스킬 레벨”이나 “강화 등급” 등도 여기에
    public int skillLevel;

    // 추가로 붙은 효과(DoT, 버프 등) 목록
    public List<SkillEffect> extraEffects;
    // 최종
    public List<SkillEffect> finalEffects;

    // 생성자
    public PlayerSkillData(BaseSkillData baseSkillRef)
    {
        this.baseSkillData = baseSkillRef;
        this.skillPrefab = baseSkillRef.skillPrefab;

        finalDamage = baseSkillRef.baseDamage;
        finalCoefficient = baseSkillRef.damageCoefficient;
        finalCost = baseSkillRef.cost;
        finalCooldown = baseSkillRef.cooldown;
        finalCastTime = baseSkillRef.castTime;
        finalDuration = baseSkillRef.duration;
        finalRange = baseSkillRef.range;
        skillLevel = 1;

        // 리스트 초기화
        extraEffects = new List<SkillEffect>();
        finalEffects = new List<SkillEffect>();

        // extraEffects는 비어 있음 (강화 전)
        for (int i = 0; i < baseSkillRef.effects.Count; i++)
        {
            finalEffects.Add(baseSkillRef.effects[i]);
        }
    }
}
