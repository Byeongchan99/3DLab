using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Character : MonoBehaviour, IDamageable
{
    [Header("Enemy Settings")]
    public float maxHP = 100f;
    public float currentHP;

    private List<ShieldComponent> activeShields = new List<ShieldComponent>();

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void AddShield(ShieldComponent shield)
    {
        // 리스트에 추가
        activeShields.Add(shield);
    }

    public void RemoveShield(ShieldComponent shield)
    {
        // 어떤 Shield가 사라지면 리스트에서 제거
        if (activeShields.Contains(shield))
        {
            activeShields.Remove(shield);
        }
    }

    // IDamageable 구현
    public void TakeDamage(float amount, GameObject source)
    {
        float remainingDamage = amount;

        // 1) 보호막을 순회하며 대미지 흡수
        //    (단, 어떤 순서로 깎을지는 디자인 결정)
        //    여기선 "가장 먼저 등록된 보호막부터" 순차적으로 깎는 예시
        for (int i = activeShields.Count - 1; i >= 0; i--)
        {
            ShieldComponent s = activeShields[i];
            remainingDamage = s.AbsorbDamage(remainingDamage);
            if (remainingDamage <= 0f) break; // 대미지 다 막음
        }

        // 2) 아직 대미지가 남으면 HP 깎기
        if (remainingDamage > 0f)
        {
            currentHP -= remainingDamage;
            if (currentHP <= 0f)
            {
                // 사망 처리
                Die();
            }
        }
    }

    // 체력 회복
    public void Heal(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
    }

    // 체력이 0 이하가 되면 사망 처리
    private void Die()
    {
        Debug.Log($"적 {gameObject.name} 파괴!");
        // 간단히 오브젝트 파괴
        Destroy(gameObject);
    }
}
