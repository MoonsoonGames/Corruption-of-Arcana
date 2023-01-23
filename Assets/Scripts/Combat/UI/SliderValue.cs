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
    public class SliderValue : MonoBehaviour
    {
        public Slider slider;

        //[HideInInspector]
        public Image standardFill;
        public TextMeshProUGUI text;
        int current, max;

        public void Setup(int maxValue)
        {
            if (slider != null)
                standardFill = slider.fillRect.GetComponent<Image>();
            else
                Debug.LogWarning("No standard slider is set");

            SetSliderMax(maxValue);
        }

        public void SetSliderMax(int value)
        {
            max = value;

            if (slider != null)
                slider.maxValue = max;
            else
                Debug.LogWarning("No standard slider is set");

            if (text != null)
                text.text = max + "/" + max;
            else
                Debug.LogWarning("No standard text is set");
        }

        public void SetSliderValue(int value)
        {
            if (slider != null)
                slider.value = value;
            else
                Debug.LogWarning("No standard slider is set");

            if (text != null)
                text.text = value + "/" + max;
            else
                Debug.LogWarning("No standard text is set");
        }
    }
}
