using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 사용 종류
public enum SkillCastType
{
    None,
    // 1. 액티브
    Instant, // 즉시
    Channeling, // 채널링
    Toggle, // 토글
    // 2. 패시브
    Always, // 항상
    Trigger, // 트리거
}

// 스킬 적용 대상
public enum SkillTargetType
{
    None,
    Self, // 자신
    Enemy, // 적
    Ally, // 아군
    All, // 모두
}

// 스킬 타겟팅 방식
public enum SkillTargetingType
{
    None,
    Single, // 단일
    Multiple, // 다중
}

// 스킬 적중 판정
public enum SkillHitType
{
    None,
    // 1. 타겟팅
    InstantTargeting, // 즉시
    ProjectileTargeting, // 투사체
    AreaTargeting, // 범위
    // 2. 논타겟팅
    ProjectileNonTargeting, // 투사체
    AreaNonTargeting, // 범위
}

[CreateAssetMenu(fileName = "BaseSkillData", menuName = "Skill/BaseSkillData")]
public class BaseSkillData : ScriptableObject
{
    [Header("속성 / 베이스 스탯")]
    public string skillName;
    public float baseDamage; // 기본 대미지
    public float damageCoefficient; // 대미지 계수
    public float cost; // 비용
    public float cooldown; // 재사용 대기시간
    public float castTime; // 시전시간
    public float duration; // 지속시간
    public float range; // 사정거리

    [Header("스킬 종류 정보")]
    public SkillCastType castType;
    public SkillTargetType targetType;
    public SkillTargetingType targetingType;
    public SkillHitType hitType;

    [Header("효과 목록")]
    public List<SkillEffect> effects;  // 여러 효과를 등록

    public GameObject skillPrefab; // 스킬 프리팹
}
