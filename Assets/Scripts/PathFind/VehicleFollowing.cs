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

        // ������ ���� �ӵ�
        private float curSpeed;

        private int curPathIndex;
        private float pathLength;
        private Vector3 targetPoint;

        private Vector3 velocity;

        private void Start()
        {
            pathLength = path.Length;
            curPathIndex = 0;

            // ������ ���� �ӵ��� ��´�
            velocity = transform.forward;
        }

        private void Update()
        {
            // �ӵ��� ����
            curSpeed = speed * Time.deltaTime;

            targetPoint = path.GetPoint(curPathIndex);

            // �������� ������ ���� ������ ����� ���� �������� �̵�
            if (Vector3.Distance(transform.position, targetPoint) < path.Radius)
            {
                // ��ΰ� ������ ����
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

            // ���� ������ �������� �ʾҴٸ� ��� �̵�
            if (curPathIndex >= pathLength)
            {
                return;
            }

            // ��θ� ���� ���� Velocity�� ���
            if (curPathIndex >= pathLength - 1 && !isLooping)
            {
                velocity += Steer(targetPoint, true);
            }
            else
            {
                velocity += Steer(targetPoint);
            }

            // �ӵ��� ���� ���� �̵�
            transform.position += velocity;
            // ���ϴ� Velocity�� ������ ȸ��
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        // �������� ������ ������ �ٲٴ� ���� �˰���
        public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
        {
            // ���� ��ġ���� ������ �������� ���� ���͸� ����Ѵ�
            Vector3 desiredVelocity = (target - transform.position);
            float dist = desiredVelocity.magnitude;

            // ���ϴ� Velocity�� ����ȭ
            desiredVelocity.Normalize();

            // �ӷ¿� ���� �ӵ��� ���
            if (bFinalPoint && dist < 10.0f)
            {
                desiredVelocity *= (curSpeed * (dist / 10.0f));
            }
            else
            {
                desiredVelocity *= curSpeed;
            }

            // �� Vector ���
            Vector3 steeringForce = desiredVelocity - velocity;
            Vector3 acceleration = steeringForce / mass;

            return acceleration;
        }
    }
}
