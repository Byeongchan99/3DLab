using System.Collections;
using System.Collections.Generic;
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

    // 3) ��ų ���
    public void UseSkill(int skillIndex, GameObject caster, GameObject target)
    {
        if (skillIndex < 0 || skillIndex >= playerSkillList.Count) return;

        PlayerSkillData skillData = playerSkillList[skillIndex];
        // SkillExecutor�� ���� ��ų ȿ���� ����
        skillExecutor.ExecuteSkill(caster, target, skillData);
    }
}

