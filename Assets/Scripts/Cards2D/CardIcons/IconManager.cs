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
                if (icon.replaceWithText)
                    newText = newText.Replace("$" + icon.replaceText + "$", "<link=\"" + icon.title + "$split$" + icon.description + "\"><color=#00FFFF>" + icon.title + "</color></link>");
                else
                    newText = newText.Replace("$" + icon.replaceText + "$", "<link=\"" + icon.title + "$split$" + icon.description + "\"><sprite index= " + icon.imageID + "></link>");
            }

            return newText;
        }
    }
}
