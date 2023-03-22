using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public abstract  class StatsManager : MonoBehaviour
    {
        protected float maxHealth = 100;
        protected float currentHealth;
        protected bool isDeath;
        protected virtual void Start()
        {
            currentHealth = maxHealth;
        }
        public virtual void OnDamage(float damage)
        {
            if (isDeath)
                return;

            if (currentHealth <= 0)
            {
                OnDeath();
                currentHealth = 0;
                isDeath = true;
            }
            currentHealth -= damage;
        }
        protected abstract void OnDeath();
        public abstract void OnRestore(float amount);
    }
}
