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
    public class DeckCharacter : Deck2D
    {
        protected override void Start()
        {
            base.Start();

            character = GetComponentInParent<Character>();

            timeline = GameObject.FindObjectOfType<Timeline>();
            if (timeline != null)
                player = timeline.player;
        }

        public override void RemoveCard(CardDrag2D card)
        {
            base.RemoveCard(card);

            bool empower = false;
            bool weaken = false;

            if (character != null)
            {
                empower = character.empowerDeck;
                weaken = character.weakenDeck;
            }

            CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
            newSpellInstance.SetSpellInstance(card.GetComponent<Card>().spell, empower, weaken, character, player);

            if (timeline != null)
            {
                timeline.RemoveSpellInstance(newSpellInstance);
                timeline.SimulateSpellEffects();
            }
        }

        public override void RemoveAllCards(bool discard)
        {
            ResetArrays();
            foreach (CardDrag2D card in cards)
            {
                DrawCard drawCard = card.GetComponent<DrawCard>();
                if (drawCard != null)
                {
                    if (discard)
                    {
                        drawCard.DiscardCard();
                    }
                    else
                    {
                        drawCard.ReturnToDeck();
                    }
                }

                Destroy(card.gameObject);
            }

            cards = new CardDrag2D[0];
            if (timeline != null)
                timeline.SimulateSpellEffects();
        }

        public override void AddCard(CardDrag2D card)
        {
            base.AddCard(card);

            if (character != null && card.playerCard)
            {
                CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
                newSpellInstance.SetSpellInstance(card.GetComponent<Card>().spell, character.empowerDeck, character.weakenDeck, character, player);

                if (timeline != null)
                    timeline.AddSpellInstance(newSpellInstance);
            }

            if (timeline != null)
                timeline.SimulateSpellEffects();
        }
    }
}
