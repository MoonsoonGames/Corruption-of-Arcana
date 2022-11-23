using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager instance;

        public GameObject toolTipObject;
        TooltipBox toolTip;

        private void Start()
        {
            instance = this;
            toolTip = toolTipObject.GetComponent<TooltipBox>();
            toolTipObject.SetActive(false);
        }

        public void Showtooltip(bool active, string titleText, string descText)
        {
            if (toolTip == null || DragManager.instance.draggedCard != null)
            {
                toolTipObject.SetActive(false);
                return;
            }

            toolTipObject.SetActive(active);

            if (active)
            {
                toolTip.SetText(titleText, descText);
            }
        }

        public void EnableTooltips(bool active)
        {
            toolTipObject.SetActive(false);

            TooltipInfo[] allTooltips = GameObject.FindObjectsOfType<TooltipInfo>();

            foreach (var item in allTooltips)
            {
                item.EnableRaycasting(active);
            }
        }
    }
}