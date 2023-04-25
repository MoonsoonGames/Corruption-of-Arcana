using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EndTurn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        Button endTurnButton;
        public Color buttonAvailable = new Color(0, 0, 0, 255);
        public Color buttonUnavailable = new Color(0, 0, 0, 255);

        bool waitingForStartTurn = false;
        bool waitingForSound = false;

        public float endTurnDelay = 1f;

        ArcanaManager arcanaManager;
        DeckTab deckTab;

        private void Start()
        {
            endTurnButton = GetComponent<Button>();
            endTurnButton.image.color = buttonAvailable;

            arcanaManager = Timeline.instance.GetArcanaManager();
            deckTab = GetComponentInParent<DeckTab>();

            Invoke("EndTurnButton", 0.1f);
        }

        bool input = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && input == false)
            {
                input = true;

                if (waitingForStartTurn == false)
                {
                    Debug.Log("End turn success");
                    EndTurnButton();
                }
            }
            else
            {
                input = false;
            }
        }

        public void EndTurnButton()
        {
            if (enabled == false || CombatManager.instance.CanEndTurn() == false)
                return;

            DisableButton();
            DragManager.instance.canDrag = false;
            PressSound();

            deckTab.SelectHand();

            float delay = CombatManager.instance.EndTurn(endTurnDelay);

            SetUIEnabled(false);
            Invoke("StartNextTurn", delay);
        }

        public void StartNextTurn()
        {
            SetUIEnabled(true);
            DragManager.instance.canDrag = true;
            //Debug.Log("New Turn");

            DisableButton();
            waitingForStartTurn = true;
            Invoke("EnableButton", 2f);

            arcanaManager.CheckArcana(0);
        }

        void DisableButton()
        {
            waitingForStartTurn = true;
            endTurnButton.image.color = buttonUnavailable;
            endTurnButton.interactable = false;
        }

        void EnableButton()
        {
            if (waitingForStartTurn)
            {
                endTurnButton.interactable = true;
                endTurnButton.image.color = buttonAvailable;
                waitingForStartTurn = false;

                if (waitingForSound)
                {
                    PlayHoverSound();
                }
            }
        }

        #region Sound Effects

        public StudioEventEmitter hoverEmitter;
        public StudioEventEmitter pressEmitter;

        void PlayHoverSound()
        {
            hoverEmitter.Play();
        }

        void StopHoverSound()
        {
            hoverEmitter.Stop();
        }

        void PressSound()
        {
            //hoverEmitter.Stop();
            pressEmitter.Play();
        }

        #endregion

        #region Pointer Events

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (endTurnButton.interactable)
            {
                PlayHoverSound();
            }
            else
            {
                waitingForSound = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHoverSound();
            waitingForSound = false;
        }

        #endregion

        #region UI

        public List<GameObject> disableUIElements;
        Dictionary<GameObject, Vector3> disableUIElementsDictionary = new Dictionary<GameObject, Vector3>();

        void SetupDictionary()
        {
            Deck2D[] decks = GameObject.FindObjectsOfType<Deck2D>();

            ClearMissing();

            foreach (var deck in decks)
            {
                bool duplicate = false;
                foreach (var item in disableUIElements)
                {
                    if (item != null)
                    {
                        if (item.GetComponentInChildren<Deck2D>() == deck)
                        {
                            Debug.Log("Found copy");
                            duplicate = true;
                        }
                    }
                }

                if (duplicate == false)
                    disableUIElements.Add(deck.gameObject);
            }

            foreach(var item in disableUIElements)
            {
                if (!disableUIElementsDictionary.ContainsKey(item))
                    disableUIElementsDictionary.Add(item, item.transform.position);
            }
        }

        void ClearMissing()
        {
            List<GameObject> newDisableUIElements = new List<GameObject>();

            foreach (var item in disableUIElements)
            {
                if (item != null)
                {
                    newDisableUIElements.Add(item);
                }
            }

            disableUIElements = newDisableUIElements;
        }

        void SetUIEnabled(bool enable)
        {
            SetupDictionary();
            foreach (var item in disableUIElementsDictionary)
            {
                if (item.Key != null)
                    item.Key.transform.position = enable ? item.Value : new Vector3(0, -500000000, 0);
            }
        }

        #endregion
    }
}