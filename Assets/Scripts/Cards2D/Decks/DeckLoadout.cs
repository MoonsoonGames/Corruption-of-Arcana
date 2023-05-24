using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class DeckLoadout : Deck2D
    {
        BuildDeck buildDeck;

        protected override void Start()
        {
            base.Start();

            buildDeck = GetComponentInParent<BuildDeck>();

            ResetArrays();
        }

        public override void RemoveCard(CardDrag2D card)
        {
            base.RemoveCard(card);

            buildDeck.equippedSpells.Remove(card.GetComponent<Card>().spell);
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
                    buildDeck.equippedSpells.Add(spell);
                }
            }
            else
            {
                Debug.LogError("No build deck");
            }
        }

        protected override void ResetArrays()
        {
            base.ResetArrays();
            
            UpdateText();
        }

        public int maxLoadoutCost = 7;
        int currentLoadout = 0;

        public TextMeshProUGUI costText;

        public void UpdateText()
        {
            int cost = 0;

            foreach (CardDrag2D card in cards)
            {
                Debug.Log("Updating text to " + cost + " with card " + card.name);
                Spell spell = card.GetComponent<Card>().spell;

                if (spell != null)
                    cost += spell.loadoutCost;
            }

            currentLoadout = cost;

            Debug.Log("Updating text to " + currentLoadout);

            costText.text = "Available Space: " + currentLoadout + " / " + maxLoadoutCost;
        }

        public bool AvailableSpaces()
        {
            return currentLoadout <= maxLoadoutCost;
        }
    }
}
