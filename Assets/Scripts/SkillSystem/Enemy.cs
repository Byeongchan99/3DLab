using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Settings")]
    public float maxHP = 100f;
    public float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // IDamageable 구현
    public void TakeDamage(float amount, GameObject source)
    {
        currentHP -= amount;
        Debug.Log($"적이 {source.name} 에게 {amount}의 피해를 받음. 현재 HP = {currentHP}");

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    // 체력이 0 이하가 되면 사망 처리
    private void Die()
    {
        Debug.Log($"적 {gameObject.name} 파괴!");
        // 간단히 오브젝트 파괴
        Destroy(gameObject);
    }
}
