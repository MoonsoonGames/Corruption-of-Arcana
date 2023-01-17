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
        Spell spell;
        bool revealed = false;

        public void Setup(CombatHelperFunctions.SpellInstance newspell)
        {
            spell = newspell.spell;
            title = newspell.caster.stats.characterName;

            description = newspell.caster.stats.characterName + " is casting " + newspell.spell.spellName + " on " + newspell.target.stats.characterName + " (" + newspell.spell.speed + ")";
        }

        /// <summary>
        /// Called when mouse hovers over icon
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("show tooltip - spell info");
            if (TooltipManager.instance == null)
                return;

            if (revealed)
                TooltipManager.instance.ShowSpellTooltip(true, title, spell);
            else
                TooltipManager.instance.ShowTooltip(true, title, description);
        }

        /// <summary>
        /// Called when mouse stops hovering over icon
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipManager.instance == null)
                return;

            if (revealed)
                TooltipManager.instance.ShowSpellTooltip(false, title, spell);
            else
                TooltipManager.instance.ShowTooltip(false, title, description);
        }
    }
}
