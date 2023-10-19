 using Game;
 using Game.Characters.Player;
 using Game.Input;
 using UnityEngine;
 using Zenject;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [SerializeField, TextArea]
        private string debugText;
        
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        public bool WallNear = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float WallCheckRadius = 0.32f;
        [Tooltip("Useful for rough ground")]
        public float WallOffset = 1f;
        
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

        // cinemachine
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        // player
        private float speed;
        private float horizontalAcceleration;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float targetRotationOffset = 0.0f;
        private float wallRotationElapsed;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;

        // timeout deltatime
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;

        // animation IDs
        private int animIDSpeed;
        private int animIDGrounded;
        private int animIDJump;
        private int animIDFreeFall;
        private int animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput playerInput;
#endif
        private Animator animator;
        private CharacterController controller;
        private InputEventsHolder input;
        
        private GameObject _mainCamera;
        private GameObject mainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                }
                return _mainCamera;
            }
        }

        private const float threshold = 0.01f;

        private bool hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return true;//_playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private MovementSettings settings;
        private Collider closestWall;
        private Vector3 closestWallPoint;

        private void Start()
        {
            settings = ServicesMediator.MovementSettings;
            input = InputMediator.InputEventsHolder;
            cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            hasAnimator = TryGetComponent(out animator);
            controller = GetComponent<CharacterController>();
#if ENABLE_INPUT_SYSTEM 
            playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            jumpTimeoutDelta = settings.JumpTimeout;
            fallTimeoutDelta = settings.FallTimeout;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            
            var groundedText = Grounded ? "Grounded" : string.Empty;
            var wallText = WallNear ? "Wall" : string.Empty;
            var jump = input.Jump ? "Jump" : string.Empty;
            var time = jumpTimeoutDelta > 0 ? $"time {jumpTimeoutDelta}" : string.Empty;
            //input.Jump && jumpTimeoutDelta
            debugText = $"{groundedText}\n{wallText}\n{jump}\n{time}";
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            animIDSpeed = Animator.StringToHash("Speed");
            animIDGrounded = Animator.StringToHash("Grounded");
            animIDJump = Animator.StringToHash("Jump");
            animIDFreeFall = Animator.StringToHash("FreeFall");
            animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private readonly Collider[] wallCheckResults = new Collider[10];
        private void GroundedCheck()
        {
            // set sphere position, with offset
            var groundPosition = GetGroundCheckPosition();
            Grounded = Physics.CheckSphere(groundPosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (!Grounded)
            {
                CheckWall();
            }

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(animIDGrounded, Grounded);
            }
        }

        private Vector3 GetGroundCheckPosition()
        {
            return new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
        }
        private Vector3 GetWallCheckPosition()
        {
            return new Vector3(transform.position.x, transform.position.y + WallOffset,
                transform.position.z);
        }
        
        private void CheckWall()
        {
            var wallCheckPosition = GetWallCheckPosition();
            var hits = Physics.OverlapSphereNonAlloc(wallCheckPosition, WallCheckRadius, wallCheckResults, GroundLayers, QueryTriggerInteraction.Ignore);
            if (hits == 0)
            {
                closestWall = null;
                WallNear = false;
                return;
            }

            var closestDistance = float.MaxValue;
            for (int i = 0; i < hits; i++)
            {
                var result = wallCheckResults[i];
                
                var closestPoint = result.ClosestPoint(wallCheckPosition);
                
                var directionToSurface = closestPoint - wallCheckPosition;
                //Debug.DrawLine(closestPoint, directionToSurface, Color.green, 5);

                var dot = Vector3.Dot(directionToSurface.normalized, Vector3.up);
                if (!(Mathf.Abs(dot) < settings.WallDetection))
                {
                    continue;
                }
                
                WallNear = true;
                var distance = (wallCheckPosition - closestPoint).sqrMagnitude;
                if (closestWall == null)
                {
                    closestWall = result;
                    closestWallPoint = closestPoint;
                }
                else if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestWall = result;
                    closestWallPoint = closestPoint;
                }
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (input.Look.sqrMagnitude >= threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                cinemachineTargetYaw += input.Look.x * deltaTimeMultiplier;
                cinemachineTargetPitch += input.Look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = settings.MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (input.Move == Vector2.zero) targetSpeed = 0.0f;
            
            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = input.Move.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude + horizontalAcceleration,
                    Time.deltaTime * settings.SpeedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed + horizontalAcceleration;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * settings.SpeedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (Grounded && input.Move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    settings.RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else
            {
                horizontalAcceleration = Mathf.Lerp(horizontalAcceleration, 0, Time.deltaTime);
            }


            var finalRotation = wallRotationElapsed > 0 ? targetRotationOffset : targetRotation; 
            wallRotationElapsed -= Time.deltaTime;
            
            Vector3 targetDirection = Quaternion.Euler(0.0f, finalRotation, 0.0f) * Vector3.forward;

            //Debug.LogError($"<color=red>{targetRotationOffset} ({targetRotation + targetRotationOffset})</color>");
            
            // move the player
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetFloat(animIDSpeed, animationBlend);
                animator.SetFloat(animIDMotionSpeed, inputMagnitude + horizontalAcceleration);
            }
        }

        private void JumpAndGravity()
        {
            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
            
            if (Grounded)
            {
                // reset the fall timeout timer
                fallTimeoutDelta = settings.FallTimeout;

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, false);
                    animator.SetBool(animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }

                // Jump
                if (input.Jump && jumpTimeoutDelta <= 0.0f)
                {
                    input.Jump = false;
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(settings.JumpHeight * -2f * settings.Gravity);

                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(animIDJump, true);
                    }
                }
            }
            else if (WallNear && input.Jump && jumpTimeoutDelta <= 0.0f)
            {
                WallJump();
            }
            else
            {
                // reset the jump timeout timer
                jumpTimeoutDelta = settings.JumpTimeout;

                // fall timeout
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(animIDFreeFall, true);
                    }
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += settings.Gravity * Time.deltaTime;
            }
        }

        private void WallJump()
        {
            input.Jump = false;
            
            var ground = GetGroundCheckPosition();
            var distance = (closestWallPoint - ground).magnitude * 1.1f;
            var ray = new Ray(ground, closestWallPoint - ground);
            if (!Physics.Raycast(ray, out var hitInfo, distance, GroundLayers, QueryTriggerInteraction.Ignore))
            {
                return;
            }

            jumpTimeoutDelta = settings.WallJumpTimeout;
            horizontalAcceleration += 2f;
            
            var dotWithInput = Mathf.Abs(Vector2.Dot(input.Move, Vector2.right));
            var wallForward = Vector3.Cross(hitInfo.normal, hitInfo.transform.up);
            if (Vector3.Dot(wallForward, transform.forward) < 0)
            {
                //crossed the wrong way
                wallForward = -wallForward;
            }

            var wallForwardRotation = Quaternion.LookRotation(wallForward);
            
            var wallNormal = hitInfo.normal;
            var wallNormalRotation = Quaternion.LookRotation(wallNormal);
            
            var offsetValue = dotWithInput > 0.8f ? settings.WallJumpSideAngle : settings.WallJumpDefaultAngle;
            var wallJumpRotation = Quaternion.Lerp(wallForwardRotation, wallNormalRotation, offsetValue / 90f);
            targetRotationOffset = wallJumpRotation.eulerAngles.y;
            wallRotationElapsed = settings.WallRotationDuration;

            //Debug.LogError($"<color=red>WALLJUMP {offsetValue} ({offsetValue / 90f}) ({targetRotationOffset})</color>");
            
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            verticalVelocity = Mathf.Sqrt(settings.JumpHeight * -2f * settings.Gravity);

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDJump, true);
            }
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
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
        
        public class Factory : PlaceholderFactory<ThirdPersonController>
        {
            
        }
    }
}