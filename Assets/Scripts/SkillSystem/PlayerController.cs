using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerSkillManager playerSkillManager;
    // 테스트
    public Character enemy;

    void Awake()
    {
        playerSkillManager = GetComponent<PlayerSkillManager>();
    }

    // 예: 키보드 입력으로 스킬 사용
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 0번 스킬 사용
            // caster = this.gameObject (플레이어)
            // target = 임시로 자신이라거나, 레이캐스트로 맞춘 적 등
            playerSkillManager.UseSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            // 1번 스킬 사용
            playerSkillManager.UseSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // 2번 스킬 사용
            playerSkillManager.UseSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            // 3번 스킬 사용
            playerSkillManager.UseSkill(3);
        }
    }

    // 테스트용 - 원래는 마우스 클릭 등으로 타겟을 지정해야 함
    public GameObject GetTarget()
    {
        return enemy.gameObject;
    }

    public void OnClickAddSkillButton(int skillIndex)
    {
        playerSkillManager.AddSkill(skillIndex);
    }

    // UI에서 버튼으로 연결하여 호출할 수도 있음
    public void OnClickUseSkillButton(int skillIndex)
    {
        playerSkillManager.UseSkill(skillIndex);
    }

    // UI에서 강화 버튼 눌렀을 때
    public void OnClickUpgradeFireBallButton(int skillIndex)
    {
        // 0번 강화 = 대미지 강화, 1번 강화 = 도트 대미지 추가
        //playerSkillManager.UpgradeSkill(skillIndex, 0); // 테스트용
        playerSkillManager.UpgradeSkill(skillIndex, 1); // 테스트용
    }

    public void OnClickUpgradeHealButton(int skillIndex)
    {
        playerSkillManager.UpgradeSkill(skillIndex, 2); // 테스트용
    }
}
