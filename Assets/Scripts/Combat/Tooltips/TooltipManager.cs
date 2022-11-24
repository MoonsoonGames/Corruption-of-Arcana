using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
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

        public GameObject spellToolTipObject;
        TooltipBox spellToolTip;

        private void Start()
        {
            instance = this;
            toolTip = toolTipObject.GetComponent<TooltipBox>();
            toolTipObject.SetActive(false);

            spellToolTip = spellToolTipObject.GetComponent<TooltipBox>();
            spellToolTipObject.SetActive(false);
        }

        public void ShowTooltip(bool active, string titleText, string descText)
        {
            if (spellToolTipObject != null)
                spellToolTipObject.SetActive(false);

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

        public void ShowSpellTooltip(bool active, string titleText, string descText)
        {
            if (toolTipObject != null)
                toolTipObject.SetActive(false);

            if (spellToolTip == null || DragManager.instance.draggedCard != null)
            {
                spellToolTipObject.SetActive(false);
                return;
            }

            spellToolTipObject.SetActive(active);

            if (active)
            {
                spellToolTip.SetText(titleText, descText);
            }
        }

        public void EnableTooltips(bool active)
        {
            toolTipObject.SetActive(false);

            TooltipInfo[] allTooltips = GameObject.FindObjectsOfType<TooltipInfo>();

            foreach (TooltipInfo item in allTooltips)
            {
                item.EnableRaycasting(active);
            }
        }
    }
}