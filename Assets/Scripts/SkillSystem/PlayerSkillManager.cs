using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillExecutor skillExecutor;
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

    // 3) ��ų ��� - Ÿ���� ��ų
    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= playerSkillList.Count) return;

        PlayerSkillData skillData = playerSkillList[skillIndex];

        if (skillData.baseSkillData.hitType == SkillHitType.ProjectileNonTargeting)
        {
            // Ÿ���� ��ų�� ���, Ÿ���� �����ؾ� ��
            // �ӽ÷� �ڽ��� Ÿ������ ����
            UseSkill(skillIndex, this.gameObject, this.gameObject);
        }
        // SkillExecutor�� ���� ��ų ȿ���� ����
        skillExecutor.ExecuteSkill(caster, target, skillData);
    }    
}

