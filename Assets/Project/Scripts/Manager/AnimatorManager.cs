using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public abstract class AnimatorManager : MonoBehaviour
    {
        [HideInInspector]public Animator BaseAnimator;
        protected virtual void Awake()
        {
            BaseAnimator = GetComponent<Animator>();
        }
        protected abstract void AssignAnimationIDs();
    }
}

