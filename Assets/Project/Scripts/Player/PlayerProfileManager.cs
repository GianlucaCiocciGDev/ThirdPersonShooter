using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Gdev
{
    public class PlayerProfileManager : ProfileManager
    {
        public VolumeProfile profile;
        ColorAdjustments colorAdjustments;
        Vignette vignette;
        ChromaticAberration chromaticAberration;

        [SerializeField] Color CriticalColor;
        Color normalColor;
        void Start()
        {
            profile.TryGet<ColorAdjustments>(out colorAdjustments);
            profile.TryGet<Vignette>(out vignette);
            profile.TryGet<ChromaticAberration>(out chromaticAberration);
            normalColor = Color.white;
            base.SetProfile(State.Normal);
        }

        protected override void SetColorAdjustments(State state)
        {
            if (state == State.Critical)
            {
                colorAdjustments.colorFilter.value = CriticalColor;
                colorAdjustments.postExposure.value = 0.27f;
            }
            else
            {
                colorAdjustments.colorFilter.value = normalColor;
                colorAdjustments.postExposure.value = 0.0f;
            }
        }
        protected override void SetVignette(State state)
        {
            if (state == State.Critical)
            {
                vignette.intensity.value = .4f;
            }
            else
            {
                //vignette.intensity.Interp(vignette.intensity.value, .025f, .1f);
                vignette.intensity.value = .025f;
            }
        }
        protected override void SetAberration(State state)
        {
            if (state == State.Critical)
            {
                //chromaticAberration.intensity.Interp(chromaticAberration.intensity.value, .4f, .1f);
                chromaticAberration.intensity.value = .7f;
            }
            else
            {
                //chromaticAberration.intensity.Interp(chromaticAberration.intensity.value, 0.0f,.1f);
                chromaticAberration.intensity.value = .0f;
            }
        }
    }
}
