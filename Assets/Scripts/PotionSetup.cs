using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class PotionSetup : MonoBehaviour
    {
        public Spell potion;
        public E_PotionType type;

        public TMPro.TMP_Text potName;
        public TMPro.TMP_Text potDesc;
        public TMPro.TMP_Text potAmont;

        public void Setup()
        {
            string potionName = potion.spellName;
            string desc = potion.spellDescription;
            int amount = PotionManager.instance.GetPotionCount(type);
        }

        public void Start()
        { 
            potName.text = potion.spellName;
            potDesc.text = potion.spellDescription;
            potAmont.text = PotionManager.instance.GetPotionCount(type).ToString();
        }
    }
}
