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

    // IDamageable ����
    public void TakeDamage(float amount, GameObject source)
    {
        currentHP -= amount;
        Debug.Log($"Enemy took {amount} damage from {source.name}. Current HP = {currentHP}");

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    // ü���� 0 ���ϰ� �Ǹ� ��� ó��
    private void Die()
    {
        Debug.Log($"Enemy {gameObject.name} died!");
        // ������ ������Ʈ �ı�
        Destroy(gameObject);
    }
}
