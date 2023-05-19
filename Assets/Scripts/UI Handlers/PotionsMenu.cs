using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class PotionsMenu : MonoBehaviour
    {
        TEMP_OpenDeckbuilding openDeckbuilding;
        InventoryManager inventoryManager;
        public void Start()
        {
            inventoryManager = GameObject.FindObjectOfType<InventoryManager>(true);
            openDeckbuilding = GameObject.FindObjectOfType<TEMP_OpenDeckbuilding>(true);
        }
        public void Close()
        {
            inventoryManager.InventoryScreen.SetActive(true);
            openDeckbuilding.OpenCloseMenu(false, this.gameObject);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
