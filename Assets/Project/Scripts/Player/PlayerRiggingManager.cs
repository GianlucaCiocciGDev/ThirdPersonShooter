using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Gdev
{
    public class PlayerRiggingManager : MonoBehaviour
    {
        [SerializeField]private  Rig rigAimLayer;
        public RigBuilder RigBuilder;
        public TwoBoneIKConstraint LeftHandConstraint;
        public TwoBoneIKConstraint RightHandConstraint;

        private void Awake()
        {
            RigBuilder = GetComponent<RigBuilder>();
        }
        public void EraseIK(bool erase, float time)
        {
            float finalValue = erase ? 0.0f : 1.0f;
            rigAimLayer.weight = Mathf.Lerp(rigAimLayer.weight, finalValue, Time.deltaTime * time);
        }
        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
        {
            if (isTwoHandingWeapon)
            {

                if (RightHandConstraint != null && rightHandTarget != null)
                {
                    RightHandConstraint.data.target = rightHandTarget.transform.GetChild(0).transform;
                    RightHandConstraint.data.hint = rightHandTarget.transform.GetChild(1).transform;
                    RightHandConstraint.data.targetPositionWeight = 1;
                    RightHandConstraint.data.targetRotationWeight = 1;
                }


                if (LeftHandConstraint != null && leftHandTarget != null)
                {
                    LeftHandConstraint.data.target = leftHandTarget.transform.GetChild(0).transform;
                    LeftHandConstraint.data.hint = leftHandTarget.transform.GetChild(1).transform;
                    LeftHandConstraint.data.targetPositionWeight = 1;
                    LeftHandConstraint.data.targetRotationWeight = 1;
                }
            }
            rigAimLayer.weight = 1;
            RigBuilder.Build();
            
        }
        public virtual void EraseHandIKForWeapon()
        {
            rigAimLayer.weight = 0;
            if (RightHandConstraint.data.target != null)
            {
                RightHandConstraint.data.targetPositionWeight = 0;
                RightHandConstraint.data.targetRotationWeight = 0;
                RightHandConstraint.data.hintWeight = 0;
            }
            if (LeftHandConstraint.data.target != null)
            {
                LeftHandConstraint.data.targetPositionWeight = 0;
                LeftHandConstraint.data.targetRotationWeight = 0;
                LeftHandConstraint.data.hintWeight = 0;
            }
        }
    }
}
