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
    public class TooltipBox : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        public void SetText(string titleText, string descText)
        {
            if (title != null)
            {
                title.text = titleText;
            }

            if (description != null)
            {
                description.text = descText;
            }
        }
    }
}
