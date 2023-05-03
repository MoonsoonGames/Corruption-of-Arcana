using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public void Setup(Object item)
        {
            image = GetComponentInChildren<Image>();

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
        }
    }
}
