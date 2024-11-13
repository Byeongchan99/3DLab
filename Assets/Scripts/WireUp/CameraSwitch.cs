using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera normalCam;  // �⺻ ��� ī�޶�
    public CinemachineVirtualCamera aimCam;     // ���� ��� ī�޶�

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ������ ���콺 ��ư�� ������ ���� ���
        {
            aimCam.Priority = 20;  // ���� ī�޶� �켱������ ���� Ȱ��ȭ
            normalCam.Priority = 10;  // �Ϲ� ī�޶� �켱������ ����
        }
        else if (Input.GetMouseButtonUp(1)) // ��ư�� ������ �⺻ ���� ���ư�
        {
            aimCam.Priority = 10;  // ���� ī�޶� �켱������ ����
            normalCam.Priority = 20;  // �Ϲ� ī�޶� �켱������ ���� Ȱ��ȭ
        }
    }
}
