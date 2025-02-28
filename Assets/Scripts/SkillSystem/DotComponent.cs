using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotComponent : MonoBehaviour
{
    private float dps;      // damage per second
    private float dur;      // dot ���� �ð�
    private GameObject src; // ��Ʈ�� �� ��ü(������)

    public void Init(float damagePerSec, float duration, GameObject source)
    {
        dps = damagePerSec;
        dur = duration;
        src = source;

        // �ڷ�ƾ ����
        StartCoroutine(DotRoutine());
    }

    private IEnumerator DotRoutine()
    {
        float elapsed = 0f;

        // elapsed�� dur(���ӽð�)�� ������ ������
        while (elapsed < dur)
        {
            // Ÿ�ٿ��� ����� ����
            IDamageable dmg = GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(dps, src);
            }

            // 1�� ���
            yield return new WaitForSeconds(1f);

            elapsed += 1f;
        }

        // ��Ʈ ������ �ڽ� ������Ʈ ����
        Destroy(this);
    }
}
