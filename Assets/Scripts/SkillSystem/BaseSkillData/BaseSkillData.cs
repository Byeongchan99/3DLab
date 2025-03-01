using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ų ��� ����
public enum SkillCastType
{
    None,
    // 1. ��Ƽ��
    Instant, // ���
    Channeling, // ä�θ�
    Toggle, // ���
    // 2. �нú�
    Always, // �׻�
    Trigger, // Ʈ����
}

// ��ų ���� ���
public enum SkillTargetType
{
    None,
    Self, // �ڽ�
    Enemy, // ��
    Ally, // �Ʊ�
    All, // ���
}

// ��ų Ÿ���� ���
public enum SkillTargetingType
{
    None,
    Single, // ����
    Multiple, // ����
}

// ��ų ���� ����
public enum SkillHitType
{
    None,
    // 1. Ÿ����
    InstantTargeting, // ���
    ProjectileTargeting, // ����ü
    AreaTargeting, // ����
    // 2. ��Ÿ����
    ProjectileNonTargeting, // ����ü
    AreaNonTargeting, // ����
}

[CreateAssetMenu(fileName = "BaseSkillData", menuName = "Skill/BaseSkillData")]
public class BaseSkillData : ScriptableObject
{
    [Header("�Ӽ� / ���̽� ����")]
    public string skillName;
    public float baseDamage; // �⺻ �����
    public float damageCoefficient; // ����� ���
    public float cost; // ���
    public float cooldown; // ���� ���ð�
    public float castTime; // �����ð�
    public float duration; // ���ӽð�
    public float range; // �����Ÿ�

    [Header("��ų ���� ����")]
    public SkillCastType castType;
    public SkillTargetType targetType;
    public SkillTargetingType targetingType;
    public SkillHitType hitType;

    [Header("ȿ�� ���")]
    public List<SkillEffect> effects;  // ���� ȿ���� ���

    public GameObject skillPrefab; // ��ų ������
}
