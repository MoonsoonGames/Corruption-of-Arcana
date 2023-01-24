using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class ChangeTimeScale : MonoBehaviour
    {
        public void SetScaleFromSlider(Slider slider)
        {
            Time.timeScale = slider.value;
        }

        public void SetScaleFromValue(float value)
        {
            Time.timeScale = value;
        }
    }
}
