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
            speedText.text = "Speed: " + spell.speed.ToString();
            descriptionText.text = spell.spellDescription;
            if (spell.overrideColor)
                cardBackground.color = spell.timelineColor;

            SetupIcons();

            gameObject.name = spell.spellName;

            GetComponent<CardDrag2D>().Setup();
        }

        public void CastSpell(Character target, Character caster)
        {
            //spell.CastSpell(target, caster);
        }

        #region Icons

        public Object sectionPrefab;

        public void SetupIcons()
        {
            if (spell.cardImage == null)
                return;

            List<CombatHelperFunctions.IconConstruct> iconConstructs = spell.GenerateIcons();

            foreach(var item in iconConstructs)
            {
                Debug.Log("Module: " + item.value + " " + item.effectType + " on " + item.target.ToString());
                GameObject section = Instantiate(sectionPrefab, this.transform) as GameObject;
                IconConstructor constructor = section.GetComponent<IconConstructor>();

                constructor.Construct(item.value, GetEffectObject(item.effectType), GetTargetType(item.target));
            }
        }

        public Object physicalIcon, emberIcon, bleakIcon, staticIcon, septicIcon, perfotationIcon, healingIcon, shieldIcon;

        public Object GetEffectObject(E_DamageTypes effectType)
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
                case E_DamageTypes.Healing:
                    return healingIcon;
                case E_DamageTypes.Shield:
                    return shieldIcon;
                default:
                    return null;
            }
        }

        public string GetTargetType(E_SpellTargetType targetType)
        {
            return targetType.ToString();

            /*
            switch (targetType)
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
                case E_DamageTypes.Healing:
                    return healingIcon;
                case E_DamageTypes.Shield:
                    return shieldIcon;
                default:
                    return null;
            }
            */
        }

        #endregion
    }
}