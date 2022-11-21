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
    public class WaterSwitch : MonoBehaviour, IInteractable
    {
        public WaterLevel[] water;

        public void Interacted(GameObject player)
        {
            if (water.Length > 0)
            {
                foreach (WaterLevel level in water)
                    level.AdjustWaterLevel();
            }
            else
            {
                //There is no water component
                Debug.LogWarning("No water components have been set");
            }
        }
    }
}