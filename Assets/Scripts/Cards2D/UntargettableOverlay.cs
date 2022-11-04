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
    public class UntargettableOverlay : MonoBehaviour
    {
        public GameObject mask;
        public TextMeshProUGUI text;

        public void SetOverlay(bool active, string message)
        {
            Debug.Log(message);
            mask.SetActive(active);
            text.text = message;
        }
    }
}
