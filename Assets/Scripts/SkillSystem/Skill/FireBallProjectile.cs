using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    public float speed = 10f;         // �̵� �ӵ�
    public float lifeTime = 5f;       // ���� �ð�(��)

    private float lifeTimer;

    private void Update()
    {
        // 1) ����(�ڽ��� transform.forward) �������� �̵�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 2) ���� �ð� ������ �ı�
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    // �浹 ����: Collider�� 'isTrigger = false'�� OnCollisionEnter,
    // Trigger ����̸� OnTriggerEnter�� ���.
    private void OnTriggerEnter(Collider other)
    {
        // ���⼭ Enemy ���� üũ�� ����� �ֱ� ����
        // ��:
        if (other.CompareTag("Enemy"))
        {
            // ����� ó��
            // IDamageable dmg = other.GetComponent<IDamageable>();
            // if (dmg != null) dmg.TakeDamage(50, this.gameObject);

            // ���̾ �Ҹ�
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            // �� �� ������ ���������
            Destroy(gameObject);
        }
    }
}
