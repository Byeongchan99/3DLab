using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotComponent : MonoBehaviour
{
    private float dps;      // damage per second
    private float dur;      // dot 유지 시간
    private GameObject caster; // 도트를 건 주체(공격자)

    public void Init(float damagePerSec, float duration, GameObject source)
    {
        dps = damagePerSec;
        dur = duration;
        caster = source;

        // 코루틴 시작
        StartCoroutine(DotRoutine());
    }

    private IEnumerator DotRoutine()
    {
        float elapsed = 0f;
        IDamageable dmg = GetComponent<IDamageable>();

        // elapsed가 dur(지속시간)에 도달할 때까지
        while (elapsed < dur)
        {
            // 타겟에게 대미지 적용           
            if (dmg != null)
            {
                dmg.TakeDamage(dps, caster);
            }

            // 1초 대기
            yield return new WaitForSeconds(1f);

            elapsed += 1f;
        }

        // 도트 끝나면 자신 컴포넌트 제거
        Destroy(this);
    }
}
