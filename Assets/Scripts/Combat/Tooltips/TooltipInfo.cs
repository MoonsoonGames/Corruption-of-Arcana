using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class TooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        Image icon;
        public string title;
        public string description;

        private void Start()
        {
            icon = GetComponent<Image>();
        }

        /// <summary>
        /// Called when mouse hovers over icon
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TooltipManager.instance == null)
                return;

            TooltipManager.instance.Showtooltip(true, title, description);
        }

        /// <summary>
        /// Called when mouse stops hovering over icon
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.instance == null)
                return;

            TooltipManager.instance.Showtooltip(false, title, description);
        }

        public void EnableRaycasting(bool active)
        {
            icon.raycastTarget = active;
        }
    }
}
