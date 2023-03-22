using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gdev
{
    public class PlayerStatsManager : StatsManager
    {
        [SerializeField] HealthStats healthStats;
        PlayerManager playerManager;
        PlayerProfileManager profileManager;

        public UnityEvent restartGameEvent;
        public bool playerIsDead
        {
            get 
            {
                return base.isDeath;
            }
        }
        protected override void Start()
        {
            base.Start();
            playerManager = GetComponent<PlayerManager>();
            profileManager = GetComponent<PlayerProfileManager>();
            healthStats.InitializeBar(base.maxHealth, base.currentHealth);
        }

        #region Implement Abstract class
        protected override void OnDeath()
        {
            playerManager.playerAnimatorManager.PlayTargetAnimation("Death");
            playerManager.enabled = false;
            restartGameEvent?.Invoke();
        }
        public override void OnDamage(float damage)
        {
            base.OnDamage(damage);
            healthStats.SetCurrentHealthBar(base.currentHealth);
            if(currentHealth < 40)
            {
                profileManager.SetProfile(State.Critical);
            }
        }
        public override void OnRestore(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            healthStats.SetCurrentHealthBar((int)currentHealth);
            profileManager.SetProfile(State.Normal);
        }
        #endregion
    }
}
