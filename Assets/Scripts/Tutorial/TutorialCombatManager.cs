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

        bool setup = false;

        public override bool CanEndTurn()
        {
            if (!setup)
            {
                setup = true;
                return true;
            }

            bool success = true;

            if (currentTurn < tutorialCards.Length)
            {
                foreach (var spellTarget in tutorialCards[currentTurn].spellTargets)
                {
                    Character target = spellTarget.target < 0 ? player : enemyTeamManager.team[spellTarget.target];

                    if (Timeline.instance.CheckTarget(spellTarget.spell, target) == false)
                        success = false;
                }
            }

            return success;
        }

        public override float EndTurn(float endTurnDelay)
        {
            float delay = base.EndTurn(endTurnDelay);

            currentTurn++;

            return delay;
        }

        public override void StartNextTurn()
        {
            if (currentTurn < tutorialCards.Length)
            {
                foreach(var spellTarget in tutorialCards[currentTurn].spellTargets)
                {
                    DeckManager.instance.AddToStart(spellTarget.spell);
                }

                for (int i = 0; i < tutorialCards[currentTurn].spellTargets.Count; i++)
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
        }
    }

    [System.Serializable]
    public struct TutorialCards
    {
        public List<SpellTarget> spellTargets;
    }

    [System.Serializable]
    public struct SpellTarget
    {
        public Spell spell;
        public int target;
    }
}
