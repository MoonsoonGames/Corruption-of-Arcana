using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class TooltipBox : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        public bool followMouse = false;

        private void Update()
        {
            if (followMouse)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = mousePosition;
            }
        }

        public void SetText(string titleText, string descText)
        {
            if (title != null)
            {
                title.text = IconManager.instance.ReplaceText(titleText);
            }

            if (description != null)
            {
                description.text = IconManager.instance.ReplaceText(descText);
            }
        }
    }
}
