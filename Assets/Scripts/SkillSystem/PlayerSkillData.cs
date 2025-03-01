using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillData
{
    // � ��ų�� ��ȭ �������� �����ϱ� ���� ����
    public BaseSkillData baseSkillData;
    public GameObject skillPrefab; // ����ü ������

    // ��ȭ �� ���������� ���� ��
    public float finalDamage;
    public float finalCoefficient;
    public float finalCost;
    public float finalCooldown;
    public float finalCastTime;
    public float finalDuration;
    public float finalRange;

    // �ʿ��ϴٸ� ����ų �������̳� ����ȭ ��ޡ� � ���⿡
    public int skillLevel;

    // �߰��� ���� ȿ��(DoT, ���� ��) ���
    public List<SkillEffect> extraEffects;
    // ����
    public List<SkillEffect> finalEffects;

    // ������
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

        // ����Ʈ �ʱ�ȭ
        extraEffects = new List<SkillEffect>();
        finalEffects = new List<SkillEffect>();

        // extraEffects�� ��� ���� (��ȭ ��)
        for (int i = 0; i < baseSkillRef.effects.Count; i++)
        {
            finalEffects.Add(baseSkillRef.effects[i]);
        }
    }
}
