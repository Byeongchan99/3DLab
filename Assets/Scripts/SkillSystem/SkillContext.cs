using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillContext
{
    public GameObject caster;
    public GameObject target;

    public string skillName; // ��ų �̸�
    public int skillID; // ��ų ID

    public float damage; // �⺻ �����
    public float damageCoefficient; // ����� ���
    public float cost; // ���
    public float cooldown; // ���� ���ð�
    public float castTime; // �����ð�
    public float duration; // ���ӽð�
    public float range; // �����Ÿ�
}
