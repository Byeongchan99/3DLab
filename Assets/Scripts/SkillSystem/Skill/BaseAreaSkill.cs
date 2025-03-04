using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAreaSkill : MonoBehaviour
{
    private GameObject caster;
    private PlayerSkillData skillData;

    // 장판 유지 시간
    public float lifeTime = 5f;
    private float timer = 0f;

    // (1) 이미 “이 장판 효과”가 적용되었는지 추적
    //     - OnTriggerEnter/Stay에서 중복 검사
    private Dictionary<GameObject, float> affectedTargets = new Dictionary<GameObject, float>();

    // (2) 마지막으로 적용한 시간 기록
    private Dictionary<GameObject, float> lastApplyTime = new Dictionary<GameObject, float>();

    // (3) 장판을 식별할 ID (DotEffect에서 “AddOrRefreshDot” 시 중복 식별용)
    private int areaID;

    public void Init(GameObject caster, PlayerSkillData skillData)
    {
        this.caster = caster;
        this.skillData = skillData;
        lifeTime = skillData.finalDuration;

        // 예: areaID = this.GetInstanceID()를 사용해서
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

        // 처음 밟을 때
        if (!affectedTargets.ContainsKey(other.gameObject))
        {
            affectedTargets.Add(other.gameObject, Time.time);

            Debug.Log("장판 효과 최초 적용: " + other.gameObject.name);
            // 예: DotEffect라면 첫 ApplyEffect
            // DamageEffect는 "1초마다" 구조일 경우, 여긴 즉시 처리하지 않을 수도 있음
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == caster) return;
        if (!affectedTargets.ContainsKey(other.gameObject))
        {
            // 혹시 빠져나갔다가 다시 들어온 경우
            affectedTargets.Add(other.gameObject, Time.time);
            Debug.Log("장판 효과 최초 적용(Stay에서): " + other.gameObject.name);
        }

        // 이제 “스킬 이펙트”를 확인하면서, 
        // 1) DamageEffect라면 => 1초마다 직접 대미지
        // 2) DotEffect라면 => 지속시간 리프레시
        foreach (var effect in skillData.finalEffects)
        {
            // 지속 시간이 있는 효과인지 확인
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
        // 장판에서 나갈 때 - 장판에서 들어갔다 나갔다를 반복할수도 있으니, 나간 뒤 1초 후에 지워야 함
        if (affectedTargets.ContainsKey(other.gameObject))
        {
            affectedTargets.Remove(other.gameObject);
        }
        // “DotEffect”처럼 지속 효과가 남는 경우에는,
        // 여기서 “지속 시간 갱신”을 멈추면 알아서 끝나면 사라지겠죠.
        Debug.Log($"장판에서 나감: {other.gameObject.name}");
    }

    // (A) DamageEffect: 장판 위에 있는 동안, 1초마다 direct damage
    private void ApplyEffectEverySecond(GameObject target, SkillEffect effect)
    {
        // lastDamageTime에서 이전에 효과를 준 시간 확인
        float lastTime;
        if (!lastApplyTime.TryGetValue(target, out lastTime))
        {
            // 없다면 0으로 초기화
            lastTime = 0f;
        }

        // 1초 지난 뒤 다시 효과 적용
        if (Time.time - lastTime >= 1f)
        {
            // 갱신
            lastApplyTime[target] = Time.time;

            // 실제 효과 적용
            var context = MakeSkillContext(target);
            effect.ApplyEffect(context);
        }
    }

    // (B) DotEffect: 장판 위에 있는 동안, 
    //     계속 DotEffect.ApplyEffect => DotComponent.AddOrRefreshDot(areaID,...)
    private void RefreshEffect(GameObject target, SkillEffect effect)
    {
        // DotEffect.ApplyEffect(context)는
        // dotComp.AddOrRefreshDot(context.skillID, damagePerSec, duration, caster)
        // 를 내부에서 호출. => "지속 시간 리셋"
        var context = MakeSkillContext(target);

        // 만약 skillID를 장판ID로 활용하면 “동일 장판”에 의한 도트가 계속 갱신됨
        context.skillID = areaID;

        effect.ApplyEffect(context);
    }

    // SkillContext 만드는 헬퍼
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
            // 장판 효과 식별자
            skillID = areaID
        };
    }
}
