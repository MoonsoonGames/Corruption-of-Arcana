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
    public class SelectWeapon : MonoBehaviour
    {
        public Weapon previewWeapon;

        public TextMeshProUGUI nameText, descriptionText;
        public Image image;

        public void PreviewWeapon(Weapon weapon)
        {
            previewWeapon = weapon;

            nameText.text = weapon.weaponName;
            descriptionText.text = weapon.description;
            image.sprite = previewWeapon.image;
        }

        public void EquipWeapon()
        {
            if (previewWeapon == null) { return; }

            previewWeapon.Equip();
        }
    }
}
