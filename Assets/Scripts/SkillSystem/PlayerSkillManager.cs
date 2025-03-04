using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public PlayerController playerController;
    public SkillExecutor skillExecutor;

    public SkillSpawner skillspawner;
    public BaseSkillDataManager baseSkillDataManager;
    public ModifierManager modifierManager;
    // �÷��̾ ���� ���� ��ų
    public List<PlayerSkillData> playerSkillList = new List<PlayerSkillData>();

    // 1) ��ų ����
    public void AddSkill(int baseSkillDataIndex)
    {
        BaseSkillData baseSkillData = baseSkillDataManager.GetBaseSkillData(baseSkillDataIndex);
        // PlayerSkillData�� ���� ���� ����Ʈ�� �߰�
        PlayerSkillData newSkill = new PlayerSkillData(baseSkillData);
        playerSkillList.Add(newSkill);

        Debug.Log($"��ų �߰�: {baseSkillData.skillName}");
    }

    // 2) ��ų ��ȭ (��: ���̾ ��ȭ)
    //    �����δ� �ε��� �Ǵ� �̸� ������ ����� ã�Ƽ� Modifier ����
    public void UpgradeSkill(int skillIndex, int modifierIndex)
    {
        if (skillIndex < 0 || skillIndex >= playerSkillList.Count) return;

        PlayerSkillData skillData = playerSkillList[skillIndex];
        SkillModifier modifier = modifierManager.GetModifier(modifierIndex);

        // Modifier�� ����
        modifier.Apply(skillData);
    }

    // 3) ��ų ���
    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= playerSkillList.Count) return;

        PlayerSkillData skillData = playerSkillList[skillIndex];
        GameObject target = playerController.GetTarget();

        switch (skillData.baseSkillData.hitType)
        {
            case SkillHitType.InstantTargeting:
                Debug.Log("��� Ÿ���� ��ų ���");              
                skillExecutor.ExecuteSkill(gameObject, target, skillData);
                break;           
            case SkillHitType.ProjectileNonTargeting:
                Debug.Log("��Ÿ���� ����ü ��ų ���");
                skillspawner.SpawnProjectile(gameObject, skillData);
                break;
            case SkillHitType.AreaNonTargeting:
                Debug.Log("��Ÿ���� ���� ��ų ���");
                skillspawner.SpawnArea(gameObject, target.transform.position, skillData);
                break;
        }
    }    
}

