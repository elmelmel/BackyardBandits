﻿using System.Numerics;
using GameEvents;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        public enum PlayerNames
        {
            Kit_small,
            Cub_big
        }

        [Header("Player")]
        public PlayerNames playerName;
        
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        public float RotationSpeed = 3.0f;

        //[Tooltip("Sprint speed of the character in m/s")]
        //public float SprintSpeed = 5.335f;

        public float ClimbingSpeed = 1.3f;
        public bool OnPipe = false;
        
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.5f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField] public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        public Animator playerAnimator;

        private BasicRigidBodyPush pushScript;
        
        //Animation States
        private const string PLAYER_IDLE = "Player_idle";
        private const string PLAYER_WALK = "Player_walk";
        private const string PLAYER_JUMP = "Player_jump";
        private const string PLAYER_FALL = "Player_fall";
        private const string PLAYER_LAND = "Player_land";
        private const string PLAYER_PULL = "Player_pull";
        private const string PLAYER_PUSH = "Player_push";
        private const string PLAYER_CLIMB_UP = "Player_climb_up";
        private const string PLAYER_CLIMB_DOWN = "Player_climb_down";
        private const string PLAYER_CLIMBING_IDLE = "Player_climbing_idle";
        private const string PLAYER_ROTATE_LEFT = "Player_rotate_left";
        private const string PLAYER_ROTATE_RIGHT = "Player_rotate_right";

        public string currentState;
        
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        //zoom
        public CinemachineVirtualCamera cinemachineVirtualCamera;
        public float zoomMinDistance = 1.5f;
        public float zoomMaxDistance = 6f;
        public float zoomSpeed = 0.02f;
        public float zoomFactor = 0.5f;
        public float zoom = 4f;
        private float cameraDistance = 4f;
        private float zoomVelocity = 0f;
        //

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        /*// animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        */
        
        //climb
        [SerializeField] private float _pipeTimeout = 0.0f;
        [SerializeField] private float _pipeTimeoutMax = 5.0f;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        [SerializeField] GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
                #else
				return false;
                #endif
            }
        }

        public void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;

            //_animator.Play(newState);

            currentState = newState;
            
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            pushScript = GetComponent<BasicRigidBodyPush>();
            playerAnimator = GetComponent<Animator>();
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            //AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            //zoom
            this.zoomVelocity = 0f;
            //
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            if (_input.move != Vector2.zero)
            {
                playerAnimator.SetBool("Moving", true);
            }
            else
            {
                playerAnimator.SetBool("Moving", false);
            }
            
            
            JumpAndGravity();
            GroundedCheck();
            if (OnPipe)
            {
                Climb();
            }
            else
            {
                Move();
            }
        }

        private void LateUpdate()
        {
            CameraRotation();

            //zoom
            CameraZoom();
            //
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pipe"))
            {
                OnPipe = true;
                playerAnimator.SetBool("OnPipe", OnPipe);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pipe"))
            {
                OnPipe = false;
                playerAnimator.SetBool("OnPipe", OnPipe);
            }
        }
        
        /*private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }
        */
        
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            
            playerAnimator.SetBool("Grounded", Grounded);
            
            // update animator if using character
            /*if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
            */
        }

        //zoom
        private void CameraZoom()
        {
            this.zoom -= _input.zoom / 240f * this.zoomFactor;
            this.zoom = Mathf.Clamp(this.zoom, this.zoomMinDistance, this.zoomMaxDistance);
            this.cameraDistance = Mathf.SmoothDamp(this.cameraDistance, this.zoom, ref this.zoomVelocity, Time.unscaledTime * this.zoomSpeed);

            this.cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = this.cameraDistance;
        }
        //

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Climb()
        {

            // Calculate the climb velocity based on climbSpeed
            Vector3 climbVelocity = transform.up * ClimbingSpeed;

            // Move the player using the climb velocity
            if (_input.move.y > 0.0f)
            {
                _controller.Move(climbVelocity * Time.deltaTime);
                ChangeAnimationState(PLAYER_CLIMB_UP);
                GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.PipeClimb));
                playerAnimator.SetFloat("MovingY", climbVelocity.y);
            }
            
            else if (_input.move.y < 0.0f)
            {
                _controller.Move(-climbVelocity * Time.deltaTime);
                ChangeAnimationState(PLAYER_CLIMB_DOWN);
                GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.PipeClimb));
                playerAnimator.SetFloat("MovingY", -climbVelocity.y);
            }
            else
            {
                playerAnimator.SetFloat("MovingY", 0.0f);
            }
            // Check for input to move left or right while climbing
            float horizontalInput = _input.move.x;
            if (horizontalInput != 0)
            {
                // Calculate the movement direction perpendicular to the pipe
                Vector3 movementDirection = transform.right * horizontalInput;

                // Move the player in the movement direction

                _controller.Move(movementDirection * (_speed * Time.deltaTime));
            }

            if (_input.jump && _pipeTimeout >= _pipeTimeoutMax)
            {
                // Move the player in the jump direction
                //_controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * (_speed * Time.deltaTime));
                //_pipeJump = true;
                _pipeTimeout = 0.0f;
                //_input.jump = false;

            }
        }

        private void Move()
        {

            // set target speed based on move speed, sprint speed and if sprint is pressed
            //float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            float targetSpeed = MoveSpeed;
            
            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;
            
            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            playerAnimator.SetFloat("Speed", _speed);
            
            if (Grounded && currentState == PLAYER_FALL)
            {
                ChangeAnimationState(PLAYER_LAND);
                if(playerName == PlayerNames.Cub_big)
                    GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.LandCub));
                if (playerName == PlayerNames.Kit_small)
                    GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.LandKit));
                
            }
            else if (_speed != 0 && !_input.jump)
            {
                ChangeAnimationState(PLAYER_WALK);
                if(playerName.ToString() == "Kit_small")
                    GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.FootstepsKit));
                else
                {
                    GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.FootstepsCub));
                }
            }
            else if (_input.jump && Grounded && _jumpTimeoutDelta <= 0.0f)
            {
                ChangeAnimationState(PLAYER_JUMP);
            }
            else if (OnPipe)
            {
                ChangeAnimationState(PLAYER_CLIMBING_IDLE);
            }
            else if(Grounded)
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
            else
            {
                ChangeAnimationState(PLAYER_FALL);
            }
            // update animator if using character
            /*if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
            */
        }

        private void JumpAndGravity()
        {
            if (Grounded || OnPipe)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                /*if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }
                */

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = 0f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    //Debug.Log("Jumping");
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    ChangeAnimationState(PLAYER_JUMP);
                    if(playerName.ToString() == "Kit_small")
                        GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.JumpKit));
                    else
                    {
                        GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.JumpCub));
                    }
                    
                    playerAnimator.SetTrigger("Jump");
                    
                    // update animator if using character
                    /*if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                    */
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    ChangeAnimationState(PLAYER_FALL);
                    // update animator if using character
                    /*if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                    */
                }

                // if we are not grounded, do not jump
                _input.jump = false;
                playerAnimator.ResetTrigger("Jump");
                playerAnimator.SetFloat("MovingY", _verticalVelocity);
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity && !Grounded)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }

            if (_pipeTimeout < _pipeTimeoutMax)
            {
                _pipeTimeout += Time.deltaTime;
            }
            
            if(!OnPipe)
                playerAnimator.SetFloat("MovingY", _verticalVelocity);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}