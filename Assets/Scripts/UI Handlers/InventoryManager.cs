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
    TEMP_OpenDeckbuilding openDeckbuilding;
    public PlayerController Player;

    public void Start()
    {
        openDeckbuilding = GameObject.FindObjectOfType<TEMP_OpenDeckbuilding>();
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

    public void CloseBTN()
    {
        InventoryScreen.SetActive(false);
        MainHUD.SetActive(true);

        Player.paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        while(InventoryScreen.activeSelf == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
    }
}
