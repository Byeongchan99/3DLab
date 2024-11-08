using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAvoidance : MonoBehaviour
{
    public float speed = 20.0f;
    public float mass = 5.0f;
    public float force = 50.0f;
    public float minimumDistToAvoid = 20.0f;

    // 차량의 실제 속도
    private float curSpeed;
    private Vector3 targetPoint;

    // 초기화 수행
    private void Start()
    {
        mass = 5.0f;
        targetPoint = Vector3.zero;
    }

    private void OnGUI()
    {
        GUILayout.Label("Click anyswhere to move the vehicle");
    }

    // 매 프레임 Update 호출됨
    private void Update()
    {
        // 차량은 마우스 클릭으로 이동
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100.0f))
        {
            targetPoint = hit.point;
        }

        // 목표 지점을 향하는 방향 벡터
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();

        // 장애물 회피 적용
        AvoidObstacles(ref dir);

        // 목표 지점에 도착하면 차량을 멈춘다
        if (Vector3.Distance(targetPoint, transform.position) < 3.0f)
            return;

        // 속도에 델타 타임을 적용한다
        curSpeed = speed * Time.deltaTime;

        // 목표 방향 벡터로 차량을 회전시킨다
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        // 차량을 전진시킨다
        transform.position += transform.forward * curSpeed;
    }

    // 장애물 회피를 위해 새 방향 벡터를 계산
    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;

        // 레이어 8(Obstacles)만 검사
        int layerMask = 1 << 8;

        // 회피 최소거리 이내에서 장애물과 차량이 충돌했는지 검사 수행
        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMask))
        {
            // 새 방향을 계산하기 위해 충돌 지점에서 법선을 구한다
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;

            // 차량의 현재 전방 벡터에 force를 더해 새로운 방향 벡터를 얻는다
            dir = transform.forward + hitNormal * force;
        }
    }
}
