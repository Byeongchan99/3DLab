using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAction : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public Camera cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float ropeMaxLength = 3f; // 로프 최대 길이 설정

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;
    private Vector3 anchorPoint;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("StartGrapple");
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            grappling = true;

            lr.positionCount = 2;
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
            lr.enabled = true;

            anchorPoint = grapplePoint;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        Vector3 directionToGrapplePoint = (grapplePoint - transform.position).normalized;
        float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);

        StartCoroutine(GrappleMovement(directionToGrapplePoint, distanceToGrapplePoint));
    }

    private IEnumerator GrappleMovement(Vector3 direction, float distance)
    {
        float currentDistance = 0f;

        while (currentDistance < distance && grappling && currentDistance < ropeMaxLength)
        {
            characterController.Move(direction * Time.deltaTime * 20f); // Adjust speed as needed
            currentDistance += direction.magnitude * Time.deltaTime * 20f;
            lr.SetPosition(0, gunTip.position);
            yield return null;
        }

        StopGrapple();
    }

    public void StopGrapple()
    {
        grappling = false;
        lr.positionCount = 0;
        lr.enabled = false;
        grapplingCdTimer = grapplingCd;
    }

    private void DrawRope()
    {
        if (grappling)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
