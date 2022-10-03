using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public Deck2D playerHandDeck;
    public GameObject cardPrefab;

    public void EndTurnButton()
    {
        Debug.Log("End Turn");

        if (playerHandDeck != null)
        {
            Debug.Log(playerHandDeck.CurrentCardsLength() + " / " + playerHandDeck.maxCards);
            if (playerHandDeck.CurrentCardsLength() < playerHandDeck.maxCards)
            {
                float difference = playerHandDeck.maxCards - playerHandDeck.CurrentCardsLength();

                for(int i = 0; i < difference; i++)
                {
                    GameObject card = Instantiate(cardPrefab, playerHandDeck.transform) as GameObject;

                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    playerHandDeck.AddCard(cardDrag);
                }
            }
        }
    }
}
