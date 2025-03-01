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
        // 전방으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"투사체 충돌: {other.gameObject.name}");

        // caster 자신이면 무시
        if (other.gameObject == caster) return;

        // 적중 효과 적용
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

        // 스킬 이펙트 전체 적용
        foreach (var effect in skillData.finalEffects)
        {
            effect.ApplyEffect(context);
        }

        // 투사체 소멸
        Destroy(gameObject);
    }
}
