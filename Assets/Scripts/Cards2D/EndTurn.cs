using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public Deck2D playerHandDeck; //Hand that player cards are drawn into
    public GameObject cardPrefab; //Prefab of the parent card type

    public void EndTurnButton()
    {
        if (playerHandDeck != null)
        {
            //Only give cards if player's hand isn't full
            if (playerHandDeck.CurrentCardsLength() < playerHandDeck.maxCards)
            {
                //Get the difference between the current and max cards to determine how many need to be drawn in
                float difference = playerHandDeck.maxCards - playerHandDeck.CurrentCardsLength();

                for(int i = 0; i < difference; i++)
                {
                    GameObject card = Instantiate(cardPrefab, playerHandDeck.transform) as GameObject;

                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    //Add the card to the array
                    playerHandDeck.AddCard(cardDrag);
                }
            }
        }
    }
}
