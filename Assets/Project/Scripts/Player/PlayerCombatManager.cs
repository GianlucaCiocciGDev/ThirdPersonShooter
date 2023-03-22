using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gdev
{
    public class PlayerCombatManager : MonoBehaviour
    {
        InputManager inputManager;
        PlayerManager playerManager;
        Camera mainCamera;
        WeaponItem currentWeapon;

        public LayerMask mask;

        [Header("IK section")]
        [SerializeField] GameObject aimTarget;

        [Space]

        [Header("AIM Setting")]
        [SerializeField] Cinemachine.CinemachineVirtualCamera aimCamera;
        [SerializeField] Image reticle;
        

        public UnityEvent<int, int,bool> UpdateAmmoSettingUI;

        private void Awake()
        {
            mainCamera = Camera.main;
        }
        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            mask = LayerMask.NameToLayer("Enemy");
            inputManager = GetComponent<InputManager>();
        }

        public void AimEvent(bool isAiming)
        {
            aimCamera.gameObject.SetActive(isAiming);
            reticle.gameObject.SetActive(isAiming);
        }
        public void HandleCombat()
        {
            if (playerManager.IsInteract)
                return;

            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = mainCamera.ScreenPointToRay(screenCenterPoint);

            RaycastHit hit;
            var rayCasthit = Physics.Raycast(ray, out hit);
            mouseWorldPosition = hit.point;
            if (hit.transform?.gameObject.layer == mask)
            {
                reticle.color = Color.red;
            }
            else
            {
                reticle.color = Color.white;
                mouseWorldPosition = ray.origin + ray.direction * 900f;
            }
                
            if(currentWeapon != null)
            {
                if (inputManager.holdingButtonFire)
                {
                    currentWeapon.Shoot(playerManager, hit);
                    UpdateAmmoSettingUI?.Invoke(currentWeapon.GetAmmunitionCurrent(), currentWeapon.GetAmmunitionTotal(),currentWeapon.IsCriticalAmmunition());
                }
            }

            HandleIkAimTarget(mouseWorldPosition);
        }
        void HandleIkAimTarget(Vector3 point)
        {
            aimTarget.transform.position = point;
        }
        public void ChangeWeapon(WeaponItem weapon)
        {
            currentWeapon = weapon;
        }
    }
}

