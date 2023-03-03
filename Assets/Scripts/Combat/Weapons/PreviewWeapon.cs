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
        public TextMeshProUGUI text;

        private void OnEnable()
        {
            if (weapon == null) { return; }

            if (weapon.image != null) 
            { 
                image.sprite = weapon.image;
                text.gameObject.SetActive(false);
            }
            else { text.text = weapon.weaponName; }
        }

        public void ShowPreviewWeapon()
        {
            selectWeapon.PreviewWeapon(weapon);
        }
    }
}
