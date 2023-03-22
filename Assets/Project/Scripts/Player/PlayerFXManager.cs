using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class PlayerFXManager : CharacterFXManager
    {
        private WeaponFX weaponFX;
        [SerializeField] private Cinemachine.CinemachineImpulseSource impulse;

        public override void PlayLaser(Vector3 target)
        {
            weaponFX.PlayLaser(target);
        }
        public override void PlayMuzzle()
        {
            weaponFX.PlayMuzzle();
            if (impulse)
                impulse.GenerateImpulse(transform.right);
        }
        public override void PlayHitEffect(RaycastHit hit)
        {
            weaponFX.PlayHitEffect(hit);
        }
        public void SetWeaponFX(WeaponFX model)
        {
            weaponFX = model;
        }
    }
}
