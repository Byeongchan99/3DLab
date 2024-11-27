using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using StarterAssets;
using UnityEngine.InputSystem.XR;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed; // ���� �̵� �ӵ�
    public float walkSpeed; // �ȱ� �ӵ�   
    public float sprintSpeed; // �޸��� �ӵ�
    public float swingSpeed; // ���� �ӵ�
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    [SerializeField] private float _verticalVelocity;
    private float _maxFallVelocity = -15.0f;

    public float groundDrag;

    [Header("Jumping")]
    public float JumpHeight = 1.2f;
    //public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public float Gravity = -15.0f;

    /*
    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    */

    [Header("Ground Check")]
    public Transform playerCenter;
    public float playerHeight;
    public LayerMask GroundLayers; // ���� ���̾�
    [SerializeField] bool grounded;
    public float groundCheckBoxSize = 0.1f;

    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    [SerializeField] private bool exitingSlope;
    public bool isSlope;
    public bool upDirection;
    public bool downDirection;

    [Header("Audio")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Header("Camera")]
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;

    // cinemachine
    public GameObject CinemachineCameraTarget;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [Header("Camera Effects")]
    //public PlayerCam cam;
    public float grappleFov = 95f;

    //public Transform orientation;

    //float horizontalInput;
    //float verticalInput;

    Vector3 moveDirection;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        grappling,
        swinging,
        walking,
        sprinting,
        //crouching,
        mantling,
        air
    }

    public bool freeze;

    public bool activeGrapple;
    public bool swinging;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    //
    private PlayerInput _playerInput;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private StarterAssetsInputs _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;

    private void Awake()
    {
        // ���� ī�޶� ����
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        readyToJump = true;

        _hasAnimator = TryGetComponent(out _animator);
        _input = GetComponent<StarterAssetsInputs>();

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        //startYScale = transform.localScale.y;

        AssignAnimationIDs(); // �ִϸ��̼� ID �Ҵ�

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    public void IsGrounded()
    {
        Vector3 boxSize = new Vector3(0.1f, 0.1f, 0.1f);
        grounded = Physics.CheckBox(transform.position, boxSize, Quaternion.identity, GroundLayers);
    }

    private void Update()
    {
        // ground check 
        //grounded = Physics.CheckBox(transform.position, boxSize, Quaternion.identity, GroundLayers);
        //Physics.Raycast(playerCenter.position, Vector3.down, playerHeight * 0.5f + 0.05f, GroundLayers));
        //grounded = Physics.CheckCapsule(transform.position, transform.position, groundCheckBoxSize, GroundLayers);
           
        //PlayerInput();
        //MovePlayer();
        //Jump();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded && !activeGrapple)
            _rigidbody.drag = groundDrag;
        else
            _rigidbody.drag = 0;

        //TextStuff();

        if (_animator)
        {
            _animator.SetBool(_animIDGrounded, grounded);
        }
    }

    private void FixedUpdate()
    {
        IsGrounded();
        CheckSlope();

        if (_isMantling)
        {
            Debug.Log("��Ʋ ��");
            return;
        }

        MovePlayer();
        Jump();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDMantle = Animator.StringToHash("Mantle");
    }

    /*
    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // when to jump
        if (_input.jump && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            //Invoke(nameof(ResetJump), jumpCooldown);
        }
       
        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    */

    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            _rigidbody.velocity = Vector3.zero;
        }

        // Mode - Grappling
        else if (activeGrapple)
        {
            state = MovementState.grappling;
            moveSpeed = sprintSpeed;
        }

        // Mode - Swinging
        else if (swinging)
        {
            state = MovementState.swinging;
            moveSpeed = swingSpeed;
        }

        // Mode - Mantling
        else if (_input.mantle)
        { 
            state = MovementState.mantling;
        }

        /*
        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        */

        // Mode - Sprinting
        else if (grounded && _input.sprint)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
            if (_input.sprint)
                moveSpeed = sprintSpeed * airMultiplier;
            else
                moveSpeed = walkSpeed * airMultiplier;
        }
    }

    private void CheckSlope()
    {
        // turn gravity off while on slope
        if (OnSlope())
        {
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();

            if (slopeMoveDirection.y > 0)
            {
                upDirection = true;
                downDirection = false;
            }
            else
            {
                downDirection = true;
                upDirection = false;
            }

            //Gravity = -30f;
            isSlope = true;
            _rigidbody.useGravity = false;
        }
        else
        {
            //Gravity = -15.0f;
            isSlope = false;
            _rigidbody.useGravity = true;
        }
    }

    private void MovePlayer()
    {
        if (activeGrapple) return;
        if (swinging) return;

        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) { 
            moveSpeed = 0.0f;
            // �Է��� ���� ���θ� �������� ���� �� ���� �ӵ��� 0���� �����Ͽ� �̲����� ����
            if (isSlope && downDirection)
            {
                _verticalVelocity = 0f;
            }
        }

        // �Է� ���� ���� - ���̽�ƽ �Է��� �ƴ� ��� 1�� ����
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // calculate movement direction
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        moveDirection = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0) * inputDirection;

        // if there is a move input rotate player when the player is moving
        // ī�޶� ���� �̵� ���� ȸ��
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            /*
            _rigidbody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (_rigidbody.velocity.y > 0)
                _rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            */

            _rigidbody.velocity = GetSlopeMoveDirection() * moveSpeed * inputMagnitude;
        }

        // on ground
        else if (grounded)
        {
            //_rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            // move the player
            _rigidbody.velocity = targetDirection.normalized * (moveSpeed * inputMagnitude) + new Vector3(0.0f, _rigidbody.velocity.y, 0.0f);
        }

        // in air
        else if (!grounded)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            // move the player
            _rigidbody.velocity = targetDirection.normalized * (moveSpeed * inputMagnitude) + new Vector3(0.0f, _rigidbody.velocity.y, 0.0f) * airMultiplier;
        }

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, moveSpeed);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void SpeedControl()
    {
        if (activeGrapple) return;

        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (_rigidbody.velocity.magnitude > moveSpeed)
                _rigidbody.velocity = _rigidbody.velocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                _rigidbody.velocity = new Vector3(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        if (grounded)
        {
            // ���� Ÿ�Ӿƿ� ����
            _fallTimeoutDelta = FallTimeout;

            // �ִϸ����� ������Ʈ
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // ���� �ӵ� ����ȭ
            if (_verticalVelocity < 0.0f)
            {
                /*
                if (isSlope)
                    _verticalVelocity = 0f;
                else 
                    _verticalVelocity = -2f;
                */
                if (isSlope)
                {
                    if (upDirection)
                    {
                        _verticalVelocity = 0f;
                    }
                    else if (downDirection)
                    {
                        _verticalVelocity = -3f;
                    }
                }
                else
                {
                    _verticalVelocity = 0f;
                }
            }

            // ���� ó��
            if (_input.jump && readyToJump && grounded)
            {
                exitingSlope = true;
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                readyToJump = false; // ���� �� ������ ����
                _jumpTimeoutDelta = JumpTimeout; // ��ٿ� Ÿ�̸� �ʱ�ȭ

                // �ִϸ����� ������Ʈ
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // ���� Ÿ�Ӿƿ� ����
            _jumpTimeoutDelta = JumpTimeout;

            // ���� Ÿ�Ӿƿ� ���� �� �ִϸ����� ������Ʈ
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

            // ���߿��� ���� �Է� ����
            _input.jump = false;
        }

        // ��ٿ� Ÿ�̸� ���� �� ResetJump ȣ��
        if (!readyToJump)
        {
            if (_jumpTimeoutDelta <= 0.0f)
                ResetJump();
        }

        // ���߿� ���� ���� ���� �߰�
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity > _maxFallVelocity && !grounded)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _verticalVelocity, _rigidbody.velocity.z);

        /*
        exitingSlope = true;

         reset y velocity
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

        _rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        */
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void CameraRotation()
    {
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = 1.0f;
            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        _rigidbody.velocity = velocityToSet;

        //cam.DoFov(grappleFov);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        //cam.DoFov(85f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    void OnDrawGizmos()
    {       
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawCube(transform.position, new Vector3(0.2f, 0.1f, 0.2f));
    }


    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, FootstepAudioVolume);
        }
    }

    /*
    #region Text & Debugging

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_mode;
    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (OnSlope())
            text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1) + " / " + Round(moveSpeed, 1));

        else
            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));

        text_mode.SetText(state.ToString());
    }
    

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    #endregion
    */

    /// <summary> Mantle ���� ���� </summary>
    private int _animIDMantle;
    private bool _isMantling = false;
    public LayerMask mantleLayerMask;  // ��Ʋ ������ ���̾� ����
    public float rayHeight = 0.9f;  // Ʈ���̽� ���� ����
    public float maxReachDistance = 0.5f;  // ���� Ʈ���̽� �ִ� �Ÿ�
    public float maxLedgeHeight = 0.9f;  // ��Ʋ ������ ��ֹ� �ִ� ����
    private Vector3 targetMantlePosition;

    public bool CanPerformMantle()
    {
        if (_isMantling)
        {
            Debug.Log("�̹� ��Ʋ�ϴ� ��");
            return false; // �̹� ��Ʋ ���̸� ��Ʋ �Ұ�
        }

        //Debug.Log("��Ʋ ���� ���� üũ")

        // 1. ���� Ʈ���̽�: ĳ������ �߽ɿ��� �ణ �������� �����ؼ� ��ֹ� ����
        Vector3 rayStartCenter = transform.position + transform.forward * -0.1f + Vector3.up * rayHeight;
        Vector3 rayDirection = transform.forward;
        float verticalOffset = 0.1f;

        // �߽�, ��, �Ʒ� ������ ���� ���� ���
        Vector3 rayStartUpper = rayStartCenter + Vector3.up * verticalOffset;
        Vector3 rayStartLower = rayStartCenter - Vector3.up * verticalOffset;

        Debug.DrawRay(rayStartCenter, rayDirection * maxReachDistance, Color.red, 0.5f);
        Debug.DrawRay(rayStartUpper, rayDirection * maxReachDistance, Color.blue, 0.5f);
        Debug.DrawRay(rayStartLower, rayDirection * maxReachDistance, Color.blue, 0.5f);

        bool hitDetected = false;
        RaycastHit hit;

        // �߽�, ��, �Ʒ� ���� �� �ϳ��� �浹�ϸ� ��Ʈ�� ����
        if (Physics.Raycast(rayStartCenter, rayDirection, out hit, maxReachDistance, mantleLayerMask) ||
            Physics.Raycast(rayStartUpper, rayDirection, out hit, maxReachDistance, mantleLayerMask) ||
            Physics.Raycast(rayStartLower, rayDirection, out hit, maxReachDistance, mantleLayerMask))
        {
            hitDetected = true;
        }

        if (hitDetected)
        {
            // 2. �浹 �������� ���� �̵��� �� �Ʒ� �������� Ʈ���̽�
            Vector3 downwardRayStart = hit.point + Vector3.up * maxLedgeHeight + transform.forward * 0.2f;

            Debug.DrawRay(downwardRayStart, Vector3.down * maxLedgeHeight, Color.green, 0.5f);
            if (Physics.Raycast(downwardRayStart, Vector3.down, out RaycastHit downwardHit, maxLedgeHeight))
            {
                // 3. ǥ���� �������� Ȯ��
                if (downwardHit.normal.y > 0.6f)
                {

                    // 4. ĸ�� �浹 �˻�� ����� ������ �ִ��� Ȯ��(���� ����)
                    Vector3 capsulePosition = downwardHit.point;
                    float capsuleHeight = 1.8f;
                    float capsuleRadius = 0.2f;

                    bool hasRoom = !Physics.CheckCapsule(
                        capsulePosition + Vector3.up * capsuleRadius,
                        capsulePosition + Vector3.up * (capsuleHeight - capsuleRadius),
                        0.05f,
                        mantleLayerMask
                    );

                    if (hasRoom)
                    {
                        targetMantlePosition = downwardHit.point;
                        return true; // ��Ʋ ����
                    }
                    /*
                    ���� ĸ�� �浹 �˻� ������ ��Ʋ ������ ��� �����Ѵٸ� �Ʒ� �ڵ�� ��ü
                    targetMantlePosition = downwardHit.point;
                    return true;
                    */
                }
            }
        }

        return false; // ��Ʋ �Ұ�
    }

    public void StartMantle()
    {
        Debug.Log("��Ʋ ����");
        _isMantling = true;
        StartCoroutine(MantleMovement());
    }

    System.Collections.IEnumerator MantleMovement()
    {
        Debug.Log("��Ʋ ���� ���� ��");
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDMantle, true);
        }

        Vector3 startPosition = targetMantlePosition + (transform.up * -1f) + (transform.forward * -0.4f); // ���� ��ġ�� �ִϸ��̼ǿ� �°� ������ �̵�
                                                                                                           //float duration = 0.833f;  // ��Ʋ �ִϸ��̼��� ����(�� 26������, ��� �ӵ� 0.5��� - 0.833�� * 2)
        float elapsedTime = 0f;

        // 0 ~ 17�����ӵ��� ���� 1��ŭ �̵�
        float firstPhaseDuration = 17f / 30f; // 17������ (30fps ���� �� 0.567��)
        Vector3 firstPhaseTarget = startPosition + transform.up * 1f;
        while (elapsedTime < firstPhaseDuration)
        {
            transform.position = Vector3.Lerp(startPosition, firstPhaseTarget, elapsedTime / firstPhaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 18 ~ 25�����ӵ��� ������ 0.4��ŭ �̵�
        float secondPhaseDuration = 8f / 30f; // 8������ (30fps ���� �� 0.267��)
        Vector3 secondPhaseTarget = firstPhaseTarget + transform.forward * 0.4f;
        elapsedTime = 0f;
        while (elapsedTime < secondPhaseDuration)
        {
            transform.position = Vector3.Lerp(firstPhaseTarget, secondPhaseTarget, elapsedTime / secondPhaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ�� ����
        transform.position = targetMantlePosition;

        _isMantling = false;
        _input.MantleInput(false);
        // ��Ʋ �� ĳ���� ���¸� �⺻ ����(Idle)�� ��ȯ
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDMantle, false);
        }
        yield return null;
    }
}
