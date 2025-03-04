using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    // ����ü ��ȯ
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

    // ���� ��ȯ
    public void SpawnArea(GameObject caster, Vector3 spawnPosition, PlayerSkillData skillData)
    {
        // 1) ��ų �������� ������
        var areaPrefab = skillData.skillPrefab; // ����
        if (!areaPrefab) return;

        // 2) ���� ��ġ (��: ĳ���� ��)
        Vector3 spawnPos = spawnPosition;
        Quaternion spawnRot = caster.transform.rotation;

        // 3) Instantiate
        GameObject area = Instantiate(areaPrefab, spawnPos, spawnRot);

        // 4) Area ��ũ��Ʈ(��: FirestormArea)�� ��ų ���� ����
        var areaScript = area.GetComponent<BaseAreaSkill>();
        if (areaScript != null)
        {
            areaScript.Init(caster, skillData);
        }
    }

    // ��ȯ�� ��ȯ
    public void SpawnSummon(GameObject caster, PlayerSkillData skillData)
    {
        // 1) ��ȯ�� �������� ������
        var summonPrefab = skillData.skillPrefab; // ����
        if (!summonPrefab) return;

        // 2) ���� ��ġ (��: ĳ���� ��)
        Vector3 spawnPos = caster.transform.position + caster.transform.forward * 1.0f;
        Quaternion spawnRot = caster.transform.rotation;

        // 3) Instantiate
        GameObject summon = Instantiate(summonPrefab, spawnPos, spawnRot);

        /*
        // 4) Summon ��ũ��Ʈ�� ��ų ���� ����
        var summonScript = summon.GetComponent<BaseSummon>();
        if (summonScript != null)
        {
            summonScript.Init(caster, skillData);
        }
        */
    }
}
