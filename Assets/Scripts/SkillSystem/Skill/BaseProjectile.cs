using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private GameObject caster;
    private PlayerSkillData skillData;

    public float speed = 10f;

    public void Init(GameObject caster, PlayerSkillData data)
    {
        this.caster = caster;
        this.skillData = data;
    }

    void Update()
    {
        // �������� �̵�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"����ü �浹: {other.gameObject.name}");

        // caster �ڽ��̸� ����
        if (other.gameObject == caster) return;

        // ���� ȿ�� ����
        SkillContext context = new SkillContext()
        {
            caster = caster,
            target = other.gameObject,
            damage = skillData.finalDamage,
            damageCoefficient = skillData.finalCoefficient,
            cost = skillData.finalCost,
            cooldown = skillData.finalCooldown,
            castTime = skillData.finalCastTime,
            duration = skillData.finalDuration,
            range = skillData.finalRange
        };

        // ��ų ����Ʈ ��ü ����
        foreach (var effect in skillData.finalEffects)
        {
            effect.ApplyEffect(context);
        }

        // ����ü �Ҹ�
        Destroy(gameObject);
    }
}
