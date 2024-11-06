using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    public Transform targetTransform;
    private float movementSpeed, rotSpeed;

    private void Start()
    {
        movementSpeed = 10.0f;
        rotSpeed = 2.0f;
    }

    private void Update()
    {
        // 타깃 위치 근처에 도달하면 일단 정지
        if (Vector3.Distance(targetTransform.position, transform.position) < 5.0f)
        {
            return;
        }

        // 현재 위치로부터 타깃 위치로의 방향 벡터 계산
        Vector3 tarPos = targetTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        // LookRotation 메서드를 사용해 이 새로운 회전 벡터를 위한 Quaternion 구성
        Quaternion tarRot = Quaternion.LookRotation(dirRot);

        // 보간법을 사용해서 이동하고 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }
}
