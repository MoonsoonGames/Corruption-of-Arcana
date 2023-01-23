using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class Billboard : MonoBehaviour
    {
        public bool lockX = false;
        public bool lockY = false;
        public bool lockZ = false;
        Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("Setup", 0.5f);
        }

        void Setup()
        {
            cam = LoadCombatManager.instance.mainCam;
        }

        // Update is called once per frame
        void Update()
        {
            if (cam == null) return;

            //Rotate towards active camera
            Vector3 direction = cam.transform.position - transform.position;
            Quaternion desiredRot = Quaternion.LookRotation(direction);

            if (lockX) desiredRot.x = transform.rotation.x;
            if (lockY) desiredRot.y = transform.rotation.y;
            if (lockZ) desiredRot.z = transform.rotation.z;

            transform.rotation = desiredRot;
        }
    }
}
