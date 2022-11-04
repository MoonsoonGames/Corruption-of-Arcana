using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EmpowerWeakenManager : MonoBehaviour
    {
        public GameObject empowerOverlay;
        public GameObject weakenOverlay;
        public GameObject reflectOverlay;

        public void DisplayEmpower(bool active)
        {
            empowerOverlay.SetActive(active);
        }

        public void DisplayWeaken(bool active)
        {
            weakenOverlay.SetActive(active);
        }

        public void DisplayReflect(bool active)
        {
            reflectOverlay.SetActive(active);
        }
    }
}
