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
            Debug.Log(dmg + "만큼의 대미지를 흡수했습니다.");
            return 0f;
        }
        else
        {
            float leftover = dmg - currentShield;
            currentShield = 0f;
            Debug.Log(dmg + "만큼의 대미지를 흡수하여 파괴되었습니다.");
            Destroy(this); // 자신 파괴
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
            Debug.Log("보호막이 파괴되었습니다.");
            owner.RemoveShield(this);
        }
    }
}
