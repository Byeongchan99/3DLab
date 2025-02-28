using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Settings")]
    public float maxHP = 100f;
    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // IDamageable 구현
    public void TakeDamage(float amount, GameObject source)
    {
        currentHP -= amount;
        Debug.Log($"Enemy took {amount} damage from {source.name}. Current HP = {currentHP}");

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    // 체력이 0 이하가 되면 사망 처리
    private void Die()
    {
        Debug.Log($"Enemy {gameObject.name} died!");
        // 간단히 오브젝트 파괴
        Destroy(gameObject);
    }
}
