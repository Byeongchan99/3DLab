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

    // �ʱ�ȭ�� ���
    private void Start()
    {
        elapsedTime = 0.0f;
        Initialize();
    }

    // Update�� �����Ӵ� 1ȸ ȣ��ȴ�
    private void Update()
    {
        UpdateSense();
    }
}
