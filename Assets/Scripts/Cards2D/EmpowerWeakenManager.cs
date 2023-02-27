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
    public class EmpowerWeakenManager : MonoBehaviour
    {
        public bool ignore = true;
        public GameObject empowerOverlay;
        public GameObject weakenOverlay;
        public GameObject reflectOverlay;

        public void DisplayEmpower(bool active)
        {
            if (ignore) return;

            empowerOverlay.SetActive(active);
        }

        public void DisplayWeaken(bool active)
        {
            if (ignore) return;

            weakenOverlay.SetActive(active);
        }

        public void DisplayReflect(bool active)
        {
            if (ignore) return;

            reflectOverlay.SetActive(active);
        }
    }
}
