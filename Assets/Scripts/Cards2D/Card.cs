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
        public Image nameImage;
        public Image background;
        public SpawnArcanaSymbol arcanaSpawner;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI descriptionText;
        public Image cardFaceLowOpacity;
        public Image cardFace;
        public GameObject unavailableOverlay;

        public void Setup()
        {
            if (spell.nameImage != null)
            {
                nameImage.sprite = spell.nameImage;
                nameText.color = new Color(0, 0, 0, 0);
            }
            else
            {
                nameImage.color = new Color(0, 0, 0, 0);
                nameText.text = spell.spellName;
            }
            if (spell.background != null)
                background.sprite = spell.background;
            cardFace.sprite = spell.cardImage;
            arcanaSpawner.SpawnArcanaSymbols(spell.arcanaCost);
            speedText.text = spell.speed.ToString();
            descriptionText.text = IconManager.instance.ReplaceText(spell.spellDescription);

            cardFaceLowOpacity.sprite = spell.cardImage;
            cardFaceLowOpacity.gameObject.SetActive(spell.cardImage != null);
            cardFace.sprite = spell.cardImage;
            ShowSymbols();
            if (cardFace.sprite == null)
                ShowArt(false);
            //SetupIcons();

            gameObject.name = spell.spellName;

            GetComponent<CardDrag2D>().Setup();
            ShowUnavailableOverlay(3);
        }

        public void ShowArt(bool show)
        {
            bool canShow = show;

            if (cardFace.sprite == null)
                canShow = false;

            Color color = Color.white;
            color.a = canShow ? 1 : 0;

            cardFace.color = color;

            //gridLayout.gameObject.SetActive(canShow);
            gridLayout.gameObject.SetActive(false);
        }


        public Object imageObject;
        public GridLayoutGroup gridLayout;

        void ShowSymbols()
        {
            List<Sprite> sprites = new List<Sprite>();

            foreach (var item in spell.spellModules)
            {
                Sprite newSprite = GetEffectObject(item.effectType);

                if (!sprites.Contains(newSprite) && item.value > 0)
                    sprites.Add(newSprite);

                foreach (var status in item.statuses)
                {
                    Sprite statusSprite = status.status.iconSprite;

                    if (!sprites.Contains(statusSprite))
                        sprites.Add(statusSprite);

                    foreach (var effect in status.status.effectModules)
                    {
                        Sprite effectSprite = GetEffectObject(effect.effectType);

                        if (!sprites.Contains(effectSprite))
                            sprites.Add(effectSprite);
                    }
                }
            }

            foreach (var item in sprites)
            {
                GameObject image = Instantiate(imageObject, gridLayout.gameObject.transform) as GameObject;

                foreach (var imageObj in image.GetComponentsInChildren<Image>())
                {
                    imageObj.sprite = item;
                }
            }
        }

        public Sprite physicalIcon, emberIcon, bleakIcon, staticIcon, septicIcon, perfotationIcon, randomIcon, healingIcon, shieldIcon, arcanaIcon;

        public Sprite GetEffectObject(E_DamageTypes effectType)
        {
            switch (effectType)
            {
                case E_DamageTypes.Physical:
                    return physicalIcon;
                case E_DamageTypes.Ember:
                    return emberIcon;
                case E_DamageTypes.Bleak:
                    return bleakIcon;
                case E_DamageTypes.Static:
                    return staticIcon;
                case E_DamageTypes.Septic:
                    return septicIcon;
                case E_DamageTypes.Perforation:
                    return perfotationIcon;
                case E_DamageTypes.Random:
                    return randomIcon;
                case E_DamageTypes.Healing:
                    return healingIcon;
                case E_DamageTypes.Shield:
                    return shieldIcon;
                case E_DamageTypes.Arcana:
                    return arcanaIcon;
                default:
                    return null;
            }
        }

        public void ShowUnavailableOverlay(int availableArcana)
        {
            bool unavailable = availableArcana < spell.arcanaCost && spell.arcanaCost > 0 ? true : false;

            unavailableOverlay.SetActive(unavailable);
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
            //Debug.Log("link info: " + linkIndex);
            if (linkIndex != -1)
            {
                //Debug.Log("show tooltip - link info");
                TMP_LinkInfo linkInfo = descriptionText.textInfo.linkInfo[linkIndex]; // Get the information about the link
                // Do something based on what link ID or Link Text is encountered...

                string id = linkInfo.GetLinkID();
                string split = "$split$";
                string[] parts = id.Split(split);

                string title = parts[0];
                string description = parts[1];

                //Debug.Log(title + " || " + description);
                TooltipManager.instance.ShowTooltip(true, title, description);

                showing = true;
            }
            else
            {
                if (showing)
                {
                    //Debug.Log("Close");
                    TooltipManager.instance.ShowTooltip(false, "Error", "Should not be showing");
                    showing = false;
                }
            }
        }
    }
}