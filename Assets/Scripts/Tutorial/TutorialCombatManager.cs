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
    public class TutorialCombatManager : CombatManager
    {
        int currentTurn = 0;

        public TutorialCards[] tutorialCards;

        public override void StartNextTurn()
        {
            if (currentTurn < tutorialCards.Length)
            {
                foreach(var card in tutorialCards[currentTurn].spells)
                {
                    DeckManager.instance.AddToStart(card);
                }

                for (int i = 0; i < tutorialCards[currentTurn].spells.Count; i++)
                {
                    GameObject card = Instantiate(cardPrefab, playerHandDeck.transform) as GameObject;
                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    //Add the card to the array
                    playerHandDeck.AddCard(cardDrag);

                    //Reset card scales
                    cardDrag.ScaleCard(1, false);
                }
            }
            else
            {
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

                    //Spawn sound effect for cards
                    PlayShuffleSound();
                }
            }

            RedrawPotions();

            Timeline.instance.ActivateTurnModifiers();

            playerTeamManager.StartTurn(); enemyTeamManager.StartTurn();

            currentTurn++;
        }
    }

    [System.Serializable]
    public struct TutorialCards
    {
        public List<Spell> spells;
    }
}
