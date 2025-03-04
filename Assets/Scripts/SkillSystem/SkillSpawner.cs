using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    // 투사체 소환
    public void SpawnProjectile(GameObject caster, PlayerSkillData skillData)
    {
        // 1) 스킬 프리팹을 가져옴
        var projectilePrefab = skillData.skillPrefab; // 가정
        if (!projectilePrefab) return;

        // 2) 생성 위치 및 방향 (예: 캐스터 앞)
        Vector3 spawnPos = caster.transform.position + caster.transform.forward * 1.0f;
        Quaternion spawnRot = caster.transform.rotation;

        // 3) Instantiate
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, spawnRot);

        // 4) Projectile 스크립트(예: FireballProjectile)에 스킬 정보 전달
        var projectileScript = projectile.GetComponent<BaseProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Init(caster, skillData);
        }
    }

    // 영역 소환
    public void SpawnArea(GameObject caster, Vector3 spawnPosition, PlayerSkillData skillData)
    {
        // 1) 스킬 프리팹을 가져옴
        var areaPrefab = skillData.skillPrefab; // 가정
        if (!areaPrefab) return;

        // 2) 생성 위치 (예: 캐스터 앞)
        Vector3 spawnPos = spawnPosition;
        Quaternion spawnRot = caster.transform.rotation;

        // 3) Instantiate
        GameObject area = Instantiate(areaPrefab, spawnPos, spawnRot);

        // 4) Area 스크립트(예: FirestormArea)에 스킬 정보 전달
        var areaScript = area.GetComponent<BaseAreaSkill>();
        if (areaScript != null)
        {
            areaScript.Init(caster, skillData);
        }
    }

    // 소환물 소환
    public void SpawnSummon(GameObject caster, PlayerSkillData skillData)
    {
        // 1) 소환물 프리팹을 가져옴
        var summonPrefab = skillData.skillPrefab; // 가정
        if (!summonPrefab) return;

        // 2) 생성 위치 (예: 캐스터 앞)
        Vector3 spawnPos = caster.transform.position + caster.transform.forward * 1.0f;
        Quaternion spawnRot = caster.transform.rotation;

        // 3) Instantiate
        GameObject summon = Instantiate(summonPrefab, spawnPos, spawnRot);

        /*
        // 4) Summon 스크립트에 스킬 정보 전달
        var summonScript = summon.GetComponent<BaseSummon>();
        if (summonScript != null)
        {
            summonScript.Init(caster, skillData);
        }
        */
    }
}
