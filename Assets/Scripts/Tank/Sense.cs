using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    public bool bDebug = true;
    public Aspect.aspect aspectName = Aspect.aspect.Enemy;
    public float detectionRate = 1.0f;

    protected float elapsedTime = 0.0f;

    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }

    // 초기화에 사용
    private void Start()
    {
        elapsedTime = 0.0f;
        Initialize();
    }

    // Update는 프레임당 1회 호출된다
    private void Update()
    {
        UpdateSense();
    }
}
