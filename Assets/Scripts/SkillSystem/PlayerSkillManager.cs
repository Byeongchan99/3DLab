using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillExecutor skillExecutor;
    public BaseSkillDataManager baseSkillDataManager;
    public ModifierManager modifierManager;
    // 플레이어가 보유 중인 스킬
    public List<PlayerSkillData> playerSkillList = new List<PlayerSkillData>();

    // 1) 스킬 습득
    public void AddSkill(int baseSkillDataIndex)
    {
        BaseSkillData baseSkillData = baseSkillDataManager.GetBaseSkillData(baseSkillDataIndex);
        // PlayerSkillData를 새로 만들어서 리스트에 추가
        PlayerSkillData newSkill = new PlayerSkillData(baseSkillData);
        playerSkillList.Add(newSkill);

        Debug.Log($"스킬 추가: {baseSkillData.skillName}");
    }

    // 2) 스킬 강화 (예: 파이어볼 강화)
    //    실제로는 인덱스 또는 이름 등으로 대상을 찾아서 Modifier 적용
    public void UpgradeSkill(int skillIndex, int modifierIndex)
    {
        if (skillIndex < 0 || skillIndex >= playerSkillList.Count) return;

        PlayerSkillData skillData = playerSkillList[skillIndex];
        SkillModifier modifier = modifierManager.GetModifier(modifierIndex);

        // Modifier를 적용
        modifier.Apply(skillData);
    }

    // 3) 스킬 사용
    public void UseSkill(int skillIndex, GameObject caster, GameObject target)
    {
        if (skillIndex < 0 || skillIndex >= playerSkillList.Count) return;

        PlayerSkillData skillData = playerSkillList[skillIndex];
        // SkillExecutor가 실제 스킬 효과를 적용
        skillExecutor.ExecuteSkill(caster, target, skillData);
    }
}

