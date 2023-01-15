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
    //public GameObject PotionScreen;
    //public GameObject WeaponsMenu;
    //public GameObject DeckBuildingScreen;

    public PlayerController Player;

    private void Start()
    {

    }

    public void JournalBTN()
    {
        InventoryScreen.SetActive(false);
        Journal.SetActive(true);
    }

    public void PotionsBTN()
    {
        //InventoryScreen.SetActive(false);
        //PotionScreen.SetActive(true);
    }

    public void WeaponsBTN()
    {
        //InventoryScreen.SetActive(false);
        //WeaponsMenu.SetActive(true);
    }

    public void DeckBuildingBTN()
    {
        //InventoryScreen.SetActive(false);
        //DeckBuildingScreen.SetActive(true);
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
}
