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
        public GameObject deckbuildingMenu;
        GetAvailableCards getAvailableCards;
        BuildDeck buildDeck;

        // Start is called before the first frame update
        void Start()
        {
            if (deckbuildingMenu == null) return;
            getAvailableCards = deckbuildingMenu.GetComponent<GetAvailableCards>();
            buildDeck = deckbuildingMenu.GetComponent<BuildDeck>();
            deckbuildingMenu.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (deckbuildingMenu == null) return;

            if (Input.GetKeyDown(KeyCode.K))
            {
                OpenCloseMenu(!deckbuildingMenu.activeSelf);
            }
        }

        public void OpenCloseMenu(bool open)
        {
            if (open)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                deckbuildingMenu.SetActive(true);
                getAvailableCards.LoadCards();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                buildDeck.SaveCards();
                deckbuildingMenu.SetActive(false);
            }
        }
    }
}
