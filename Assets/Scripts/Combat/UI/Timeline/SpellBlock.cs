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
    public class SpellBlock : MonoBehaviour
    {
        CombatHelperFunctions.SpellInstance spell;
        Image image;

        public void SetInfo(CombatHelperFunctions.SpellInstance spell)
        {
            image = GetComponentInChildren<Image>();

            image.sprite = spell.spell.timelineIcon;

            SpellTooltipInfo tooltip = GetComponentInChildren<SpellTooltipInfo>();

            tooltip.Setup(spell);

            this.spell = spell;
        }

        public void HighlightTargets(bool highlight)
        {
            if (highlight)
            {
                spell.target.GetHealth().GetColorFlash().Highlight(Color.red);
                spell.caster.GetHealth().GetColorFlash().Highlight(Color.green);
            }
            else
            {
                spell.caster.GetHealth().GetColorFlash().RemoveHighlightColour();
                spell.target.GetHealth().GetColorFlash().RemoveHighlightColour();
            }
        }
    }
}
