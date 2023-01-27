using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
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

        public void ConstructSpell(int newValue, Object effectPrefab, int newHitCount, string newTarget)
        {
            if (newHitCount > 1)
                value.text = newValue.ToString() + " X " + newHitCount.ToString();
            else
                value.text = newValue.ToString();

            target.text = "on " + newTarget;

            Instantiate(effectPrefab, effectIcon.transform);
        }

        public void ConstructStatus(bool applyOverShield, Object effectPrefab, int duration, string newTarget, StatusEffects status)
        {
            value.text = applyOverShield ? "" : "This status is resisted by shields";

            target.text = "on " + newTarget;

            GameObject icon = Instantiate(effectPrefab, effectIcon.transform) as GameObject;
            TooltipInfo tooltip = icon.GetComponent<TooltipInfo>();

            if (tooltip != null)
            {
                tooltip.title = status.effectName;
                tooltip.description = status.effectDescription;
            }
        }

        public Object executePrefab;

        public void ConstructExecute(float newThreshold, string newTarget)
        {
            if (newThreshold > 0)
            {
                float threshold = newThreshold * 100;
                value.text = threshold.ToString() + "%";

                target.text = "on " + newTarget;

                Instantiate(executePrefab, effectIcon.transform);
            }
        }
    }
}
