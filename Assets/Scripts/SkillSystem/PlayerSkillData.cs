using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillData
{
    // � ��ų�� ��ȭ �������� �����ϱ� ���� ����
    public BaseSkillData baseSkillRef;

    // ��ȭ �� ���������� ���� ��
    public float finalDamage;
    public float finalCoefficient;
    public float finalCost;
    public float finalCooldown;
    public float finalCastTime;
    public float finalDuration;
    public float finalRange;

    // �߰��� ���� ȿ��(DoT, ���� ��) ���
    // ���� BaseSkillData�� �ִ� effects + ���� ���� effects�� ��ĥ ���� �ְ�,
    // ���������� �ϳ��� ����Ʈ�� ���� ������ ���� ����.
    public List<SkillEffect> finalEffects;

    // �ʿ��ϴٸ� ����ų �������̳� ����ȭ ��ޡ� � ���⿡
    public int skillLevel;
}
