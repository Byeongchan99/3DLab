using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public List<PlayerSkillData> playerSkillList;
    public SkillExecutor skillExecutor;

    // ��ų ��� ����
    public void UseSkill(GameObject caster, GameObject target, int skillIndex)
    {
        PlayerSkillData skillData = playerSkillList[skillIndex];
        skillExecutor.ExecuteSkill(caster, target, skillData);
    }

    public void AddSkill(PlayerSkillData skillData)
    {
        playerSkillList.Add(skillData);
    }
}

