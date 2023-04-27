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
    public class TEMP_OpenDeckbuilding : MonoBehaviour
    {
        public static TEMP_OpenDeckbuilding instance;

        public GameObject deckbuildingMenu;
        public GameObject upgradeDeckMenu;
        public GameObject weaponsMenu;
        public GetWeapons getWeapons;
        GetAvailableCards getAvailableCards, upgradeAvailableCards;
        BuildDeck buildDeck, upgradeBuildDeck;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;

            if (deckbuildingMenu != null)
            {
                getAvailableCards = deckbuildingMenu.GetComponent<GetAvailableCards>();
                buildDeck = deckbuildingMenu.GetComponent<BuildDeck>();
                deckbuildingMenu.SetActive(false);
            }

            if (upgradeDeckMenu != null)
            {
                upgradeAvailableCards = upgradeDeckMenu.GetComponent<GetAvailableCards>();
                upgradeBuildDeck = upgradeDeckMenu.GetComponent<UpgradeDeck>();
                upgradeDeckMenu.SetActive(false);
            }

            if (weaponsMenu != null)
            {
                weaponsMenu.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (deckbuildingMenu == null) return;

            if (Input.GetKeyDown(KeyCode.K))
            {
                OpenCloseMenu(!deckbuildingMenu.activeSelf, deckbuildingMenu);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                OpenCloseMenu(!upgradeDeckMenu.activeSelf, upgradeDeckMenu);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                OpenCloseMenu(!weaponsMenu.activeSelf, weaponsMenu);
            }
        }

        public void OpenCloseMenu(bool open, GameObject menu)
        {
            if (open)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                menu.SetActive(true);
                if (menu == deckbuildingMenu)
                {
                    buildDeck.OpenMenu();
                    getAvailableCards.LoadCards();
                }
                else if (menu == upgradeDeckMenu)
                {
                    upgradeBuildDeck.OpenMenu();
                    upgradeAvailableCards.LoadCards();
                }
                else if (menu == weaponsMenu)
                    getWeapons.OpenEquipment();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                if (menu == deckbuildingMenu)
                    buildDeck.SaveCards();
                if (menu == upgradeDeckMenu)
                    upgradeBuildDeck.SaveCards();
                menu.SetActive(false);
            }
        }
    }
}
