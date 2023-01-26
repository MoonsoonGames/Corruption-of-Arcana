using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Interactable
{
    public class Interactable : MonoBehaviour
    {
        public string interactID;
        public bool multipleInteractions = true;

        public bool lockInteractions = false;
        Collider player;

        public bool forceInteract;
        GameObject interactingCharacter;

        static TooltipBox interactionPopup;
        public string interactionPopupTitle;
        public string interactionPopupMessage;

        private void Start()
        {
            Interactable[] allInteractables = FindObjectsOfType<Interactable>();

            for (int i = 0; i < allInteractables.Length; i++)
            {
                if (allInteractables[i] == this)
                {
                    //interactID = allInteractables[i].name + "-" + i + "-" + SceneManager.GetActiveScene().name;
                }
            }
            foreach (var item in allInteractables)
            {
                if (item == this)
                {
                    interactID = item.name + "-" + item.transform.position + "-" + SceneManager.GetActiveScene().name;
                }
            }

            if (LoadCombatManager.instance.interacted.Contains(interactID))
            {
                Debug.Log("Contains, destroy");
                Destroy(gameObject);
            }

            SetInteractMessage(null, null);
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


        public void UnlockInteraction()
        {
            lockInteractions = false;
            if (player != null)
            {
                CheckInteraction(player.gameObject);
            }
        }

        void CheckInteraction(GameObject playerObj)
        {
            if (forceInteract)
            {
                Interact(playerObj);
            }
            else
            {
                //Open interact message
                interactingCharacter = playerObj;
                SetInteractMessage(interactionPopupTitle, interactionPopupMessage);
            }
        }

        void CheckCancelInteraction(GameObject playerObj)
        {
            if (forceInteract)
            {
                CancelInteract(playerObj);
            }
            else
            {
                //Close interact message
                interactingCharacter = null;
                SetInteractMessage(null, null);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log(other.name + " has entered collision");
                player = other;
                if (lockInteractions) return;

                CheckInteraction(other.gameObject);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log(other.name + " has left collision");
                player = null;
                if (lockInteractions) return;

                CheckCancelInteraction(other.gameObject);
            }
        }

        void Interact(GameObject playerRef)
        {
            SetInteractMessage(null, null);

            //Call interface function
            //Debug.Log("Interact");
            IInteractable[] interacts = GetComponents<IInteractable>();
            foreach (var item in interacts)
            {
                item.Interacted(playerRef);
            }

            if (multipleInteractions == false)
            {
                LoadCombatManager.instance.interacted.Add(interactID);
            }
        }

        void CancelInteract(GameObject playerRef)
        {
            SetInteractMessage(null, null);
            //Call interface function
            //Debug.Log("Interact");
            ICancelInteractable[] interacts = GetComponents<ICancelInteractable>();
            foreach (var item in interacts)
            {
                item.CancelInteraction(playerRef);
            }
        }

        void SetInteractMessage(string title, string message)
        {
            if (interactionPopup == null)
            {
                interactionPopup = GameObject.FindGameObjectWithTag("InteractPopup").GetComponent<TooltipBox>();
            }

            if (interactionPopup == null) return;

            if (title != null && message != null)
            {
                interactionPopup.gameObject.SetActive(true);
                interactionPopup.SetText(interactionPopupTitle, interactionPopupMessage);
            }
            else
            {
                interactionPopup.gameObject.SetActive(false);
            }
        }
    }
}
