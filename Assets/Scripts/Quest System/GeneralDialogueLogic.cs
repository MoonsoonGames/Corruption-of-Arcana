using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Player;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class GeneralDialogueLogic : MonoBehaviour
    {
        public static GeneralDialogueLogic instance { get; private set; }

        private void Start()
        {
            instance = this;
        }

        public void OpenUpgradingMenu()
        {
            TEMP_OpenDeckbuilding.instance.OpenCloseMenu(true, TEMP_OpenDeckbuilding.instance.upgradeDeckMenu);
        }

        public void EndDialogue()
        {
            Debug.Log("EndDialogue");
            PlayerController controller = GameObject.FindObjectOfType<PlayerController>();

            controller.canMove = true;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            UpdateQuestMarkers();
        }

        void UpdateQuestMarkers()
        {
            if (Compass.instance != null)
                Compass.instance.CheckQuestMarkers();
        }
    }
}