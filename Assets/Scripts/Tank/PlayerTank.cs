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
        // Ÿ�� ��ġ ��ó�� �����ϸ� �ϴ� ����
        if (Vector3.Distance(targetTransform.position, transform.position) < 5.0f)
        {
            return;
        }

        // ���� ��ġ�κ��� Ÿ�� ��ġ���� ���� ���� ���
        Vector3 tarPos = targetTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        // LookRotation �޼��带 ����� �� ���ο� ȸ�� ���͸� ���� Quaternion ����
        Quaternion tarRot = Quaternion.LookRotation(dirRot);

        // �������� ����ؼ� �̵��ϰ� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }
}
