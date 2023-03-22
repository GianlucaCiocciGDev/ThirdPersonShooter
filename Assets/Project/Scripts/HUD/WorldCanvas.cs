using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class WorldCanvas : MonoBehaviour
    {
        Transform mainCamera;
        void Start()
        {
            mainCamera = Camera.main.transform;
        }
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        }
    }
}
