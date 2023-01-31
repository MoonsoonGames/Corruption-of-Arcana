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
    public class UtilityScript : MonoBehaviour
    {
        public HelperFunctions.UtilityModule[] utilityModules;
        bool moving = false;
        public Vector2 offset;

        private void Start()
        {
            List<HelperFunctions.UtilityModule> newList = new List<HelperFunctions.UtilityModule>();

            foreach (var module in utilityModules)
            {
                HelperFunctions.UtilityModule moduleCopy = new HelperFunctions.UtilityModule();

                moduleCopy.type = module.type;
                moduleCopy.axes = module.axes;
                moduleCopy.speed = module.speed;
                moduleCopy.time = module.time;
                moduleCopy.currentTime = module.currentTime + Random.Range(offset.x, offset.y);
                moduleCopy.forward = module.forward;

                newList.Add(moduleCopy);
            }

            for (int i = 0; i < newList.Count; i++)
            {
                utilityModules[i] = newList[i];
            }

            newList.Clear();

            float offsetFloat = Random.Range(offset.x, offset.y);
            Invoke("StartScript", offsetFloat);
        }

        void StartScript()
        {
            for (int i = 0; i < utilityModules.Length; i++)
            {
                if (utilityModules[i].time <= 0)
                {
                    utilityModules[i].time = Mathf.Infinity;
                }
            }

            moving = true;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                for (int i = 0; i < utilityModules.Length; i++)
                {
                    float speed = utilityModules[i].speed;
                    if (utilityModules[i].currentTime >= utilityModules[i].time)
                    {
                        utilityModules[i].currentTime = 0;
                        utilityModules[i].forward = !utilityModules[i].forward;
                    }

                    if (!utilityModules[i].forward)
                    {
                        speed = -utilityModules[i].speed;
                    }

                    float x = speed * utilityModules[i].axes.x;
                    float y = speed * utilityModules[i].axes.y;
                    float z = speed * utilityModules[i].axes.z;

                    Vector3 newVector = new Vector3(x, y, z);

                    switch (utilityModules[i].type)
                    {
                        case E_UtilityScripts.Position:
                            transform.position += newVector;
                            break;
                        case E_UtilityScripts.Rotation:
                            Quaternion newQuaternion = Quaternion.Euler(transform.rotation.eulerAngles + newVector);
                            transform.rotation = newQuaternion;
                            break;
                        case E_UtilityScripts.Scale:
                            transform.localScale += newVector;
                            break;
                    }

                    utilityModules[i].currentTime += Time.deltaTime;
                }
            }
        }
    }
}
