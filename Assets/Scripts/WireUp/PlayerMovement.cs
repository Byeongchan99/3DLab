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
    [SerializeField] private float moveSpeed; // 현재 이동 속도
    public float walkSpeed; // 걷기 속도   
    public float sprintSpeed; // 달리기 속도
    public float swingSpeed; // 스윙 속도
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
    public LayerMask GroundLayers; // 지면 레이어
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
        // 메인 카메라 참조
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

        AssignAnimationIDs(); // 애니메이션 ID 할당

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
            Debug.Log("맨틀 중");
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
            // 입력이 없고 경사로를 내려가고 있을 때 수직 속도를 0으로 설정하여 미끄러짐 방지
            if (isSlope && downDirection)
            {
                _verticalVelocity = 0f;
            }
        }

        // 입력 강도 조절 - 조이스틱 입력이 아닐 경우 1로 설정
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // calculate movement direction
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        moveDirection = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0) * inputDirection;

        // if there is a move input rotate player when the player is moving
        // 카메라에 따라 이동 방향 회전
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
            // 낙하 타임아웃 리셋
            _fallTimeoutDelta = FallTimeout;

            // 애니메이터 업데이트
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // 수직 속도 안정화
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

            // 점프 처리
            if (_input.jump && readyToJump && grounded)
            {
                exitingSlope = true;
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                readyToJump = false; // 점프 후 재점프 방지
                _jumpTimeoutDelta = JumpTimeout; // 쿨다운 타이머 초기화

                // 애니메이터 업데이트
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
            // 점프 타임아웃 리셋
            _jumpTimeoutDelta = JumpTimeout;

            // 낙하 타임아웃 감소 및 애니메이터 업데이트
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

            // 공중에서 점프 입력 무시
            _input.jump = false;
        }

        // 쿨다운 타이머 감소 및 ResetJump 호출
        if (!readyToJump)
        {
            if (_jumpTimeoutDelta <= 0.0f)
                ResetJump();
        }

        // 나중에 로프 관련 변수 추가
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

    /// <summary> Mantle 관련 변수 </summary>
    private int _animIDMantle;
    private bool _isMantling = false;
    public LayerMask mantleLayerMask;  // 맨틀 가능한 레이어 설정
    public float rayHeight = 0.9f;  // 트레이스 시작 높이
    public float maxReachDistance = 0.5f;  // 전방 트레이스 최대 거리
    public float maxLedgeHeight = 0.9f;  // 맨틀 가능한 장애물 최대 높이
    private Vector3 targetMantlePosition;

    public bool CanPerformMantle()
    {
        if (_isMantling)
        {
            Debug.Log("이미 맨틀하는 중");
            return false; // 이미 맨틀 중이면 맨틀 불가
        }

        //Debug.Log("맨틀 가능 여부 체크")

        // 1. 전방 트레이스: 캐릭터의 중심에서 약간 뒤쪽으로 시작해서 장애물 감지
        Vector3 rayStartCenter = transform.position + transform.forward * -0.1f + Vector3.up * rayHeight;
        Vector3 rayDirection = transform.forward;
        float verticalOffset = 0.1f;

        // 중심, 위, 아래 레이의 시작 지점 계산
        Vector3 rayStartUpper = rayStartCenter + Vector3.up * verticalOffset;
        Vector3 rayStartLower = rayStartCenter - Vector3.up * verticalOffset;

        Debug.DrawRay(rayStartCenter, rayDirection * maxReachDistance, Color.red, 0.5f);
        Debug.DrawRay(rayStartUpper, rayDirection * maxReachDistance, Color.blue, 0.5f);
        Debug.DrawRay(rayStartLower, rayDirection * maxReachDistance, Color.blue, 0.5f);

        bool hitDetected = false;
        RaycastHit hit;

        // 중심, 위, 아래 레이 중 하나라도 충돌하면 히트로 간주
        if (Physics.Raycast(rayStartCenter, rayDirection, out hit, maxReachDistance, mantleLayerMask) ||
            Physics.Raycast(rayStartUpper, rayDirection, out hit, maxReachDistance, mantleLayerMask) ||
            Physics.Raycast(rayStartLower, rayDirection, out hit, maxReachDistance, mantleLayerMask))
        {
            hitDetected = true;
        }

        if (hitDetected)
        {
            // 2. 충돌 지점에서 위로 이동한 후 아래 방향으로 트레이스
            Vector3 downwardRayStart = hit.point + Vector3.up * maxLedgeHeight + transform.forward * 0.2f;

            Debug.DrawRay(downwardRayStart, Vector3.down * maxLedgeHeight, Color.green, 0.5f);
            if (Physics.Raycast(downwardRayStart, Vector3.down, out RaycastHit downwardHit, maxLedgeHeight))
            {
                // 3. 표면이 평평한지 확인
                if (downwardHit.normal.y > 0.6f)
                {

                    // 4. 캡슐 충돌 검사로 충분한 공간이 있는지 확인(끼임 방지)
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
                        return true; // 맨틀 가능
                    }
                    /*
                    만약 캡슐 충돌 검사 때문에 맨틀 동작이 계속 실패한다면 아래 코드로 대체
                    targetMantlePosition = downwardHit.point;
                    return true;
                    */
                }
            }
        }

        return false; // 맨틀 불가
    }

    public void StartMantle()
    {
        Debug.Log("맨틀 시작");
        _isMantling = true;
        StartCoroutine(MantleMovement());
    }

    System.Collections.IEnumerator MantleMovement()
    {
        Debug.Log("맨틀 동작 실행 중");
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDMantle, true);
        }

        Vector3 startPosition = targetMantlePosition + (transform.up * -1f) + (transform.forward * -0.4f); // 시작 위치를 애니메이션에 맞게 강제로 이동
                                                                                                           //float duration = 0.833f;  // 맨틀 애니메이션의 길이(총 26프레임, 재생 속도 0.5배속 - 0.833초 * 2)
        float elapsedTime = 0f;

        // 0 ~ 17프레임동안 위로 1만큼 이동
        float firstPhaseDuration = 17f / 30f; // 17프레임 (30fps 기준 약 0.567초)
        Vector3 firstPhaseTarget = startPosition + transform.up * 1f;
        while (elapsedTime < firstPhaseDuration)
        {
            transform.position = Vector3.Lerp(startPosition, firstPhaseTarget, elapsedTime / firstPhaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 18 ~ 25프레임동안 앞으로 0.4만큼 이동
        float secondPhaseDuration = 8f / 30f; // 8프레임 (30fps 기준 약 0.267초)
        Vector3 secondPhaseTarget = firstPhaseTarget + transform.forward * 0.4f;
        elapsedTime = 0f;
        while (elapsedTime < secondPhaseDuration)
        {
            transform.position = Vector3.Lerp(firstPhaseTarget, secondPhaseTarget, elapsedTime / secondPhaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치로 설정
        transform.position = targetMantlePosition;

        _isMantling = false;
        _input.MantleInput(false);
        // 맨틀 후 캐릭터 상태를 기본 상태(Idle)로 전환
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDMantle, false);
        }
        yield return null;
    }
}
