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
        // ����Ʈ�� �߰�
        activeShields.Add(shield);
    }

    public void RemoveShield(ShieldComponent shield)
    {
        // � Shield�� ������� ����Ʈ���� ����
        if (activeShields.Contains(shield))
        {
            activeShields.Remove(shield);
        }
    }

    // IDamageable ����
    public void TakeDamage(float amount, GameObject source)
    {
        float remainingDamage = amount;

        // 1) ��ȣ���� ��ȸ�ϸ� ����� ���
        //    (��, � ������ �������� ������ ����)
        //    ���⼱ "���� ���� ��ϵ� ��ȣ������" ���������� ��� ����
        for (int i = activeShields.Count - 1; i >= 0; i--)
        {
            ShieldComponent s = activeShields[i];
            remainingDamage = s.AbsorbDamage(remainingDamage);
            if (remainingDamage <= 0f) break; // ����� �� ����
        }

        // 2) ���� ������� ������ HP ���
        if (remainingDamage > 0f)
        {
            currentHP -= remainingDamage;
            if (currentHP <= 0f)
            {
                // ��� ó��
                Die();
            }
        }
    }

    // ü�� ȸ��
    public void Heal(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
    }

    // ü���� 0 ���ϰ� �Ǹ� ��� ó��
    private void Die()
    {
        Debug.Log($"�� {gameObject.name} �ı�!");
        // ������ ������Ʈ �ı�
        Destroy(gameObject);
    }
}
