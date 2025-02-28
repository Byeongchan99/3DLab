using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    public float speed = 10f;         // 이동 속도
    public float lifeTime = 5f;       // 생존 시간(초)

    private float lifeTimer;

    private void Update()
    {
        // 1) 전방(자신의 transform.forward) 방향으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 2) 일정 시간 지나면 파괴
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    // 충돌 판정: Collider가 'isTrigger = false'면 OnCollisionEnter,
    // Trigger 방식이면 OnTriggerEnter를 사용.
    private void OnTriggerEnter(Collider other)
    {
        // 여기서 Enemy 등을 체크해 대미지 주기 가능
        // 예:
        if (other.CompareTag("Enemy"))
        {
            // 대미지 처리
            // IDamageable dmg = other.GetComponent<IDamageable>();
            // if (dmg != null) dmg.TakeDamage(50, this.gameObject);

            // 파이어볼 소멸
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            // 벽 등 막히면 사라지도록
            Destroy(gameObject);
        }
    }
}
