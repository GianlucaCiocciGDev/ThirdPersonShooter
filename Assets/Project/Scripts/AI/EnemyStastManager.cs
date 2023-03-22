using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class EnemyStastManager : StatsManager
    {
        [SerializeField]private float blinkIntensity;
        [SerializeField] private float blinkDuration;
        [SerializeField] private DeadState deadState;

        EnemyManager enemyManager;
        SkinnedMeshRenderer skinnedMeshRenderer;
        float blinkTimer;
        Color baseColor;
        HealthStats healthStats;

        public bool enemyIsDead
        {
            get
            {
                return base.isDeath;
            }
        }

        protected override void Start()
        {
            base.Start();
            enemyManager = GetComponent<EnemyManager>();
            skinnedMeshRenderer =transform.GetChild(0).GetChild(1).GetComponentInChildren<SkinnedMeshRenderer>();
            baseColor = skinnedMeshRenderer.material.color;
            healthStats = GetComponentInChildren<HealthStats>();
            healthStats.InitializeBar(base.maxHealth, base.currentHealth);
        }
        private void Update()
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntensity) + 1.0f;
            skinnedMeshRenderer.material.color = baseColor * intensity;
        }

        #region Implement Abstract class
        public override void OnDamage(float damage)
        {
            base.OnDamage(damage);
            blinkTimer = blinkDuration;
            healthStats.SetCurrentHealthBar(base.currentHealth);
        }
        protected override void OnDeath()
        {
            enemyManager.SwitchNextState(deadState);
        }
        public override void OnRestore(float amount)
        {
            base.currentHealth = amount;
        }
        #endregion
    }
}
