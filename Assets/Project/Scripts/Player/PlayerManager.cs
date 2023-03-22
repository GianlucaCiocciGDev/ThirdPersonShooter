using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class PlayerManager : MonoBehaviour
    {
        [HideInInspector]public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector]public PlayerCombatManager playerCombatManager;
        [HideInInspector]public PlayerInventoryManager playerInventoryManager;
        [HideInInspector]public PlayerStatsManager playerStatsManager;
        [HideInInspector]public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector]public PlayerFXManager playerFXManager;
        [HideInInspector]public PlayerRiggingManager playerRiggingManager;
        [HideInInspector]public WeaponHolder weaponHolder;

        public bool IsInteract;
        private void Awake()
        {
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerRiggingManager = GetComponent<PlayerRiggingManager>();
            weaponHolder = GetComponent<WeaponHolder>();
        }

        void Update()
        {
            IsInteract = playerAnimatorManager.GetIsInteract();
            playerLocomotionManager.HandleMove();
            playerCombatManager.HandleCombat();
        }
        private void LateUpdate()
        {
            playerLocomotionManager.CameraRotation();
        }
    }
}
