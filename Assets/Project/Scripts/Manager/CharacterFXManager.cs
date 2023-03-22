using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public abstract class CharacterFXManager : MonoBehaviour
    {
        public abstract void PlayMuzzle();
        public abstract void PlayLaser(Vector3 target);
        public abstract void PlayHitEffect(RaycastHit hit);
    }
}
