using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFlockController : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 bound;
    public float speed = 100.0f;

    private Vector3 initialPosition;
    private Vector3 nextMovementPoint;

    // 초기화에 사용됨
    private void Start()
    {
        initialPosition = transform.position;
        CalculateNextMovementPoint();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);

        if (Vector3.Distance(nextMovementPoint, transform.position) <= 10.0f)
        {
            CalculateNextMovementPoint();
        }
    }

    // 현재 위치와 바운더리 벡터 사이의 범위에서 다음으로 이동할 임의의 목적지를 찾음
    void CalculateNextMovementPoint()
    {
        float posX = Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
        float posY = Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
        float posZ = Random.Range(initialPosition.z - bound.z, initialPosition.z + bound.z);

        nextMovementPoint = initialPosition + new Vector3(posX, posY, posZ);
    }
}
