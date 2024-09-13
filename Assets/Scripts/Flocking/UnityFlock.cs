using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFlock : MonoBehaviour
{
    public float minSpeed = 20.0f; // �ּ� �̵� �ӵ�
    public float turnSpeed = 20.0f; // ȸ�� �ӵ�
    public float randomFreq = 20.0f; // ���� ���� ��ȯ �ֱ�
    public float randomForce = 20.0f; // ���� ���� ��ȯ ����

    // ���� ���� ����
    public float toOriginForce = 50.0f; // ������ �߽����� �̵��ϴ� ��
    public float toOriginRange = 100.0f; // ������ ����� ����(����)

    public float gravity = 2.0f;

    // �л� ���� ���� - ���� ��ü ���� �ּ� �Ÿ��� �����ϱ� ���� ����
    public float avoidanceRadius = 50.0f; // ȸ�� �ݰ�
    public float avoidanceForce = 20.0f;

    // ���� ���� ���� - ������ ���� �Ǵ� ������ �߽� ��ġ���� �ּ� �Ÿ��� �����ϱ� ���� ����
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;

    // ���� ��ü�� �̵��� ���õ� ����
    private Transform origin; // ���� ������Ʈ ��ü �׷��� �����ϴ� �θ� ������Ʈ, ������ �߽� ����
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    // �̿� boid���� ���� ����
    private Transform[] objects;
    private UnityFlock[] otherFlocks;
    private Transform transformComponent;

    private void Start()
    {
        randomFreq = 1.0f / randomFreq;

        // parent ������Ʈ�� ã�Ƽ� origin ������ �Ҵ�
        origin = transform.parent;

        // ���� Ʈ������
        transformComponent = transform;

        // �ӽ� ������Ʈ
        Component[] tempFlocks = null;

        // �׷� ���� �θ� Ʈ���������κ��� ��� ����Ƽ ���� ������Ʈ�� ����
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
        }

        // �׷� ���� ��� ���� ������Ʈ�� �Ҵ��ϰ� ����
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new UnityFlock[tempFlocks.Length];

        for (int i = 0; i < tempFlocks.Length; i++)
        {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (UnityFlock)tempFlocks[i];
        }

        // parent�� null�� �����ϸ� UnityFlockController ������Ʈ�� ������ �ȴ�
        transform.parent = null;

        // �־��� ���� �ֱ⿡ ���� ���� Ǫ�ø� ����Ѵ�
        StartCoroutine(UpdateRandom());
    }

    private void Update()
    {
        // ���� ����
        float speed = velocity.magnitude;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        float count = 0;
        float f = 0.0f;
        float d = 0.0f;
        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;
        Vector3 wantedVel;

        // ���� boid�� �ٸ� boid ������ �Ÿ��� �˻��ϰ� �ӵ��� �׿� ���� ����
        for (int i = 0; i < objects.Length; i++) 
        {
            Transform transform = objects[i];
            if (transform != transformComponent) { 
                Vector3 otherPosition = transform.position;

                // ������ ����ϱ� ���� ��� ��ġ
                avgPosition += otherPosition;
                count++;

                // �ٸ� �������� �� ���������� ���� ����
                forceV = myPosition - otherPosition;

                // ���� ������ ũ��(����)
                d = forceV.magnitude;

                // ���� ������ ���̰� followRadius���� �۴ٸ� ���� �ø�
                if (d < followRadius)
                {
                    // ���� ������ ���̰� ������ ȸ�� �ݰ溸�� ������
                    // ���� ���� ȸ�� �Ÿ��� ����� ������Ʈ�� �ӵ��� ���
                    if (d < avoidanceRadius)
                    {
                        f = 1.0f - (d / avoidanceRadius);
                        if (d > 0) 
                            avgVelocity += (forceV / d) * f * avoidanceForce;
                    }

                    // �������� ���� �Ÿ��� ����
                    f = d / followRadius;
                    UnityFlock otherSealgull = otherFlocks[i];
                    // otherSealgull �ӵ� ���͸� ����ȭ�� �̵� ������ ���� ��, ���ο� �ӵ��� ����
                    avgVelocity += otherSealgull.normalizedVelocity * f * followVelocity;
                }            
            }
        }

        // ������ ��� ��ġ�� �ӵ��� ���
        if (count > 0)
        {
            // ������ ��� �ӵ��� ���(����)
            avgVelocity /= count;

            // ������ �߰� ���� ���(����)
            toAvg = (avgPosition / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }

        // ������ ���� ���� ����
        forceV = origin.position - myPosition;
        d = forceV.magnitude;
        f = d / toOriginRange;

        // ������ ���� ������ �ӵ��� ���
        if (d > 0) // ���� boid�� ������ �߽ɿ� ���� �ʴٸ�
            originPush = (forceV / d) * f * toOriginForce;

        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }

        wantedVel = velocity;

        // ���� �ӵ� ���
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime; // UpdateRandom() �ڷ�ƾ���� ���� ���� ����
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;

        // ������ ȸ����Ű�� ���� ���� �ӵ� ���
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.0f);

        transformComponent.rotation = Quaternion.LookRotation(velocity);

        // ����� �ӵ��� ����� ���� �̵�
        transformComponent.Translate(velocity * Time.deltaTime, Space.World);

        // �ӵ� ����ȭ
        normalizedVelocity = velocity.normalized;
    }

    // randomFreq ������ �ð� ���ݿ� ����� randomPush ���� ����
    // �������� ������ �ٲٴ� �Լ�
    IEnumerator UpdateRandom()
    {
        while (true)
        {
            // randomForce�� ���������� �ϴ� �� ������ ������ x, y, z ������ Vector3 ������Ʈ�� ��ȯ
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreq + Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
        }
    }
}
