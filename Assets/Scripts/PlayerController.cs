using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent; // 플레이어의 NavMeshAgent 컴포넌트에 대한 참조

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭 감지
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라에서 마우스 위치로 광선(Ray) 발사
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // 광선이 어떤 오브젝트에 닿았는지 확인
            {
                agent.SetDestination(hit.point); // NavMeshAgent의 목적지를 클릭한 위치로 설정
            }
        }
    }
}
