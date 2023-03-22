using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Gdev
{
    public class AmmoStats : MonoBehaviour
    {
        [SerializeField] TMP_Text ammoCountText;
        [SerializeField] Slider ammoCountSlider;
        [SerializeField] Image ammoCountSliderFillImage;

        [SerializeField] Color maxAmmoColor;
        [SerializeField] Color criticalAmmoColor;
        public void UpdateAmmoStats(int currentAmmo,int totalAmmo,bool criticalAmmo)
        {
            ammoCountText.text = $"{currentAmmo}/{totalAmmo}";
            ammoCountSlider.value = currentAmmo;

            if(criticalAmmo) // sotto il 20% la situazione munitzioni è critica
            {
                ammoCountSliderFillImage.color = criticalAmmoColor;
            }
            else
                ammoCountSliderFillImage.color = maxAmmoColor;

            ammoCountSlider.maxValue = totalAmmo;
        }

    }
}
