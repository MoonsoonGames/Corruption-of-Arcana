using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class TooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected Image icon;
        public string title;
        public string description;

        protected virtual void Start()
        {
            icon = GetComponent<Image>();
        }

        /// <summary>
        /// Called when mouse hovers over icon
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {

            //Debug.Log("show tooltip - tooltip info");
            if (TooltipManager.instance == null)
                return;

            TooltipManager.instance.ShowTooltip(true, title, description);
        }

        /// <summary>
        /// Called when mouse stops hovering over icon
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.instance == null)
                return;

            TooltipManager.instance.ShowTooltip(false, title, description);
        }

        public virtual void EnableRaycasting(bool active)
        {
            if (icon != null)
                icon.raycastTarget = active;
        }

        private void OnDisable()
        {
            TooltipManager.instance.ShowTooltip(false, "", "");
        }

        private void OnDestroy()
        {
            TooltipManager.instance.ShowTooltip(false, "", "");
        }
    }
}
