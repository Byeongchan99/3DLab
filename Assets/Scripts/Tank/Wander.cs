using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    private Vector3 tarPos;

    private float movementSpeed = 5.0f;
    private float rotSpeed = 2.0f;
    private float minX, maxX, minZ, maxZ;

    // 초기화에 사용
    private void Start()
    {
        minX = -45.0f;
        maxX = 45.0f;

        minZ = -45.0f;
        maxZ = 45.0f;

        // 돌아다닐 위치 얻기
        GetNextPosition();
    }

    // Update는 초당 1회 호출됨
    private void Update()
    {
        // 목적 지점 근처인지 검사
        if (Vector3.Distance(tarPos, transform.position) <= 5.0f)
            GetNextPosition();

        // 목적지 방향으로의 회전을 위한 쿼터니온(quaternion) 설정
        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);

        // 회전과 트랜슬레이션 갱신
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }

    void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
    }
}
