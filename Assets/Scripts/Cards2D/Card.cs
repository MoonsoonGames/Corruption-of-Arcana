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
        public SpawnArcanaSymbol arcanaSpawner;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI descriptionText;
        public Image cardBackground;
        public Image cardFace;

        public void Setup()
        {
            nameText.text = spell.spellName;
            arcanaSpawner.SpawnArcanaSymbols(spell.arcanaCost);
            speedText.text = spell.speed.ToString();
            descriptionText.text = IconManager.instance.ReplaceText(spell.spellDescription);
            cardFace.sprite = spell.cardImage;
            //SetupIcons();

            gameObject.name = spell.spellName;

            GetComponent<CardDrag2D>().Setup();
        }

        public void ShowArt(bool show)
        {
            bool canShow = show;

            if (cardFace.sprite == null)
                canShow = false;

            Color color = Color.white;
            color.a = canShow ? 1 : 0;

            cardFace.color = color;
        }

        public void CastSpell(Character target, Character caster)
        {
            //spell.CastSpell(target, caster);
        }

        public void SetupIcons()
        {
            if (cardFace == null)
                return;
            
            descriptionText.text = IconManager.instance.ReplaceText(spell.spellDescription);
        }

        bool showing = false;

        private void Update()
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(descriptionText, Input.mousePosition, DragManager.instance.UICam);
            Debug.Log("link info: " + linkIndex);
            if (linkIndex != -1)
            {
                Debug.Log("show tooltip - link info");
                TMP_LinkInfo linkInfo = descriptionText.textInfo.linkInfo[linkIndex]; // Get the information about the link
                // Do something based on what link ID or Link Text is encountered...

                string id = linkInfo.GetLinkID();
                string split = "$split$";
                string[] parts = id.Split(split);

                string title = parts[0];
                string description = parts[1];

                Debug.Log(title + " || " + description);
                TooltipManager.instance.ShowTooltip(true, title, description);

                showing = true;
            }
            else
            {
                if (showing)
                {
                    Debug.Log("Close");
                    TooltipManager.instance.ShowTooltip(false, "Error", "Should not be showing");
                    showing = false;
                }
            }
        }
    }
}