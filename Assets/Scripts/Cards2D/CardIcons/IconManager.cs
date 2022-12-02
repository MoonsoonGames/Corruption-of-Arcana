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
    public class IconManager : MonoBehaviour
    {
        public static IconManager instance { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        public CombatHelperFunctions.IconToolTip[] iconTooltips;

        public string ReplaceText(string text)
        {
            string newText = text;

            foreach (var icon in iconTooltips)
            {
                newText = newText.Replace("$" + icon.replaceText + "$", "<link=\"" + icon.title + "$split$" + icon.description + "\"><sprite index= " + icon.imageID + "></link>");
            }

            return newText;
        }

        //public Object physicalIcon, emberIcon, glacialIcon, staticIcon, septicIcon, phantomIcon, randomIcon, healingIcon, shieldIcon, executeIcon;
        /*
        public Object GetEffectObject(string effectType)
        {
            
            switch (effectType)
            {
                case "$physical$":
                    return physicalIcon;
                case "$ember$":
                    return emberIcon;
                case "$glacial$":
                    return glacialIcon;
                case "$static$":
                    return staticIcon;
                case "$spetic$":
                    return septicIcon;
                case "$phantom$":
                    return phantomIcon;
                case "$random$":
                    return randomIcon;
                case "$healing$":
                    return healingIcon;
                case "$shield$":
                    return shieldIcon;
                case "$execute$":
                    return executeIcon;
                default:
                    return null;
            }
        }
        */

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
    }
}
