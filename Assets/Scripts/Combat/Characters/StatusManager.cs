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
    public class StatusManager : MonoBehaviour
    {
        public GameObject iconHolder;
        public Object iconPrefab;
        List<GameObject> statusIcons = new List<GameObject>();

        public void ClearIcons()
        {
            foreach (var item in statusIcons)
            {
                Destroy(item.gameObject);
            }

            statusIcons.Clear();
        }

        public void AddStatus(CombatHelperFunctions.StatusInstance status)
        {
            GameObject icon = Instantiate(iconPrefab, iconHolder.transform) as GameObject;

            Image image = icon.GetComponent<Image>();

            if (image != null)
            {
                image.sprite = status.status.iconSprite;
            }

            TooltipInfo info = icon.GetComponent<TooltipInfo>();

            if (info != null)
            {
                info.title = status.status.effectName + " for " + status.duration + " turns";
                info.description = status.status.effectDescription;
            }

            statusIcons.Add(icon);
        }
    }
}
