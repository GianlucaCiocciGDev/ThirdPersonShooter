using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public abstract  class LocomotionManager : MonoBehaviour
    {
        public abstract void HandleMove();
        public abstract void CameraRotation();
        public abstract void HandleGravity();
        public abstract void HandleJump();
    }
}
