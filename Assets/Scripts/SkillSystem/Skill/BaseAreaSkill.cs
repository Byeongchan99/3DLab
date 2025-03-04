using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAreaSkill : MonoBehaviour
{
    private GameObject caster;
    private PlayerSkillData skillData;

    // ���� ���� �ð�
    public float lifeTime = 5f;
    private float timer = 0f;

    // (1) �̹� ���� ���� ȿ������ ����Ǿ����� ����
    //     - OnTriggerEnter/Stay���� �ߺ� �˻�
    private Dictionary<GameObject, float> affectedTargets = new Dictionary<GameObject, float>();

    // (2) ���������� ������ �ð� ���
    private Dictionary<GameObject, float> lastApplyTime = new Dictionary<GameObject, float>();

    // (3) ������ �ĺ��� ID (DotEffect���� ��AddOrRefreshDot�� �� �ߺ� �ĺ���)
    private int areaID;

    public void Init(GameObject caster, PlayerSkillData skillData)
    {
        this.caster = caster;
        this.skillData = skillData;
        lifeTime = skillData.finalDuration;

        // ��: areaID = this.GetInstanceID()�� ����ؼ�
        areaID = this.GetInstanceID();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == caster) return;

        // ó�� ���� ��
        if (!affectedTargets.ContainsKey(other.gameObject))
        {
            affectedTargets.Add(other.gameObject, Time.time);

            Debug.Log("���� ȿ�� ���� ����: " + other.gameObject.name);
            // ��: DotEffect��� ù ApplyEffect
            // DamageEffect�� "1�ʸ���" ������ ���, ���� ��� ó������ ���� ���� ����
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == caster) return;
        if (!affectedTargets.ContainsKey(other.gameObject))
        {
            // Ȥ�� ���������ٰ� �ٽ� ���� ���
            affectedTargets.Add(other.gameObject, Time.time);
            Debug.Log("���� ȿ�� ���� ����(Stay����): " + other.gameObject.name);
        }

        // ���� ����ų ����Ʈ���� Ȯ���ϸ鼭, 
        // 1) DamageEffect��� => 1�ʸ��� ���� �����
        // 2) DotEffect��� => ���ӽð� ��������
        foreach (var effect in skillData.finalEffects)
        {
            // ���� �ð��� �ִ� ȿ������ Ȯ��
            if (effect.duration > 0f)
            {
                RefreshEffect(other.gameObject, effect);
            }
            else
            {
                ApplyEffectEverySecond(other.gameObject, effect);
            }          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���ǿ��� ���� �� - ���ǿ��� ���� �����ٸ� �ݺ��Ҽ��� ������, ���� �� 1�� �Ŀ� ������ ��
        if (affectedTargets.ContainsKey(other.gameObject))
        {
            affectedTargets.Remove(other.gameObject);
        }
        // ��DotEffect��ó�� ���� ȿ���� ���� ��쿡��,
        // ���⼭ ������ �ð� ���š��� ���߸� �˾Ƽ� ������ ���������.
        Debug.Log($"���ǿ��� ����: {other.gameObject.name}");
    }

    // (A) DamageEffect: ���� ���� �ִ� ����, 1�ʸ��� direct damage
    private void ApplyEffectEverySecond(GameObject target, SkillEffect effect)
    {
        // lastDamageTime���� ������ ȿ���� �� �ð� Ȯ��
        float lastTime;
        if (!lastApplyTime.TryGetValue(target, out lastTime))
        {
            // ���ٸ� 0���� �ʱ�ȭ
            lastTime = 0f;
        }

        // 1�� ���� �� �ٽ� ȿ�� ����
        if (Time.time - lastTime >= 1f)
        {
            // ����
            lastApplyTime[target] = Time.time;

            // ���� ȿ�� ����
            var context = MakeSkillContext(target);
            effect.ApplyEffect(context);
        }
    }

    // (B) DotEffect: ���� ���� �ִ� ����, 
    //     ��� DotEffect.ApplyEffect => DotComponent.AddOrRefreshDot(areaID,...)
    private void RefreshEffect(GameObject target, SkillEffect effect)
    {
        // DotEffect.ApplyEffect(context)��
        // dotComp.AddOrRefreshDot(context.skillID, damagePerSec, duration, caster)
        // �� ���ο��� ȣ��. => "���� �ð� ����"
        var context = MakeSkillContext(target);

        // ���� skillID�� ����ID�� Ȱ���ϸ� ������ ���ǡ��� ���� ��Ʈ�� ��� ���ŵ�
        context.skillID = areaID;

        effect.ApplyEffect(context);
    }

    // SkillContext ����� ����
    private SkillContext MakeSkillContext(GameObject target)
    {
        return new SkillContext()
        {
            caster = caster,
            target = target,
            damage = skillData.finalDamage,
            damageCoefficient = skillData.finalCoefficient,
            cost = skillData.finalCost,
            cooldown = skillData.finalCooldown,
            castTime = skillData.finalCastTime,
            duration = skillData.finalDuration,
            range = skillData.finalRange,
            // ���� ȿ�� �ĺ���
            skillID = areaID
        };
    }
}
