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
        // �÷��̾� ��ġ ã��
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update�� �����Ӵ� �� �� ȣ��ȴ�
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        // ���� ������ ������ �ð� �˻縦 �����Ѵ�
        if (elapsedTime >= detectionRate)
        {
            DetectAspect();
        }
    }

    void DetectAspect()
    {
        RaycastHit hit;

        // ���� ��ġ�κ��� �÷��̾� ��ġ���� ����
        rayDirection = playerTrans.position - transform.position;

        // �ΰ����� ĳ������ ���� ���Ϳ� �÷��̾�� �ΰ����� ĳ���� ������ ���� ���� ���� ������ �˻��Ѵ�
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            // �÷��̾ �þ߿� ���Դ��� �˻�
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    // Ư�� �˻�
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

        // �뷫���� �þ� ���� �ð�ȭ
        Vector3 leftRayPoint = frontRayPoint;
        leftRayPoint.x += FieldOfView * 0.5f;

        Vector3 rightRayPoint = frontRayPoint;
        rightRayPoint.x -= FieldOfView * 0.5f;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);

        Debug.DrawLine(transform.position, leftRayPoint, Color.green);

        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}
