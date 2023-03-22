using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gdev
{
    [CreateAssetMenu(fileName = "Items", menuName = "Items/WeaponItem")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        private LayerMask _mask;
        [Space]
        [Header("Damage")]
        public float baseDamage;

        [SerializeField]private int totalAmmunition;
        private int ammunitionCurrent = 100;
        private double criticalAmmunition;
        [SerializeField]private int roundsPerMinutes;
        [SerializeField]private int maximumRange;

        public bool isAutomatic;
        private float _lastShotTime;

        private void OnEnable()
        {
            ammunitionCurrent = totalAmmunition;
            _mask = LayerMask.NameToLayer("Enemy");
            _lastShotTime = Time.time;
            criticalAmmunition = CriticalAmmunition();
        }
        public bool HasAmmunition() => ammunitionCurrent > 0;
        public float GetRateOfFire() => roundsPerMinutes;
        public bool IsAutomatic() => isAutomatic;
        public int GetAmmunitionCurrent() => ammunitionCurrent;
        public int GetAmmunitionTotal() => totalAmmunition;
        public bool IsCriticalAmmunition() => ammunitionCurrent <= criticalAmmunition;
        public void Shoot(PlayerManager playerManager, RaycastHit ray)
        {
            bool hasHitTarget = ray.transform != null;

            if (HasAmmunition())
            {
                if (Time.time - _lastShotTime > 60.0f / GetRateOfFire())
                {
                    _lastShotTime = Time.time;
                    ammunitionCurrent--;
                    playerManager.playerFXManager.PlayMuzzle();
                    if (hasHitTarget)
                    {
                        playerManager.playerFXManager.PlayLaser(ray.point);

                        if (ray.transform?.root.gameObject == playerManager.transform.root.gameObject)
                            return;

                        float rangeOfFire = (ray.point - playerManager.transform.position).magnitude;
                        if (rangeOfFire <= maximumRange)
                        {
                            if (ray.transform.gameObject.layer != _mask)
                                playerManager.playerFXManager.PlayHitEffect(ray);
                            else
                                ray.transform.GetComponent<EnemyStastManager>()?.OnDamage(baseDamage);
                        }
                    }
                }
            }
        }
        public void Reload()
        {
            ammunitionCurrent = totalAmmunition;
        }
        private double CriticalAmmunition() =>  (double)(totalAmmunition * 20) / 100;
    }
}

