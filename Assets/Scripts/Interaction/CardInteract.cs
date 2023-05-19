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
    public class CardInteract : MonoBehaviour, IInteractable
    {
        public string ID;
        public string message;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public List<Spell> spells;
        public E_Operations operation;
        public Interactable.Interactable interactable;

        public void Interacted(GameObject player)
        {
            GetInteractCards interactCards = GameObject.FindObjectOfType<GetInteractCards>(true);
            interactCards.gameObject.SetActive(true);
            interactCards.LoadCards(spells, operation, interactable, message);
        }
    }
}