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
        SpellTooltip spellToolTip;

        public GameObject tutorialToolTipObject;
        TooltipBox tutorialToolTip;

        private void Start()
        {
            instance = this;

            if (toolTipObject != null)
            {
                toolTip = toolTipObject.GetComponent<TooltipBox>();
                toolTipObject.SetActive(false);
            }

            if (spellToolTipObject != null)
            {
                spellToolTip = spellToolTipObject.GetComponent<SpellTooltip>();
                spellToolTipObject.SetActive(false);
            }

            if (tutorialToolTipObject != null)
            {
                tutorialToolTip = tutorialToolTipObject.GetComponent<TooltipBox>();
                tutorialToolTipObject.SetActive(false);
            }
        }

        public void ShowTooltip(bool active, string titleText, string descText)
        {
            //Debug.Log("show tooltip - manager");
            if (spellToolTipObject != null)
                spellToolTipObject.SetActive(false);

            if (toolTip == null || (DragManager.instance != null && DragManager.instance.draggedCard != null))
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

        public void ShowTutorialTooltip(bool active, string titleText, string descText)
        {
            tutorialToolTipObject.SetActive(active);

            if (active)
            {
                Debug.Log(titleText + " || " + descText);
                tutorialToolTip.SetText(titleText, descText);
            }
        }

        public void ShowSpellTooltip(bool active, string titleText, Spell spell)
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
                spellToolTip.SetText(titleText, spell);
            }
        }

        public void EnableTooltips(bool active)
        {
            if (toolTipObject != null)
                toolTipObject.SetActive(false);

            TooltipInfo[] allTooltips = GameObject.FindObjectsOfType<TooltipInfo>();

            foreach (TooltipInfo item in allTooltips)
            {
                item.EnableRaycasting(active);
            }
        }
    }
}