using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
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

}
