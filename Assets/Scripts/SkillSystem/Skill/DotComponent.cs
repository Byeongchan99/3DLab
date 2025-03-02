using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotComponent : MonoBehaviour
{
    private float dps;      // damage per second
    private float dur;      // dot ���� �ð�
    private GameObject caster; // ��Ʈ�� �� ��ü(������)

    public void Init(float damagePerSec, float duration, GameObject source)
    {
        dps = damagePerSec;
        dur = duration;
        caster = source;

        // �ڷ�ƾ ����
        StartCoroutine(DotRoutine());
    }

    private IEnumerator DotRoutine()
    {
        float elapsed = 0f;
        IDamageable dmg = GetComponent<IDamageable>();

        // elapsed�� dur(���ӽð�)�� ������ ������
        while (elapsed < dur)
        {
            // Ÿ�ٿ��� ����� ����           
            if (dmg != null)
            {
                dmg.TakeDamage(dps, caster);
            }

            // 1�� ���
            yield return new WaitForSeconds(1f);

            elapsed += 1f;
        }

        // ��Ʈ ������ �ڽ� ������Ʈ ����
        Destroy(this);
    }
}
