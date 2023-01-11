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
        Image image;
        TextMeshProUGUI text;

        public void SetInfo(bool revealed, Color color, string basicText, CombatHelperFunctions.SpellInstance spell)
        {
            image = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();

            image.color = color;
            text.text = basicText;

            SpellTooltipInfo tooltip = GetComponentInChildren<SpellTooltipInfo>();

            tooltip.Setup(spell, revealed);
        }
    }
}
