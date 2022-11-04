using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SimulateValues : MonoBehaviour
    {
        public TextMeshProUGUI dmg, heal, shield;

        public void DisplayValues(int damage, int healing, int shd)
        {
            dmg.gameObject.SetActive(damage > 0);
            dmg.text = damage.ToString();

            heal.gameObject.SetActive(healing > 0);
            heal.text = healing.ToString();

            shield.gameObject.SetActive(shd > 0);
            shield.text = shd.ToString();
        }
    }
}
