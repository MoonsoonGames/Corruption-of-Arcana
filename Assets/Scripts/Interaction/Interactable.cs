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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + " has entered collision");

                if (forceInteract)
                {
                    Interact(other.gameObject);
                }
                else
                {
                    //Open interact message
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!forceInteract)
                {
                    Debug.Log("Can interact");
                    if (Input.GetButtonDown("Interact"))
                    {
                        Debug.Log("Button pressed");
                        Interact(other.gameObject);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + " has left collision");

                //Close interact message
            }
        }

        void Interact(GameObject playerRef)
        {
            //Call interface function
            Debug.Log("Interact");
            GetComponent<IInteractable>().Interacted(playerRef);
        }
    }
}
