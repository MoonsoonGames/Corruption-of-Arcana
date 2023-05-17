using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Necropanda;
using Necropanda.Player;

public class InventoryManager : MonoBehaviour
{
    public Necropanda.Interfaces.HUDInterface HUDScript;
    public GameObject MainHUD;
    public GameObject InventoryScreen;
    public GameObject Journal;
    public GameObject PotionsMenu;
    public GameObject MapScreen;
    public MapSelector mapSelector;
    TEMP_OpenDeckbuilding openDeckbuilding;

    public void Start()
    {
        openDeckbuilding = GameObject.FindObjectOfType<TEMP_OpenDeckbuilding>(true);
    }
    public void JournalBTN()
    {
        InventoryScreen.SetActive(false);
        Journal.SetActive(true);
    }

    public void PotionsBTN()
    {
        InventoryScreen.SetActive(false);
        openDeckbuilding.OpenCloseMenu(true, PotionsMenu);
        Debug.Log("open potions menu");
    }

    public void WeaponsBTN()
    {
        InventoryScreen.SetActive(false);
        openDeckbuilding.OpenCloseMenu(true, openDeckbuilding.weaponsMenu);
        Debug.Log("open weapons menu");
    }

    public void DeckBuildingBTN()
    {
        InventoryScreen.SetActive(false);
        openDeckbuilding.OpenCloseMenu(true, openDeckbuilding.deckbuildingMenu);
    }

    public void MapBTN()
    {
        if (mapSelector == null)
        {
            mapSelector = FindObjectOfType<MapSelector>();
        }

        InventoryScreen.SetActive(false);
        openDeckbuilding.OpenCloseMenu(true, MapScreen);
        mapSelector.UpdateMapImage();
    }

    public void CloseBTN()
    {
        InventoryScreen.SetActive(false);
        MainHUD.SetActive(true);

        HUDScript.player.paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
