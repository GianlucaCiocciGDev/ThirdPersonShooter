using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gdev
{
    public class PlayerInventoryManager : InventoryManager
    {
        public List<WeaponItem> weaponsInventory = new List<WeaponItem>();
        public WeaponItem equippedWeapon;
        int currentIndex = 0;

        PlayerManager playerManager;
        PlayerRiggingManager playerRiggingManager;
        WeaponHolder handSlotHolder;
        PlayerCombatManager playerCombatManager;

        [Header("Hand Ik Targets")]
        private RightHandIKTarget _RightHandIKTarget;
        private LeftHandIKTarget _LeftHandIKTarget;

        [SerializeField] GameObject leftHand;
        [SerializeField] GameObject bottleModel;

        protected override void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }
        protected override void Start()
        {
            playerRiggingManager = GetComponent<PlayerRiggingManager>();
            handSlotHolder = GetComponentInChildren<WeaponHolder>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            Task task = LoadWeapon(10);
        }
        public override void TrySwitchWeapon()
        {
            if (playerManager.IsInteract)
                return;

            int weaponInInventoryCount = weaponsInventory.Count;
            if (weaponInInventoryCount < 2)
                return;

            currentIndex++;
            if (currentIndex >= weaponInInventoryCount)
                currentIndex = 0;

            playerRiggingManager.EraseHandIKForWeapon();
            playerManager.playerAnimatorManager.SwitchWeapon();
            Task task = LoadWeapon(500);
        }
        public async override Task LoadWeapon(int milliseconds)
        {
            await Task.Run(async () =>
            {
                await base.LoadWeapon(milliseconds);
            });
            InitializeNewWeapon();
        }
        protected override void InitializeNewWeapon()
        {
            equippedWeapon = weaponsInventory[currentIndex];
            handSlotHolder.LoadWeaponModel(equippedWeapon);
            playerCombatManager.ChangeWeapon(equippedWeapon);
            _LeftHandIKTarget = handSlotHolder.CurrentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            _RightHandIKTarget = handSlotHolder.CurrentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
            playerRiggingManager.SetHandIKForWeapon(_RightHandIKTarget, _LeftHandIKTarget, true);
            playerManager.playerCombatManager.UpdateAmmoSettingUI?.Invoke(equippedWeapon.GetAmmunitionCurrent(), equippedWeapon.GetAmmunitionTotal(), equippedWeapon.IsCriticalAmmunition());
        }
        public override int GetLastIndex()
        {
            //Get last index with wrap around.
            int newIndex = currentIndex - 1;
            if (newIndex < 0)
                newIndex = weaponsInventory.Count;

            //Return.
            return newIndex;
        }
        public override int GetNextIndex()
        {
            //Get next index with wrap around.
            int newIndex = currentIndex + 1;
            if (newIndex >= weaponsInventory.Count)
                newIndex = 0;

            //Return.
            return newIndex;
        }
        public override WeaponItem GetEquipped() => equippedWeapon;
        public override int GetEquippedIndex() => currentIndex;
        public override async void ReloadCurrentWeapon()
        {
            if (playerManager.IsInteract)
                return;

            playerRiggingManager.EraseHandIKForWeapon();
            playerManager.playerAnimatorManager.ReloadWeapon();
            await Task.Run(async () =>
            {
                await base.LoadWeapon(1000);
            });
            playerRiggingManager.SetHandIKForWeapon(_RightHandIKTarget, _LeftHandIKTarget, true);
            equippedWeapon.Reload();
            playerManager.playerCombatManager.UpdateAmmoSettingUI?.Invoke(equippedWeapon.GetAmmunitionCurrent(), equippedWeapon.GetAmmunitionTotal(), equippedWeapon.IsCriticalAmmunition());
        }
        public override async void RestoreHealth()
        {
            if (playerManager.IsInteract)
                return;

            playerRiggingManager.EraseHandIKForWeapon();
            GameObject bottlePrefab = Instantiate(bottleModel,leftHand.transform);
            playerManager.playerAnimatorManager.Restore();
            await Task.Run(async () =>
            {
                await base.LoadWeapon(900);
            });
            Destroy(bottlePrefab);
            playerRiggingManager.SetHandIKForWeapon(_RightHandIKTarget, _LeftHandIKTarget, true);
            playerManager.playerStatsManager.OnRestore(100);
        }
    }
}
