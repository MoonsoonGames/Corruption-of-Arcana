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
        public Deck2D playerHandDeck; //Hand that player cards are drawn into
        Deck2D[] decks;
        public GameObject cardPrefab; //Prefab of the parent card type
        Timeline timeline;
        TeamManager[] teamManagers;

        Button endTurnButton;
        public Color buttonAvailable = new Color(0, 0, 0, 255);
        public Color buttonUnavailable = new Color(0, 0, 0, 255);

        bool waitingForStartTurn = false;
        bool waitingForSound = false;

        public float endTurndelay = 2f;

        private void Start()
        {
            timeline = GameObject.FindObjectOfType<Timeline>();
            teamManagers = GameObject.FindObjectsOfType<TeamManager>();
            endTurnButton = GetComponent<Button>();
            endTurnButton.image.color = buttonAvailable;

            Invoke("EndTurnButton", 0.1f);
        }

        public void EndTurnButton()
        {
            DisableButton();
            DragManager.instance.canDrag = false;
            PressSound();

            decks = GameObject.FindObjectsOfType<Deck2D>();
            foreach (Deck2D deck in decks)
            {
                if (deck != playerHandDeck)
                {
                    deck.RemoveAllCards(false);
                }
            }

            float delay = timeline.PlayTimeline() + endTurndelay;

            SetUIEnabled(false);
            Invoke("StartNextTurn", delay);
        }

        public void StartNextTurn()
        {
            SetUIEnabled(true);
            DragManager.instance.canDrag = true;
            //Debug.Log("New Turn");

            //Only give cards if player's hand isn't full
            if (playerHandDeck.CurrentCardsLength() < playerHandDeck.maxCards)
            {
                //Get the difference between the current and max cards to determine how many need to be drawn in
                float difference = playerHandDeck.maxCards - playerHandDeck.CurrentCardsLength();

                for (int i = 0; i < difference; i++)
                {
                    GameObject card = Instantiate(cardPrefab, playerHandDeck.transform) as GameObject;
                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    //Add the card to the array
                    playerHandDeck.AddCard(cardDrag);

                    //Reset card scales
                    cardDrag.ScaleCard(1, false);
                }
            }

            timeline.ActivateTurnModifiers();

            foreach (TeamManager manager in teamManagers)
            {
                manager.StartTurn();
            }
            DisableButton();
            waitingForStartTurn = true;
            Invoke("EnableButton", 2f);
        }

        void DisableButton()
        {
            waitingForStartTurn = false;
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
        public StudioParameterTrigger pressTrigger;

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
            pressTrigger.TriggerParameters();
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

        void SetUIEnabled(bool enable)
        {
            foreach (var item in disableUIElements)
            {
                item.SetActive(enable);
            }
        }

        #endregion
    }
}