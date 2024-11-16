using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAction : MonoBehaviour
{
    public Transform playerCenter;
    public Transform gunTip;
    public Camera aimModeCamera;
    public LayerMask isGrapplingObject;
    private RaycastHit hit;
    private LineRenderer lineRenderer;
    private float maxDistance = 100f;
    private bool isGrappling = false;
    private Vector3 spot;
    private CharacterController characterController;
    private Vector3 anchorPoint;
    private float ropeMaxLength = 3f; // ���� �ִ� ���� ����

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        characterController = playerCenter.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("StartGrapple");
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        // �߾ӿ��� ���̸� ����
        Ray ray = aimModeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // ����ĳ��Ʈ�� �߻��ϰ�, ����� ���� ó��
        if (Physics.Raycast(ray, out hit, maxDistance, isGrapplingObject))
        {
            isGrappling = true;

            spot = hit.point;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gunTip.position);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.enabled = true;

            anchorPoint = spot; // �Ŵ޸� ���� ����
        }
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    void DrawRope()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
            lineRenderer.SetPosition(1, spot);
        }
    }

    void FixedUpdate()
    {
        if (isGrappling)
        {
            ApplyHangingEffect();
        }
    }

    void ApplyHangingEffect()
    {
        // ���� ��ġ�� ��ǥ ���� ���� ���� ���
        Vector3 directionToAnchor = anchorPoint - playerCenter.position;
        float distanceToAnchor = directionToAnchor.magnitude;

        // ���� �ִ� ���̸� �ʰ����� �ʵ��� �̵� ����
        if (distanceToAnchor > ropeMaxLength)
        {
            directionToAnchor.Normalize();
            Vector3 movement = directionToAnchor * (distanceToAnchor - ropeMaxLength);
            characterController.Move(movement * Time.deltaTime);
        }
    }
}
