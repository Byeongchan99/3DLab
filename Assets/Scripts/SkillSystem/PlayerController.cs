using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerSkillManager playerSkillManager;
    // 테스트
    public Enemy enemy;

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
            playerSkillManager.UseSkill(0, this.gameObject, this.gameObject);
        }
    }

    public void OnClickAddSkillButton(int skillIndex)
    {
        playerSkillManager.AddSkill(skillIndex);
    }

    // UI에서 버튼으로 연결하여 호출할 수도 있음
    public void OnClickUseSkillButton(int skillIndex)
    {
        playerSkillManager.UseSkill(skillIndex, this.gameObject, enemy.gameObject);
    }

    // UI에서 강화 버튼 눌렀을 때
    public void OnClickUpgradeSkillButton(int skillIndex)
    {
        // 파이어볼 = 0번 스킬
        playerSkillManager.UpgradeSkill(skillIndex, 0); // 테스트용
    }
}
