using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class WeaponHolder : MonoBehaviour
    {
        public Transform ParentOverride;
        public WeaponItem CurrentWeapon;
        public GameObject CurrentWeaponModel;
        PlayerFXManager playerFXManager;

        private void Start()
        {
            playerFXManager = GetComponentInParent<PlayerFXManager>();
        }
        public void LoadWeaponModel(WeaponItem itemWeapon)
        {
            UnloadWeaponAndDestroy();
            if (itemWeapon == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(itemWeapon.modelPrefab) as GameObject;
            if (model != null)
            {
                if (ParentOverride != null)
                {
                    model.transform.parent = ParentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            CurrentWeaponModel = model;

            WeaponFX currentWeaponFx = CurrentWeaponModel.GetComponentInChildren<WeaponFX>();
            playerFXManager.SetWeaponFX(currentWeaponFx);
        }
        public void UnloadWeapon()
        {
            if (CurrentWeaponModel != null)
            {
                CurrentWeaponModel.SetActive(false);
            }
        }
        public void UnloadWeaponAndDestroy()
        {
            if (CurrentWeaponModel != null)
            {
                Destroy(CurrentWeaponModel);
            }
        }
    }
}

