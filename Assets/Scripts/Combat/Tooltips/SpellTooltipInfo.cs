using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SpellTooltipInfo : TooltipInfo, IPointerEnterHandler, IPointerExitHandler
    {
        public void Setup(CombatHelperFunctions.SpellInstance spell, bool revealed)
        {
            title = spell.caster.stats.characterName;

            if (revealed)
                description = spell.caster.stats.characterName + " is casting " + spell.spell.spellName + " on " + spell.target.stats.characterName + " (" + spell.spell.speed + ")";
            else
                description = spell.caster.stats.characterName + " is casting a spell" + " (" + spell.spell.speed + ")";
        }

        /// <summary>
        /// Called when mouse hovers over icon
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (TooltipManager.instance == null)
                return;

            TooltipManager.instance.ShowSpellTooltip(true, title, description);
        }

        /// <summary>
        /// Called when mouse stops hovering over icon
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.instance == null)
                return;

            TooltipManager.instance.ShowSpellTooltip(false, title, description);
        }
    }
}
