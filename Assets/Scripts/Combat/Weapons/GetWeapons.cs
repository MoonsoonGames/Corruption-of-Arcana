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
        public GridLayoutGroup grid;
        public SelectWeapon selectWeapon;
        public Object weaponPrefab;

        public void OpenEquipment()
        {
            for (int i = 0; i < grid.transform.childCount; i++)
            {
                Destroy(grid.transform.GetChild(i).gameObject);
            }

            foreach(var item in DeckManager.instance.unlockedWeapons)
            {
                GameObject weaponObj = GameObject.Instantiate(weaponPrefab, grid.transform) as GameObject;

                weaponObj.GetComponent<PreviewWeapon>().Setup(item, selectWeapon);
            }
        }
    }
}
