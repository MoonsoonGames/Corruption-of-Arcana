using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Interactable
{
    public class LightSwitch : MonoBehaviour, IInteractable
    {
        public GameObject light;
        public void Interacted(GameObject player)
        {
            // Toggles the light
            light.SetActive(!light.activeInHierarchy);
        }
    }
}
