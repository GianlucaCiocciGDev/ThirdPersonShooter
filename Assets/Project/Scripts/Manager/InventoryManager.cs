using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gdev
{
    public abstract class InventoryManager : MonoBehaviour
    {
        protected abstract void Start();
        protected abstract void Awake();
        public abstract void TrySwitchWeapon();
        public virtual async Task LoadWeapon(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
        protected abstract void InitializeNewWeapon();
        public abstract int GetLastIndex();
        public abstract int GetNextIndex();
        public abstract WeaponItem GetEquipped();
        public abstract int GetEquippedIndex();
        public abstract void ReloadCurrentWeapon();
        public abstract void RestoreHealth();
    }
}
