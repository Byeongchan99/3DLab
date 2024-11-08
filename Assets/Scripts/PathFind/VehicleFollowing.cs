using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFind
{
    public class VehicleFollowing : MonoBehaviour
    {
        public Path path;
        public float speed = 20.0f;
        public float mass = 5.0f;

        public bool isLooping = true;

        // 차량의 실제 속도
        private float curSpeed;

        private int curPathIndex;
        private float pathLength;
        private Vector3 targetPoint;

        private Vector3 velocity;

        private void Start()
        {
            pathLength = path.Length;
            curPathIndex = 0;

            // 차량의 현재 속도를 얻는다
            velocity = transform.forward;
        }

        private void Update()
        {
            // 속도를 통일
            curSpeed = speed * Time.deltaTime;

            targetPoint = path.GetPoint(curPathIndex);

            // 목적지의 반지름 내에 들어오면 경로의 다음 지점으로 이동
            if (Vector3.Distance(transform.position, targetPoint) < path.Radius)
            {
                // 경로가 끝나면 정지
                if (curPathIndex < pathLength - 1)
                {
                    curPathIndex++;
                }
                else if (isLooping)
                {
                    curPathIndex = 0;
                }
                else
                {
                    return;
                }
            }

            // 최종 지점에 도착하지 않았다면 계속 이동
            if (curPathIndex >= pathLength)
            {
                return;
            }

            // 경로를 따라 다음 Velocity를 계산
            if (curPathIndex >= pathLength - 1 && !isLooping)
            {
                velocity += Steer(targetPoint, true);
            }
            else
            {
                velocity += Steer(targetPoint);
            }

            // 속도에 따라 차량 이동
            transform.position += velocity;
            // 원하는 Velocity로 차량을 회전
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        // 목적지로 벡터의 방향을 바꾸는 조향 알고리즘
        public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
        {
            // 현재 위치에서 목적지 방향으로 방향 벡터를 계산한다
            Vector3 desiredVelocity = (target - transform.position);
            float dist = desiredVelocity.magnitude;

            // 원하는 Velocity를 정규화
            desiredVelocity.Normalize();

            // 속력에 따라 속도를 계산
            if (bFinalPoint && dist < 10.0f)
            {
                desiredVelocity *= (curSpeed * (dist / 10.0f));
            }
            else
            {
                desiredVelocity *= curSpeed;
            }

            // 힘 Vector 계산
            Vector3 steeringForce = desiredVelocity - velocity;
            Vector3 acceleration = steeringForce / mass;

            return acceleration;
        }
    }
}
