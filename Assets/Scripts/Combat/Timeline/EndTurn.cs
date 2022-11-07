using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EndTurn : MonoBehaviour
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

            decks = GameObject.FindObjectsOfType<Deck2D>();
            foreach (Deck2D deck in decks)
            {
                if (deck != playerHandDeck)
                {
                    deck.RemoveAllCards(false);
                }
            }

            float delay = timeline.PlayTimeline() + endTurndelay;

            Invoke("StartNextTurn", delay);
        }

        public void StartNextTurn()
        {
            DragManager.instance.canDrag = true;
            //Debug.Log("New Turn");
            foreach (Deck2D deck in decks)
            {
                if (deck == playerHandDeck)
                {
                    //Only give cards if player's hand isn't full
                    if (playerHandDeck.CurrentCardsLength() < deck.maxCards)
                    {
                        //Get the difference between the current and max cards to determine how many need to be drawn in
                        float difference = deck.maxCards - deck.CurrentCardsLength();

                        for (int i = 0; i < difference; i++)
                        {
                            GameObject card = Instantiate(cardPrefab, deck.transform) as GameObject;
                            CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                            //Add the card to the array
                            deck.AddCard(cardDrag);

                            //Reset card scales
                            cardDrag.ScaleCard(1, false);
                        }
                    }
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
            }
        }

        #region Sound Effects

        public EventReference click;
        public EventReference hover;
        //FMOD.Studio.EventInstance fmodInstance;

        public void PlayClickSound()
        {
            RuntimeManager.PlayOneShot(click);
        }

        public void PlayHoverSound()
        {
            RuntimeManager.PlayOneShot(hover);
        }

        #endregion
    }
}