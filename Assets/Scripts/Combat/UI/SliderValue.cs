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
    public class SliderValue : MonoBehaviour
    {
        public Slider slider;

        //[HideInInspector]
        public Image standardFill;

        private void Start()
        {
            if (slider != null)
                standardFill = slider.fillRect.GetComponent<Image>();
            else
                Debug.LogWarning("No standard slider is set");
        }

        public void SetSliderMax(int value)
        {
            if (slider != null)
                slider.maxValue = value;
            else
                Debug.LogWarning("No standard slider is set");
        }

        public void SetSliderValue(int value)
        {
            if (slider != null)
                slider.value = value;
            else
                Debug.LogWarning("No standard slider is set");
        }
    }
}
