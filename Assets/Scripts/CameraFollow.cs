using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public Vector3 offset; // �÷��̾�� ī�޶� ������ ������(�Ÿ�)

    void LateUpdate()
    {
        // ī�޶��� ��ġ�� �÷��̾��� ��ġ + ���������� ����
        transform.position = player.position + offset;
    }
}
