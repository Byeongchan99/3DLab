using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    internal FlockController controller;
    public Rigidbody rb;  // Rigidbody 컴포넌트를 저장할 변수 선언

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트를 가져와서 변수에 저장
    }

    private void Update()
    {
        if (controller != null)  // controller가 null이 아닌지 확인
        {
            Vector3 relativePos = steer() * Time.deltaTime;

            if (relativePos != Vector3.zero)
            {
                rb.velocity = relativePos;  // rigidbody 대신 rb를 사용
                // boid의 최소와 최대 속도를 강제한다.
                float speed = rb.velocity.magnitude;
                if (speed > controller.maxVelocity)
                {
                    rb.velocity = rb.velocity.normalized * controller.maxVelocity;
                }
                else if (speed < controller.minVelocity)
                {
                    rb.velocity = rb.velocity.normalized * controller.minVelocity;
                }
            }
        }
    }

    private Vector3 steer()
    {
        Vector3 center = controller.flockCenter - transform.localPosition; // 응집
        Vector3 velocity = controller.flockVelocity - rb.velocity; // 정렬
        Vector3 follow = controller.target.localPosition - transform.localPosition; // 리더 추종

        Vector3 separation = Vector3.zero;

        foreach (Flock flock in controller.flockList)
        {
            if (flock != this)
            {
                Vector3 relativePos = transform.localPosition - flock.transform.localPosition;
                separation += relativePos / (relativePos.sqrMagnitude);
            }
        }

        // 무작위화
        Vector3 radomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
        radomize.Normalize();

        return (controller.centerWeight * center +
                controller.velocityWeight * velocity +
                controller.separationWeight * separation +
                controller.followWeight * follow +
                controller.randomizeWeight * radomize);
    }
}
