using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gdev
{
    public class HealthStats : MonoBehaviour
    {
        [SerializeField]private Slider _HealtBarSlider;
        [SerializeField]private Slider _HealthBarBackSlider;

        private float _MaxHealt;
        private float _CurrentHealth;

        public void SetMaxHealth(float maxHealth)
        {
            _HealtBarSlider.maxValue = maxHealth;
            _HealtBarSlider.value = maxHealth;

            _MaxHealt = maxHealth;
            _CurrentHealth = _MaxHealt;

            _HealthBarBackSlider.maxValue = maxHealth;
            _HealthBarBackSlider.value = maxHealth;
        }
        public async void SetCurrentHealthBar(float currentHealth)
        {
            _CurrentHealth = currentHealth;
            _HealtBarSlider.value = Mathf.Clamp(currentHealth, 0, _MaxHealt);
            await _HealthBarBackSlider.DOValue(_CurrentHealth, 1f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
        }
        public void InitializeBar(float maxHealth, float currentHealth)
        {
            SetMaxHealth(maxHealth);
            SetCurrentHealthBar(currentHealth);
        }
    }
}
