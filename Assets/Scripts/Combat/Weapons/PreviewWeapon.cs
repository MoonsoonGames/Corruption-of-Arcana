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
    public class PreviewWeapon : MonoBehaviour
    {
        public Weapon weapon;
        public SelectWeapon selectWeapon;

        public Image image;

        public void Setup(SelectWeapon selectWeapon)
        {
            this.selectWeapon = selectWeapon;

            if (weapon == null) 
            { 
                image.color = new Color(0, 0, 0, 0);
                return;
            }

            if (weapon.image != null) 
            {
                image.sprite = weapon.image;
                image.color = DeckManager.instance.unlockedWeapons.Contains(weapon) ? new Color(255, 255, 255, 255) : new Color(0, 0, 0, 255);
            }
        }

        public void ShowPreviewWeapon()
        {
            if (DeckManager.instance.unlockedWeapons.Contains(weapon))
                selectWeapon.PreviewWeapon(weapon);
        }
    }
}
