using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerSkillManager playerSkillManager;
    // �׽�Ʈ
    public Character enemy;

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
            playerSkillManager.UseSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            // 1�� ��ų ���
            playerSkillManager.UseSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // 2�� ��ų ���
            playerSkillManager.UseSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            // 3�� ��ų ���
            playerSkillManager.UseSkill(3);
        }
    }

    // �׽�Ʈ�� - ������ ���콺 Ŭ�� ������ Ÿ���� �����ؾ� ��
    public GameObject GetTarget()
    {
        return enemy.gameObject;
    }

    public void OnClickAddSkillButton(int skillIndex)
    {
        playerSkillManager.AddSkill(skillIndex);
    }

    // UI���� ��ư���� �����Ͽ� ȣ���� ���� ����
    public void OnClickUseSkillButton(int skillIndex)
    {
        playerSkillManager.UseSkill(skillIndex);
    }

    // UI���� ��ȭ ��ư ������ ��
    public void OnClickUpgradeFireBallButton(int skillIndex)
    {
        // 0�� ��ȭ = ����� ��ȭ, 1�� ��ȭ = ��Ʈ ����� �߰�
        //playerSkillManager.UpgradeSkill(skillIndex, 0); // �׽�Ʈ��
        playerSkillManager.UpgradeSkill(skillIndex, 1); // �׽�Ʈ��
    }

    public void OnClickUpgradeHealButton(int skillIndex)
    {
        playerSkillManager.UpgradeSkill(skillIndex, 2); // �׽�Ʈ��
    }
}
