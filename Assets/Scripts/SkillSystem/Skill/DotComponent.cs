using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotComponent : MonoBehaviour
{
    // �ϳ��� DoT �׸�(�ҽ�, dps, ���� ���ӽð�, �ڷ�ƾ...)
    private class DotInfo
    {
        public int sourceID;       // ��Ʈ�� �� ��ü(����, ��ų ��)�� �ĺ���
        public float dps;          // �ʴ� �����
        public float duration;     // ���� ���ӽð�
        public GameObject caster;  // ������ (�ʿ� ���ٸ� ����)
    }

    // ���� ��Ʈ�� �����ϱ� ���� �ڷᱸ��
    private List<DotInfo> dotList = new List<DotInfo>();

    // ��Ʈ tick�� ó���� �ڷ�ƾ
    private Coroutine dotRoutine;

    // *** ��Ʈ�� �߰��ϰų�, ���� ��Ʈ�� ������ ���� ***
    // sourceID: �����̳� ��ų ���� �����ϱ� ���� GetInstanceID()�� ������ ���
    // damagePerSec: �ʴ� �����
    // duration: ���� �ο��� ��Ʈ�� ���� �ð�
    // caster: ������
    public void AddOrRefreshDot(int sourceID, float damagePerSec, float duration, GameObject caster)
    {
        DotInfo existingDot = dotList.Find(d => d.sourceID == sourceID);
        if (existingDot != null)
        {
            // �̹� ���� ��ó(sourceID)�� ��Ʈ�� �ɷ��ִٸ� -> ���ӽð�, dps ����
            existingDot.dps = damagePerSec;
            existingDot.duration = duration;
            existingDot.caster = caster;
        }
        else
        {
            // ���ο� ��Ʈ �߰�
            DotInfo newDot = new DotInfo()
            {
                sourceID = sourceID,
                dps = damagePerSec,
                duration = duration,
                caster = caster
            };
            dotList.Add(newDot);
        }

        // �ڷ�ƾ�� ������ ����
        if (dotRoutine == null)
        {
            dotRoutine = StartCoroutine(DotTickRoutine());
        }
    }

    // �ڷ�ƾ: 1�ʸ��� Tick�� �ְ�, �� ��Ʈ�� duration�� ����
    private IEnumerator DotTickRoutine()
    {
        IDamageable dmg = GetComponent<IDamageable>();

        while (dotList.Count > 0)
        {       
            // 2) �� ��Ʈ���� ����� ���� & ���ӽð� ����
            for (int i = dotList.Count - 1; i >= 0; i--)
            {
                dotList[i].duration -= 1f; // 1�� ���
                if (dmg != null)
                {
                    // ����� ����
                    dmg.TakeDamage(dotList[i].dps, dotList[i].caster);
                }

                // ����� ��Ʈ�� ����Ʈ���� ����
                if (dotList[i].duration <= 0f)
                {
                    dotList.RemoveAt(i);
                }
            }
            // 1) 1�� ��ٸ���
            yield return new WaitForSeconds(1f);
        }

        // ��� ��Ʈ�� ������� ������Ʈ ����
        dotRoutine = null;
        Destroy(this);
    }
}
