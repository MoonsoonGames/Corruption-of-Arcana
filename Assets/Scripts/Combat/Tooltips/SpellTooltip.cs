using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SpellTooltip : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public GameObject sections;

        private void Start()
        {
            oldSections = new List<GameObject>();
        }

        public void SetText(string titleText, Spell spell)
        {
            if (title != null)
            {
                title.text = titleText;
            }

            if (spell != null)
            {
                DeleteOldIcons();
                SetupIcons(spell);
            }
        }

        #region Icons

        public Object sectionPrefab;
        List<GameObject> oldSections;

        public void DeleteOldIcons()
        {
            if (oldSections == null) return;
            if (oldSections.Count == 0) return;

            List<GameObject> newList = new List<GameObject>();

            foreach (GameObject section in oldSections)
            {
                newList.Add(section);
            }

            foreach (GameObject section in newList)
            {
                oldSections.Remove(section);
                Destroy(section.gameObject);
            }

            newList.Clear();
        }

        public void SetupIcons(Spell spell)
        {
            if (sections == null)
                return;

            List<CombatHelperFunctions.SpellIconConstruct> spellConstructs = spell.SpellIcons();

            foreach (var item in spellConstructs)
            {
                //Debug.Log("Module: " + item.value + " X " + item.hitCount + " " + item.effectType + " on " + item.target.ToString());
                GameObject section = Instantiate(sectionPrefab, sections.transform) as GameObject;
                IconConstructor constructor = section.GetComponent<IconConstructor>();

                if (constructor != null)
                    constructor.ConstructSpell(item.value, GetEffectObject(item.effectType), item.hitCount, GetTargetType(item.target));

                oldSections.Add(section);
            }

            List<CombatHelperFunctions.StatusIconConstruct> statusConstructs = spell.EffectIcons();

            foreach (var item in statusConstructs)
            {
                //Debug.Log("Module: " + item.value + " X " + item.hitCount + " " + item.effectType + " on " + item.target.ToString());
                GameObject section = Instantiate(sectionPrefab, sections.transform) as GameObject;
                IconConstructor constructor = section.GetComponent<IconConstructor>();

                if (constructor != null)
                    constructor.ConstructStatus(item.chance, item.effectIcon, item.duration, GetTargetType(item.target), item.effect);

                oldSections.Add(section);
            }

            CombatHelperFunctions.ExecuteIconConstruct executeConstruct = spell.ExecuteIcons();

            if (executeConstruct.threshold > 0)
            {
                //Debug.Log("Module: " + item.value + " X " + item.hitCount + " " + item.effectType + " on " + item.target.ToString());
                GameObject section = Instantiate(sectionPrefab, sections.transform) as GameObject;
                IconConstructor constructor = section.GetComponent<IconConstructor>();

                if (constructor != null)
                    constructor.ConstructExecute(executeConstruct.threshold, GetTargetType(executeConstruct.target));

                oldSections.Add(section);
            }
        }

        public Object physicalIcon, emberIcon, bleakIcon, staticIcon, septicIcon, perfotationIcon, randomIcon, healingIcon, shieldIcon;

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
                case E_DamageTypes.Random:
                    return randomIcon;
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
            string targetString = HelperFunctions.AddSpacesToSentence(targetType.ToString());
            return targetString;

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
