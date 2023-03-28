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
    public class DeckCollection : Deck2D
    {
        BuildDeck buildDeck;

        protected override void Start()
        {
            base.Start();

            buildDeck = GetComponentInParent<BuildDeck>();
        }

        public override void RemoveCard(CardDrag2D card)
        {
            base.RemoveCard(card);

            buildDeck.collectedSpells.Remove(card.GetComponent<Card>().spell);
        }

        public override void RemoveAllCards(bool discard)
        {
            ResetArrays();
            foreach (CardDrag2D card in cards)
            {
                Spell spell = card.GetComponent<Card>().spell;
                buildDeck.collectedSpells.Remove(spell);

                Destroy(card.gameObject);
            }

            cards = new CardDrag2D[0];
            if (timeline != null)
                timeline.SimulateSpellEffects();
        }

        public override void AddCard(CardDrag2D card)
        {
            base.AddCard(card);

            if (buildDeck != null)
            {
                Spell spell = card.GetComponent<Card>().spell;

                if (spell != null)
                {
                    buildDeck.collectedSpells.Add(spell);
                }
            }
            else
            {
                Debug.LogError("No build deck");
            }
        }
    }
}
