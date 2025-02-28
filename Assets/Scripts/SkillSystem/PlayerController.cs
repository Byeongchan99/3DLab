using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerSkillManager playerSkillManager;
    // �׽�Ʈ
    public Enemy enemy;

    void Awake()
    {
        playerSkillManager = GetComponent<PlayerSkillManager>();
    }

    // ��: Ű���� �Է����� ��ų ���
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 0�� ��ų ���
            // caster = this.gameObject (�÷��̾�)
            // target = �ӽ÷� �ڽ��̶�ų�, ����ĳ��Ʈ�� ���� �� ��
            playerSkillManager.UseSkill(0, this.gameObject, this.gameObject);
        }
    }

    public void OnClickAddSkillButton(int skillIndex)
    {
        playerSkillManager.AddSkill(skillIndex);
    }

    // UI���� ��ư���� �����Ͽ� ȣ���� ���� ����
    public void OnClickUseSkillButton(int skillIndex)
    {
        playerSkillManager.UseSkill(skillIndex, this.gameObject, enemy.gameObject);
    }

    // UI���� ��ȭ ��ư ������ ��
    public void OnClickUpgradeSkillButton(int skillIndex)
    {
        // ���̾ = 0�� ��ų
        playerSkillManager.UpgradeSkill(skillIndex, 0); // �׽�Ʈ��
    }
}
