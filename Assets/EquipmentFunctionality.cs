using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda;

public class EquipmentFunctionality : MonoBehaviour
{
    public GameObject WeaponsMenu;
    public GameObject[] WeaponPages;  
    public InventoryManager inventoryManager;

    void TogglePage(GameObject[] pages, bool state)
    {
        foreach (GameObject obj in pages)
        {
            obj.SetActive(state);
        }
    }
    public void WeaponBTN(Weapon weapon)
    {
        //SelectedWeapon=*Weapon*
        //ShowCards
    }

    public void AxeTab()
    {
        TogglePage(WeaponPages, false);
        WeaponPages[0].SetActive(true);
    }

    public void ShieldTab()
    {
        TogglePage(WeaponPages, false);
        WeaponPages[1].SetActive(true);
    }

    public void SwordTab()
    {
        TogglePage(WeaponPages, false);
        WeaponPages[2].SetActive(true);
    }
    public void SwordStaffTab()
    {
        TogglePage(WeaponPages, false);
        WeaponPages[3].SetActive(true);
    }

    public void WandTab()
    {
        TogglePage(WeaponPages, false);
        WeaponPages[4].SetActive(true);
    }

    public void Close()
    {
        inventoryManager.InventoryScreen.SetActive(true);
        WeaponsMenu.SetActive(false);
    }
}
