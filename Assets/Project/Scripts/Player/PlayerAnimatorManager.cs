using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        int _VerticalAnimID;
        int _HorizontalAnimID;
        int _IsInteractAnimID;
        int _JumpAnimID;
        int _SwitchWeaponAnimID;
        int _ReloadWeaponAnimID;
        int _RestoreAnimID;

        protected override void Awake()
        {
            base.Awake();
            AssignAnimationIDs();
        }
        protected override void AssignAnimationIDs()
        {
            _VerticalAnimID = Animator.StringToHash("Vertical");
            _HorizontalAnimID = Animator.StringToHash("Horizontal");
            _IsInteractAnimID = Animator.StringToHash("IsInteract");
            _JumpAnimID = Animator.StringToHash("Jump");
            _SwitchWeaponAnimID = Animator.StringToHash("Switch Weapon");
            _ReloadWeaponAnimID= Animator.StringToHash("Reload Weapon");
            _RestoreAnimID= Animator.StringToHash("Restore");
        }
        public bool GetIsInteract()=> BaseAnimator.GetBool(_IsInteractAnimID);
        public void UpdateAnimatorValue(float h, float v)
        {
            float valueX = Mathf.Clamp(h, -1, 1);
            float valueY = Mathf.Clamp(v, -1, 1);
            BaseAnimator.SetFloat(_HorizontalAnimID, valueX, .1f, Time.deltaTime);
            BaseAnimator.SetFloat(_VerticalAnimID, valueY , .1f, Time.deltaTime);
        }
        public void SwitchWeapon()
        {
            BaseAnimator.SetBool(_IsInteractAnimID, true);
            BaseAnimator.SetTrigger(_SwitchWeaponAnimID);
        }
        public void ReloadWeapon()
        {
            BaseAnimator.SetBool(_IsInteractAnimID, true);
            BaseAnimator.SetTrigger(_ReloadWeaponAnimID);
        }
        public void Restore()
        {
            BaseAnimator.SetBool(_IsInteractAnimID, true);
            BaseAnimator.SetTrigger(_RestoreAnimID);
        }
        public void Jump()
        {
            BaseAnimator.SetTrigger(_JumpAnimID);
        }
        public void PlayTargetAnimation(string targetAnim)
        {
            BaseAnimator.CrossFade(targetAnim, 0.2f);
        }
    }
}

