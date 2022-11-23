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
    public class IconConstructor : MonoBehaviour
    {
        public TextMeshProUGUI value;
        public GameObject effectIcon;
        public TextMeshProUGUI hitCount;
        public TextMeshProUGUI target;

        public void Construct(int newValue, Object effectPrefab, int newHitCount, string newTarget)
        {
            if (newHitCount > 1)
                value.text = newValue.ToString() + " X" + newHitCount.ToString();
            else
                value.text = newValue.ToString();

            target.text = newTarget;

            Instantiate(effectPrefab, effectIcon.transform);
        }
    }
}
