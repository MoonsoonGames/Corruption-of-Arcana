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
    public class GetWeapons : MonoBehaviour
    {
        public SelectWeapon selectWeapon;

        public void OpenEquipment()
        {
            PreviewWeapon[] previewWeapons = GetComponentsInChildren<PreviewWeapon>(true);

            foreach (var item in previewWeapons)
            {
                item.Setup(selectWeapon);
            }
        }
    }
}
