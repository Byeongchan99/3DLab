using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public void SpawnProjectile(GameObject caster, PlayerSkillData skillData)
    {
        // 1) ��ų �������� ������
        var projectilePrefab = skillData.skillPrefab; // ����
        if (!projectilePrefab) return;

        // 2) ���� ��ġ �� ���� (��: ĳ���� ��)
        Vector3 spawnPos = caster.transform.position + caster.transform.forward * 1.0f;
        Quaternion spawnRot = caster.transform.rotation;

        // 3) Instantiate
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, spawnRot);

        // 4) Projectile ��ũ��Ʈ(��: FireballProjectile)�� ��ų ���� ����
        var projectileScript = projectile.GetComponent<BaseProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Init(caster, skillData);
        }
    }

}
