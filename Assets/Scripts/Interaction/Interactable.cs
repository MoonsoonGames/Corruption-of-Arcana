using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Interactable
{
    public class Interactable : MonoBehaviour
    {
        public bool forceInteract;
        GameObject interactingCharacter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log(other.name + " has entered collision");

                if (forceInteract)
                {
                    Interact(other.gameObject);
                }
                else
                {
                    //Open interact message
                    interactingCharacter = other.gameObject;
                }
            }
        }

        private void Update()
        {
            if (interactingCharacter != null && forceInteract == false)
            {
                //Debug.Log("Can interact");
                if (Input.GetButtonDown("Interact"))
                {
                    //Debug.Log("Button pressed");
                    Interact(interactingCharacter);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log(other.name + " has left collision");

                if (forceInteract)
                {
                    CancelInteract(other.gameObject);
                }
                else
                {
                    //Close interact message
                    interactingCharacter = null;
                }
            }
        }

        void Interact(GameObject playerRef)
        {
            //Call interface function
            Debug.Log("Interact");
            GetComponent<IInteractable>().Interacted(playerRef);
        }

        void CancelInteract(GameObject playerRef)
        {
            //Call interface function
            Debug.Log("Interact");
            GetComponent<ICancelInteractable>().CancelInteraction(playerRef);
        }
    }
}
