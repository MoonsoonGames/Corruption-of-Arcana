using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class ShieldUI : MonoBehaviour
    {
        Image image;
        TextMeshProUGUI text;

        public void Setup(int initialValue)
        {
            image = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();

            SetShield(initialValue);
        }

        public void SetShield(int value)
        {
            if (value > 0)
            {
                image.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
                text.text = value.ToString();
            }
            else
            {
                image.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
            }
        }
    }
}
