using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAction : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    public ThirdPersonControllerWithRigidbody characterController;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    public bool aimMode = false; // ���� ���
    private bool isGrappling;
    private SpringJoint joint;
    public Rigidbody rb;

    public float pullForce = 200f; // �ʿ信 ���� ���� ������ �� ������/������ �̵�

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && aimMode == true && isGrappling == false)
        {
            Debug.Log("StartGrapple");
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;

        // �߰��� �׷��ø� ���߱�
        if (Input.GetMouseButtonUp(0) && isGrappling)
        {
            StopGrapple();
        }
    }

    private void FixedUpdate()
    {
        // FixedUpdate���� �׷��ø� ���� ����
        if (isGrappling)
        {
            ApplyGrapplingForce();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0)
            return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            // SpringJoint�� �÷��̾ �߰�
            joint = rb.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(rb.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
            lr.enabled = true;

            isGrappling = true;

            // ExecuteGrapple�� FixedUpdate���� ���������� ó���ϵ��� ����
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        // ExecuteGrapple�� �ʱ� �������� �ʿ��ϰ�,
        // �������� �� ������ FixedUpdate���� ó���մϴ�.
        if (joint == null) return;

        // �׷��ø� ���� �������� �ణ�� ������ �� �� ���� ���������� ����
        isGrappling = true;
    }

    private void ApplyGrapplingForce()
    {
        // �÷��̾� Rigidbody�� ���� ����
        if (rb != null && isGrappling)
        {
            Vector3 directionToGrapplePoint = (grapplePoint - rb.position).normalized;

            // ��ǥ �������� ĳ���͸� ���� ���� ������ ���� ����         
            rb.AddForce(directionToGrapplePoint * pullForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    public void StopGrapple()
    {
        isGrappling = false;

        // SpringJoint ����
        if (joint != null)
        {
            Destroy(joint);
        }

        lr.positionCount = 0;
        lr.enabled = false;

        grapplingCdTimer = grapplingCd;
    }

    private void DrawRope()
    {
        if (isGrappling)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    public bool IsGrappling()
    {
        return isGrappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
