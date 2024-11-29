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
    public PlayerMovement characterController;

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

    public bool aimMode = false; // 조준 모드
    private SpringJoint joint;
    public Rigidbody rb;

    public float pullForce = 200f; // 필요에 따라 값을 조정해 더 빠르게/느리게 이동

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && aimMode == true && characterController.swinging == false)
        {
            Debug.Log("StartGrapple");
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;

        // 중간에 그래플링 멈추기
        if (Input.GetMouseButtonUp(0) && characterController.swinging)
        {
            StopGrapple();
        }
    }

    private void FixedUpdate()
    {
        // FixedUpdate에서 그래플링 힘을 적용
        if (characterController.swinging)
        {
            //ApplyGrapplingForce();
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

            // SpringJoint를 플레이어에 추가
            joint = rb.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(rb.position, grapplePoint);

            /*
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            */

            joint.maxDistance = distanceFromPoint;
            joint.minDistance = distanceFromPoint;

            joint.spring = 5f;
            joint.damper = 50f;
            joint.massScale = 1f;

            lr.positionCount = 2;
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
            lr.enabled = true;

            characterController.swinging = true;

            // ExecuteGrapple을 FixedUpdate에서 지속적으로 처리하도록 설정
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }       
    }

    private void ExecuteGrapple()
    {
        // ExecuteGrapple은 초기 설정에만 필요하고,
        // 지속적인 힘 적용은 FixedUpdate에서 처리합니다.
        if (joint == null) return;

        // 그래플링 시작 지점에서 약간의 지연을 둔 후 힘을 지속적으로 적용
        characterController.swinging = true;
    }

    /*
    private void ApplyGrapplingForce()
    {
        // 플레이어 Rigidbody에 힘을 가함
        if (rb != null && isGrappling)
        {
            Vector3 directionToGrapplePoint = (grapplePoint - rb.position).normalized;

            // 목표 지점으로 캐릭터를 당기기 위해 적절한 힘을 가함         
            rb.AddForce(directionToGrapplePoint * pullForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
    */

    public void StopGrapple()
    {
        characterController.swinging = false;

        // SpringJoint 제거
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
        if (characterController.swinging)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    public bool IsGrappling()
    {
        return characterController.swinging;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
