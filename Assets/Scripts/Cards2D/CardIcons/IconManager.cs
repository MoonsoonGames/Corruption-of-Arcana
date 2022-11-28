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

        public string[] damageTitles;
        public string[] damageDescriptions;

        public string ReplaceText(string text)
        {
            string newText = text;


            #region replaceText

            newText = newText.Replace("$healing$", "<link=\"" + damageTitles[0] + "$split$" + damageDescriptions[0] + "\"><sprite index= 0></link>");
            newText = newText.Replace("$shield$", "<link=\"" + damageTitles[1] + "$split$" + damageDescriptions[1] + "\"><sprite index= 1></link>");
            newText = newText.Replace("$physical$", "<link=\"" + damageTitles[2] + "$split$" + damageDescriptions[2] + "\"><sprite index= 2></link>");
            newText = newText.Replace("$phantom$", "<link=\"" + damageTitles[3] + "$split$" + damageDescriptions[3] + "\"><sprite index= 3></link>");
            newText = newText.Replace("$static$", "<link=\"" + damageTitles[4] + "$split$" + damageDescriptions[4] + "\"><sprite index= 4></link>");
            newText = newText.Replace("$glacial$", "<link=\"" + damageTitles[5] + "$split$" + damageDescriptions[5] + "\"><sprite index= 5></link>");
            newText = newText.Replace("$ember$", "<link=\"" + damageTitles[6] + "$split$" + damageDescriptions[6] + "\"><sprite index= 6></link>");
            newText = newText.Replace("$septic$", "<link=\"" + damageTitles[7] + "$split$" + damageDescriptions[7] + "\"><sprite index= 7></link>");

            #endregion

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
