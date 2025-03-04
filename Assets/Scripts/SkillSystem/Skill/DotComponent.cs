using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotComponent : MonoBehaviour
{
    // 하나의 DoT 항목(소스, dps, 남은 지속시간, 코루틴...)
    private class DotInfo
    {
        public int sourceID;       // 도트를 건 주체(장판, 스킬 등)의 식별자
        public float dps;          // 초당 대미지
        public float duration;     // 남은 지속시간
        public GameObject caster;  // 공격자 (필요 없다면 제외)
    }

    // 여러 도트를 관리하기 위한 자료구조
    private List<DotInfo> dotList = new List<DotInfo>();

    // 도트 tick을 처리할 코루틴
    private Coroutine dotRoutine;

    // *** 도트를 추가하거나, 기존 도트가 있으면 갱신 ***
    // sourceID: 장판이나 스킬 등을 구분하기 위해 GetInstanceID()나 고유값 사용
    // damagePerSec: 초당 대미지
    // duration: 새로 부여할 도트의 지속 시간
    // caster: 공격자
    public void AddOrRefreshDot(int sourceID, float damagePerSec, float duration, GameObject caster)
    {
        DotInfo existingDot = dotList.Find(d => d.sourceID == sourceID);
        if (existingDot != null)
        {
            // 이미 같은 출처(sourceID)의 도트가 걸려있다면 -> 지속시간, dps 갱신
            existingDot.dps = damagePerSec;
            existingDot.duration = duration;
            existingDot.caster = caster;
        }
        else
        {
            // 새로운 도트 추가
            DotInfo newDot = new DotInfo()
            {
                sourceID = sourceID,
                dps = damagePerSec,
                duration = duration,
                caster = caster
            };
            dotList.Add(newDot);
        }

        // 코루틴이 없으면 시작
        if (dotRoutine == null)
        {
            dotRoutine = StartCoroutine(DotTickRoutine());
        }
    }

    // 코루틴: 1초마다 Tick을 주고, 각 도트의 duration을 깎음
    private IEnumerator DotTickRoutine()
    {
        IDamageable dmg = GetComponent<IDamageable>();

        while (dotList.Count > 0)
        {       
            // 2) 각 도트마다 대미지 적용 & 지속시간 감소
            for (int i = dotList.Count - 1; i >= 0; i--)
            {
                dotList[i].duration -= 1f; // 1초 경과
                if (dmg != null)
                {
                    // 대미지 적용
                    dmg.TakeDamage(dotList[i].dps, dotList[i].caster);
                }

                // 만료된 도트는 리스트에서 제거
                if (dotList[i].duration <= 0f)
                {
                    dotList.RemoveAt(i);
                }
            }
            // 1) 1초 기다리고
            yield return new WaitForSeconds(1f);
        }

        // 모든 도트가 사라지면 컴포넌트 제거
        dotRoutine = null;
        Destroy(this);
    }
}
