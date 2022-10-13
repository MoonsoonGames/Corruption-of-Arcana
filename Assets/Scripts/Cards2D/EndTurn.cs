using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        EnemyManager enemyManager;

        private void Start()
        {
            decks = GameObject.FindObjectsOfType<Deck2D>();
            timeline = GameObject.FindObjectOfType<Timeline>();
            enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        }

        public void EndTurnButton()
        {
            foreach (Deck2D deck in decks)
            {
                if (deck != playerHandDeck)
                {
                    deck.RemoveAllCards();
                }
            }

            float delay = timeline.CastSpells() + 0.5f;

            Invoke("StartNextTurn", delay);
        }

        public void StartNextTurn()
        {
            Debug.Log("New Turn");
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

            enemyManager.StartTurn();
        }
    }
}