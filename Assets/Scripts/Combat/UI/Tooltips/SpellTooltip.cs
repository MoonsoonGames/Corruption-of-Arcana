using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SpellTooltip : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public GameObject sections;

        public bool followMouse = false;
        RectTransform bg;
        public RectTransform canvas;
        public float xMultiplier = 1.87f;
        public float yMultiplier = 2.475f;

        private void Update()
        {
            if (followMouse)
            {
                //https://www.youtube.com/watch?v=dzkFdkwzVhs

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = mousePosition;

                Vector2 tooltipPos = transform.GetComponent<RectTransform>().anchoredPosition;
                float width = bg.rect.width * xMultiplier;
                float height = bg.rect.height * yMultiplier;

                if (tooltipPos.x + width > canvas.rect.width)
                {
                    tooltipPos.x = canvas.rect.width - width;
                }
                if (tooltipPos.y + height > canvas.rect.height)
                {
                    tooltipPos.y = canvas.rect.height - height;
                }
                transform.GetComponent<RectTransform>().anchoredPosition = tooltipPos;
            }
        }

        private void Start()
        {
            oldSections = new List<GameObject>();
            bg = GetComponent<RectTransform>();
        }

        public void SetText(string titleText, Spell spell)
        {
            if (title != null)
            {
                title.text = IconManager.instance.ReplaceText(titleText);
            }

            if (spell != null)
            {
                DeleteOldIcons();
                SetupIcons(spell);
            }
        }

        #region Icons

        public Object sectionPrefab;
        List<GameObject> oldSections = new List<GameObject>();

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

            #region Spell Icons

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

            #endregion

            #region Status Icons

            List<CombatHelperFunctions.StatusIconConstruct> statusConstructs = spell.EffectIcons();

            foreach (var item in statusConstructs)
            {
                //Debug.Log("Module: " + item.value + " X " + item.hitCount + " " + item.effectType + " on " + item.target.ToString());
                GameObject section = Instantiate(sectionPrefab, sections.transform) as GameObject;
                IconConstructor constructor = section.GetComponent<IconConstructor>();

                if (constructor != null)
                    constructor.ConstructStatus(item.applyOverShield, item.effectIcon, item.duration, GetTargetType(item.target), item.effect);

                oldSections.Add(section);
            }

            #endregion

            #region Execute Icon

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

            #endregion

            #region Summon Icons

            Dictionary<CharacterStats, int> summonConstructs = spell.SummonIcons();

            foreach (var item in summonConstructs)
            {
                //Debug.Log("Module: " + item.value + " X " + item.hitCount + " " + item.effectType + " on " + item.target.ToString());
                GameObject section = Instantiate(sectionPrefab, sections.transform) as GameObject;
                IconConstructor constructor = section.GetComponent<IconConstructor>();

                if (constructor != null)
                    constructor.ConstructSummon(item.Key, item.Value);

                oldSections.Add(section);
            }

            #endregion
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
