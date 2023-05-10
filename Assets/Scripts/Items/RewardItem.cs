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
    public class RewardItem : MonoBehaviour
    {
        Image image;
        TextMeshProUGUI amount;

        public void Setup(Object item, int count)
        {
            image = GetComponentInChildren<Image>();
            amount = GetComponentInChildren<TextMeshProUGUI>();

            if (item.GetType() == typeof(Weapon))
            {
                // Add to deck manager list
                Weapon weapon = (Weapon)item;
                name = "Reward (" + weapon.weaponName + ")";
                image.sprite = weapon.image;
            }
            else if (item.GetType() == typeof(Spell))
            {
                Spell currentSpell = (Spell)item;
                name = "Reward (" + currentSpell.spellName + ")";
                switch (currentSpell.cardType)
                {
                    case E_CardTypes.Cards:
                        image.sprite = currentSpell.cardImage;
                        break;
                    case E_CardTypes.Potions:
                        image.sprite = currentSpell.nameImage;
                        break;
                }
            }
            
            if (amount != null)
            {
                amount.text = "X " + count.ToString();

                if (count == 1)
                    amount.color = new Color(0, 0, 0, 0);
            }
                
            else
                Debug.Log("no text");
        }
    }
}
