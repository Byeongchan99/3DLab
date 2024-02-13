using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent; // �÷��̾��� NavMeshAgent ������Ʈ�� ���� ����

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư Ŭ�� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ī�޶󿡼� ���콺 ��ġ�� ����(Ray) �߻�
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // ������ � ������Ʈ�� ��Ҵ��� Ȯ��
            {
                agent.SetDestination(hit.point); // NavMeshAgent�� �������� Ŭ���� ��ġ�� ����
            }
        }
    }
}
