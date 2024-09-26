using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public float minVelocity = 1; // 최저 속도
    public float maxVelocity = 8; // 최고 군집 속력
    public int flockSize = 20; // 그룹 내에 있는 군집의 수

    // boid가 중앙에서 어느 정도까지 떨어질 수 있는지 지정(weight가 클수록 중앙에 근접)
    public float centerWeight = 1;

    public float velocityWeight = 1; // 정렬 동작

    // 군집 내에서 개별 boid 간의 거리
    public float separationWeight = 1;

    // 개별 boid와 리더 간의 거리(weight가 클수록 가깝게 따라감)
    public float followWeight = 1;

    // 추가적인 임의성 제공
    public float randomizeWeight = 1;

    public Flock prefab;
    public Transform target;

    // 그룹 내 군집의 중앙 위치
    internal Vector3 flockCenter;
    internal Vector3 flockVelocity; // 평균 속도

    public ArrayList flockList = new ArrayList();

    private void Start()
    {
        for (int i = 0; i < flockSize; i++)
        {
            Flock flock = Instantiate(prefab, transform.position, transform.rotation) as Flock;
            flock.transform.parent = transform;
            flock.controller = this;
            flockList.Add(flock);
        }
    }

    private void Update()
    {
        // 전체 군집 그룹의 중앙 위치와 속도를 계산한다
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        foreach (Flock flock in flockList)
        {
            center += flock.transform.localPosition;
            velocity += flock.rb.velocity;
        }

        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
