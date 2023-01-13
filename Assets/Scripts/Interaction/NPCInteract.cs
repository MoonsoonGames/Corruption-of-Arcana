using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class NPCInteract : MonoBehaviour, IInteractable
    {
        public string SpeakToMessage = "SpeakingTo";
        public string ID;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
            {
                Flowchart.BroadcastFungusMessage(SpeakToMessage);
                Debug.Log("Spoken to NPC");
            }

    }
}
