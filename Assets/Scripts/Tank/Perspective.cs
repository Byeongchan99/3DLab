using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 100;

    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialize()
    {
        // 플레이어 위치 찾기
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update는 프레임당 한 번 호출된다
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        // 검출 범위에 있으면 시각 검사를 수행한다
        if (elapsedTime >= detectionRate)
        {
            DetectAspect();
        }
    }

    void DetectAspect()
    {
        RaycastHit hit;

        // 현재 위치로부터 플레이어 위치로의 방향
        rayDirection = playerTrans.position - transform.position;

        // 인공지능 캐릭터의 전방 벡터와 플레이어와 인공지능 캐릭터 사이의 방향 벡터 간의 각도를 검사한다
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            // 플레이어가 시야에 들어왔는지 검사
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    // 특성 검사
                    if (aspect.aspectName == aspectName)
                    {
                        print("Enemy Detected");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerTrans == null)
            return;

        Debug.DrawLine(transform.position, playerTrans.position, Color.red);

        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        // 대략적인 시야 범위 시각화
        Vector3 leftRayPoint = frontRayPoint;
        leftRayPoint.x += FieldOfView * 0.5f;

        Vector3 rightRayPoint = frontRayPoint;
        rightRayPoint.x -= FieldOfView * 0.5f;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);

        Debug.DrawLine(transform.position, leftRayPoint, Color.green);

        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}
