using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class PlayerLocomotionManager : LocomotionManager
    {
        [Header("Player")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        private float _cinemachineTargetPitch;
        private float _rotationVelocity;
        private const float _threshold = 0.01f;

        [Header("Gravity Settings")]
        public float verticalSpeed;
        [SerializeField] float Gravity = -15.0f;
        public float JumpHeight = 1.2f;
        public bool grounded = true;

        private CharacterController _controller;
        private InputManager inputManager;
        private PlayerManager playerManager;

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            inputManager = GetComponent<InputManager>();
            _controller = GetComponent<CharacterController>();
        }

        #region Implement abstract class
        public override void HandleMove()
        {
            bool isSprinting = inputManager.holdingButtonRun;
            float targetSpeed = isSprinting ? SprintSpeed : MoveSpeed;
            Vector3 verticalmovement = new Vector3(0.0f, verticalSpeed, 0.0f);
            Vector3 inputDirection = new Vector3(inputManager.axisMovement.x, 0.0f, inputManager.axisMovement.y).normalized;

            float valueX = Mathf.Lerp(inputDirection.x, targetSpeed * 1, Time.deltaTime);
            float valueY = Mathf.Lerp(inputDirection.z, targetSpeed * 1, Time.deltaTime);
            playerManager.playerAnimatorManager.UpdateAnimatorValue(valueX, valueY);

            if (inputManager.axisMovement != Vector2.zero)
            {
                inputDirection = transform.right * inputManager.axisMovement.x + transform.forward * inputManager.axisMovement.y;
            }
            _controller.Move((verticalmovement + (inputDirection * targetSpeed)) * Time.deltaTime);

            HandleGravity();
        }
        public override void CameraRotation()
        {
            if (inputManager.axisLook.sqrMagnitude >= _threshold)
            {
                _cinemachineTargetPitch += inputManager.axisLook.y * RotationSpeed;
                _rotationVelocity = inputManager.axisLook.x * RotationSpeed;

                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                //Quaternion desiredDir = Quaternion.LookRotation(Vector3.up * _rotationVelocity);
                //transform.rotation = desiredDir;
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }
        public override void HandleGravity()
        {
            if (Physics.Raycast(transform.position, Vector3.down, .05f, LayerMask.GetMask("Ground")))
            {
                verticalSpeed = 0.01f;
                grounded = true;
            }
            else
            {
                verticalSpeed += Gravity * Time.deltaTime;
                grounded = false;
            }
            playerManager.playerAnimatorManager.BaseAnimator.SetBool("Grounded", grounded);
        }
        public override void HandleJump()
        {
            if (grounded)
            {
                playerManager.playerAnimatorManager.Jump();
                verticalSpeed += Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }
        #endregion

        #region Utils
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.down) * .05f;
            Gizmos.DrawRay(transform.position, direction);
        }
    }
}

