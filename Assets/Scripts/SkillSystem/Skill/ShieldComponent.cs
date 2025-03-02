using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShieldComponent : MonoBehaviour
{
    public float currentShield;
    private float duration;
    private Character owner;

    public void Init(float shieldAmount, float shieldDuration, Character ownerRef)
    {
        currentShield = shieldAmount;
        duration = shieldDuration;
        owner = ownerRef;
        StartCoroutine(ShieldLifetime());
    }

    public float AbsorbDamage(float dmg)
    {
        if (dmg <= currentShield)
        {
            currentShield -= dmg;
            Debug.Log(dmg + "��ŭ�� ������� ����߽��ϴ�.");
            return 0f;
        }
        else
        {
            float leftover = dmg - currentShield;
            currentShield = 0f;
            Debug.Log(dmg + "��ŭ�� ������� ����Ͽ� �ı��Ǿ����ϴ�.");
            Destroy(this); // �ڽ� �ı�
            return leftover;
        }
    }

    private IEnumerator ShieldLifetime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this);
    }

    private void OnDestroy()
    {
        if (owner != null)
        {
            Debug.Log("��ȣ���� �ı��Ǿ����ϴ�.");
            owner.RemoveShield(this);
        }
    }
}
