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

    // IDamageable ����
    public void TakeDamage(float amount, GameObject source)
    {
        currentHP -= amount;
        Debug.Log($"���� {source.name} ���� {amount}�� ���ظ� ����. ���� HP = {currentHP}");

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    // ü���� 0 ���ϰ� �Ǹ� ��� ó��
    private void Die()
    {
        Debug.Log($"�� {gameObject.name} �ı�!");
        // ������ ������Ʈ �ı�
        Destroy(gameObject);
    }
}
