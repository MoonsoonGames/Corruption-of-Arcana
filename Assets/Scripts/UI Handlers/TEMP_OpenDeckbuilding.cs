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

        bool cooldown = true;
        float cooldownTime = 0.2f;

        // Update is called once per frame
        void Update()
        {
            if (deckbuildingMenu == null) return;

            if (Input.GetKeyDown(KeyCode.K) && cooldown)
            {
                if (!weaponsMenu.activeSelf && !upgradeDeckMenu.activeSelf)
                    OpenCloseMenu(!deckbuildingMenu.activeSelf, deckbuildingMenu);
            }

            if (Input.GetKeyDown(KeyCode.E) && cooldown)
            {
                if (!upgradeDeckMenu.activeSelf && !deckbuildingMenu.activeSelf)
                    OpenCloseMenu(!weaponsMenu.activeSelf, weaponsMenu);
            }
        }

        public void OpenCloseMenu(bool open, GameObject menu)
        {
            if (open)
            {
                Time.timeScale = 0;
                cooldown = false;
                StartCoroutine(Cooldown(cooldownTime));
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                menu.SetActive(true);

                if (menu == deckbuildingMenu)
                {
                    getAvailableCards.LoadCards();
                    StartCoroutine(buildDeck.OpenMenu(0.25f));
                }
                else if (menu == upgradeDeckMenu)
                {
                    upgradeAvailableCards.LoadCards();
                    StartCoroutine(upgradeBuildDeck.OpenMenu(0.25f));
                }
                else if (menu == weaponsMenu)
                {
                    getWeapons.OpenEquipment();
                }

            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                if (menu == deckbuildingMenu)
                    buildDeck.SaveCards();
                if (menu == upgradeDeckMenu)
                    upgradeBuildDeck.SaveCards();
                menu.SetActive(false);
            }
        }

        IEnumerator Cooldown(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            cooldown = true;
        }
    }
}
