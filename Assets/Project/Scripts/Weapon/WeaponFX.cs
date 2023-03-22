using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class WeaponFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] muzzleFlash;
        [SerializeField] private TrailRenderer tracerEffect;
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] Transform startRayCastPoint;
        public void PlayMuzzle()
        {
            foreach (ParticleSystem p in muzzleFlash)
            {
                p.Emit(1);
            }
        }
        public void PlayLaser(Vector3 target)
        {
            var tracer = Instantiate(tracerEffect, startRayCastPoint.position, Quaternion.identity);
            tracer.AddPosition(startRayCastPoint.position);
            tracer.transform.position = target;
        }
        public void PlayHitEffect(RaycastHit hit)
        {
            hitEffect.transform.position = hit.point;
            hitEffect.transform.forward = hit.normal;
            hitEffect.Emit(1);
        }
    }
}
