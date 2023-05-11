using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Necropanda.Player;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class NPCInteract : MonoBehaviour, IInteractable
    {
        public GameObject flowChart;
        public string ID;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
        {
            if (flowChart != null)
            {
                Debug.Log("EndDialogue");
                PlayerController controller = player.GetComponent<PlayerController>();

                flowChart.SetActive(true);

                controller.canMove = false;

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
}
