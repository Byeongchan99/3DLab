using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFlock : MonoBehaviour
{
    public float minSpeed = 20.0f; // 최소 이동 속도
    public float turnSpeed = 20.0f; // 회전 속도
    public float randomFreq = 20.0f; // 랜덤 방향 전환 주기
    public float randomForce = 20.0f; // 랜덤 방향 전환 세기

    // 정렬 관련 변수
    public float toOriginForce = 50.0f; // 군집의 중심으로 이동하는 힘
    public float toOriginRange = 100.0f; // 군집의 흩어짐 정도(범위)

    public float gravity = 2.0f;

    // 분산 관련 변수 - 개별 개체 간의 최소 거리를 유지하기 위한 변수
    public float avoidanceRadius = 50.0f; // 회피 반경
    public float avoidanceForce = 20.0f;

    // 응집 관련 변수 - 군집의 리더 또는 군집의 중심 위치와의 최소 거리를 유지하기 위한 변수
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;

    // 개별 개체의 이동과 관련된 변수
    private Transform origin; // 군집 오브젝트 전체 그룹을 제어하는 부모 오브젝트, 군집의 중심 역할
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    // 이웃 boid들의 정보 저장
    private Transform[] objects;
    private UnityFlock[] otherFlocks;
    private Transform transformComponent;

    private void Start()
    {
        randomFreq = 1.0f / randomFreq;

        // parent 오브젝트를 찾아서 origin 변수에 할당
        origin = transform.parent;

        // 군집 트랜스폼
        transformComponent = transform;

        // 임시 컴포넌트
        Component[] tempFlocks = null;

        // 그룹 내의 부모 트랜스폼으로부터 모든 유니티 군집 컴포넌트를 얻음
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
        }

        // 그룹 내의 모든 군집 오브젝트를 할당하고 저장
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new UnityFlock[tempFlocks.Length];

        for (int i = 0; i < tempFlocks.Length; i++)
        {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (UnityFlock)tempFlocks[i];
        }

        // parent에 null을 지정하면 UnityFlockController 오브젝트가 리더가 된다
        transform.parent = null;

        // 주어진 랜덤 주기에 따라 랜덤 푸시를 계산한다
        StartCoroutine(UpdateRandom());
    }

    private void Update()
    {
        // 내부 변수
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

        // 현재 boid와 다른 boid 사이의 거리를 검사하고 속도를 그에 맞춰 갱신
        for (int i = 0; i < objects.Length; i++) 
        {
            Transform transform = objects[i];
            if (transform != transformComponent) { 
                Vector3 otherPosition = transform.position;

                // 응집을 계산하기 위한 평균 위치
                avgPosition += otherPosition;
                count++;

                // 다른 군집에서 이 군집까지의 방향 벡터
                forceV = myPosition - otherPosition;

                // 방향 벡터의 크기(길이)
                d = forceV.magnitude;

                // 만약 벡터의 길이가 followRadius보다 작다면 값을 늘림
                if (d < followRadius)
                {
                    // 현재 벡터의 길이가 지정된 회피 반경보다 작으면
                    // 무리 간의 회피 거리에 기반해 오브젝트의 속도를 계산
                    if (d < avoidanceRadius)
                    {
                        f = 1.0f - (d / avoidanceRadius);
                        if (d > 0) 
                            avgVelocity += (forceV / d) * f * avoidanceForce;
                    }

                    // 리더와의 현재 거리를 유지
                    f = d / followRadius;
                    UnityFlock otherSealgull = otherFlocks[i];
                    // otherSealgull 속도 벡터를 정규화해 이동 방향을 얻은 후, 새로운 속도를 설정
                    avgVelocity += otherSealgull.normalizedVelocity * f * followVelocity;
                }            
            }
        }

        // 군집의 평균 위치와 속도를 계산
        if (count > 0)
        {
            // 군집의 평균 속도를 계산(정렬)
            avgVelocity /= count;

            // 군집의 중간 값을 계산(응집)
            toAvg = (avgPosition / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }

        // 리더를 향한 방향 벡터
        forceV = origin.position - myPosition;
        d = forceV.magnitude;
        f = d / toOriginRange;

        // 리더에 대한 군집의 속도를 계산
        if (d > 0) // 만약 boid가 무리의 중심에 있지 않다면
            originPush = (forceV / d) * f * toOriginForce;

        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }

        wantedVel = velocity;

        // 최종 속도 계산
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime; // UpdateRandom() 코루틴에서 계산된 랜덤 벡터
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;

        // 무리를 회전시키기 위한 최종 속도 계산
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.0f);

        transformComponent.rotation = Quaternion.LookRotation(velocity);

        // 계산한 속도에 기반해 군집 이동
        transformComponent.Translate(velocity * Time.deltaTime, Space.World);

        // 속도 정상화
        normalizedVelocity = velocity.normalized;
    }

    // randomFreq 변수의 시간 간격에 기반해 randomPush 값을 갱신
    // 랜덤으로 방향을 바꾸는 함수
    IEnumerator UpdateRandom()
    {
        while (true)
        {
            // randomForce를 반지름으로 하는 구 내에서 임의의 x, y, z 값으로 Vector3 오브젝트를 반환
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreq + Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
        }
    }
}
