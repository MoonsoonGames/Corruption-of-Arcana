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
    public class Card : MonoBehaviour
    {
        [HideInInspector]
        public Spell spell;

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI arcanaText;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI descriptionText;
        public Image cardBackground;
        public Image cardFace;

        public void Setup()
        {
            nameText.text = spell.spellName;
            arcanaText.text = spell.arcanaCost.ToString();
            speedText.text = "Speed: " + spell.speed.ToString();
            descriptionText.text = spell.spellDescription;
            if (spell.overrideColor)
                cardBackground.color = spell.timelineColor;
            if (spell.cardImage != null)
            {
                cardFace.sprite = spell.cardImage;
                cardFace.color = Color.white;
            }

            gameObject.name = spell.spellName;

            GetComponent<CardDrag2D>().Setup();
        }

        public void CastSpell(Character target, Character caster)
        {
            spell.CastSpell(target, caster);
        }
    }
}