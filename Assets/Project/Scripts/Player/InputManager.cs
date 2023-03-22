using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Gdev
{
    public class InputManager : MonoBehaviour
    {
        MapInput input;
        public Vector2 axisMovement;
        public Vector2 axisLook;

        public bool holdingButtonFire;
        public bool holdingButtonAim;
        public bool holdingButtonRun;

        public bool analogMovement;
        bool infoWindowOpen = false;

        public UnityEvent<bool> AimingEvent;
        public UnityEvent<bool> OpenTutorialEvent;

        PlayerManager playerManager;

        private void Awake()
        {
            input = new MapInput();

            input.Player.Look.started += OnLook;
            input.Player.Look.performed += OnLook;
            input.Player.Look.canceled += OnLook;

            input.Player.Movement.started += OnMove;
            input.Player.Movement.performed += OnMove;
            input.Player.Movement.canceled += OnMove;

            input.Player.Run.started += OnTryRun;
            input.Player.Movement.performed += OnMove;
            input.Player.Run.canceled += OnTryRun;

            input.Player.Jump.started += OnJump;
            input.Player.Jump.performed += OnJump;
            input.Player.Jump.canceled += OnJump;

            input.Player.Aim.started += OnTryAiming;
            input.Player.Aim.performed += OnTryAiming;
            input.Player.Aim.canceled += OnTryAiming;

            input.Player.Fire.started += OnTryFire;
            input.Player.Fire.performed += OnTryFire;
            input.Player.Fire.canceled += OnTryFire;

            input.Player.Quit.started += OnQuitGame;

            input.Player.InventoryNextWheel.started += OnTryInventoryNext;
            input.Player.InventoryNextWheel.performed += OnTryInventoryNext;
            input.Player.InventoryNextWheel.canceled += OnTryInventoryNext;

            input.Player.Reload.started += OnReload;
            input.Player.Reload.canceled += OnReload;

            input.Player.Restore.started += OnRestore;
            input.Player.Restore.canceled += OnRestore;

            input.Player.Info.started += OnInfo;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }
        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        #region Implement Personal Events
        /// <summary>
        /// Movement Axis.
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            axisMovement = context.ReadValue<Vector2>();
        }
        /// <summary>
        /// Look Axis.
        /// </summary>
        public void OnLook(InputAction.CallbackContext context)
        {
            axisLook = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// Jump.
        /// </summary>
        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    playerManager.playerLocomotionManager.HandleJump();
                    break;
            }
        }
        
        /// <summary>
        /// Shoot.
        /// </summary>
        public void OnTryFire(InputAction.CallbackContext context)
        {
            switch (context)
            {
                case { phase: InputActionPhase.Started }:
                    holdingButtonFire = true;
                    break;
                case { phase: InputActionPhase.Performed }:
                    break;
                case { phase: InputActionPhase.Canceled }:
                    holdingButtonFire = false;
                    break;
            }
        }

        /// <summary>
        /// Aiming.
        /// </summary>
        public void OnTryAiming(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    AimingEvent?.Invoke(true);
                    holdingButtonAim = true;
                    break;
                case InputActionPhase.Canceled:
                    AimingEvent?.Invoke(false);
                    holdingButtonAim = false;
                    break;
            }
        }

        /// <summary>
        /// Running.
        /// </summary>
        public void OnTryRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    holdingButtonRun = true;
                    break;
                case InputActionPhase.Canceled:
                    holdingButtonRun = false;
                    break;
            }
        }

        /// <summary>
        /// Switch Iventory Weapon.
        /// </summary>
        public void OnTryInventoryNext(InputAction.CallbackContext context)
        {
            if (playerManager.playerInventoryManager == null)
                return;

            switch (context)
            {
                case { phase: InputActionPhase.Performed }:

                    playerManager.playerInventoryManager.TrySwitchWeapon();
                    break;
            }
        }

        /// <summary>
        /// Reload Weapon.
        /// </summary>
        public void OnReload(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    playerManager.playerInventoryManager.ReloadCurrentWeapon();
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }

        /// <summary>
        /// Restore Health.
        /// </summary>
        private void OnRestore(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    playerManager.playerInventoryManager.RestoreHealth();
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }

        /// <summary>
        /// Quit Game.
        /// </summary>
        public void OnQuitGame(InputAction.CallbackContext context)
        {
            switch (context)
            {
                case { phase: InputActionPhase.Started }:
                    Application.Quit();
                    break;
            }
        }

        /// <summary>
        /// Open Window Info.
        /// </summary>
        public void OnInfo(InputAction.CallbackContext context)
        {
            switch (context)
            {
                case { phase: InputActionPhase.Started }:
                    OpenTutorialEvent?.Invoke(!infoWindowOpen);
                    infoWindowOpen = !infoWindowOpen;
                    break;
            }
        }
        #endregion

        private void OnEnable()
        {
            input.Enable();
        }
        private void OnDisable()
        {
            input.Disable();
        }
    }
}

