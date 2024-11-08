using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAvoidance : MonoBehaviour
{
    public float speed = 20.0f;
    public float mass = 5.0f;
    public float force = 50.0f;
    public float minimumDistToAvoid = 20.0f;

    // ������ ���� �ӵ�
    private float curSpeed;
    private Vector3 targetPoint;

    // �ʱ�ȭ ����
    private void Start()
    {
        mass = 5.0f;
        targetPoint = Vector3.zero;
    }

    private void OnGUI()
    {
        GUILayout.Label("Click anyswhere to move the vehicle");
    }

    // �� ������ Update ȣ���
    private void Update()
    {
        // ������ ���콺 Ŭ������ �̵�
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100.0f))
        {
            targetPoint = hit.point;
        }

        // ��ǥ ������ ���ϴ� ���� ����
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();

        // ��ֹ� ȸ�� ����
        AvoidObstacles(ref dir);

        // ��ǥ ������ �����ϸ� ������ �����
        if (Vector3.Distance(targetPoint, transform.position) < 3.0f)
            return;

        // �ӵ��� ��Ÿ Ÿ���� �����Ѵ�
        curSpeed = speed * Time.deltaTime;

        // ��ǥ ���� ���ͷ� ������ ȸ����Ų��
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        // ������ ������Ų��
        transform.position += transform.forward * curSpeed;
    }

    // ��ֹ� ȸ�Ǹ� ���� �� ���� ���͸� ���
    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;

        // ���̾� 8(Obstacles)�� �˻�
        int layerMask = 1 << 8;

        // ȸ�� �ּҰŸ� �̳����� ��ֹ��� ������ �浹�ߴ��� �˻� ����
        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMask))
        {
            // �� ������ ����ϱ� ���� �浹 �������� ������ ���Ѵ�
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;

            // ������ ���� ���� ���Ϳ� force�� ���� ���ο� ���� ���͸� ��´�
            dir = transform.forward + hitNormal * force;
        }
    }
}
