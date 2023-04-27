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
    public class DeckUpgradeInput : Deck2D
    {
        BuildDeck buildDeck;
        public DeckUpgradeOutput outputDeck;

        protected override void Start()
        {
            base.Start();

            buildDeck = GetComponentInParent<BuildDeck>();
        }

        public override void AddCard(CardDrag2D card)
        {
            base.AddCard(card);
            CheckUpgrade();
        }

        public override void RemoveCard(CardDrag2D card)
        {
            base.RemoveCard(card);
            CheckUpgrade();
        }

        public override void RemoveAllCards(bool discard)
        {
            base.RemoveAllCards(discard);
            CheckUpgrade();
        }

        private void CheckUpgrade()
        {
            if (cards.Length != 3 || outputDeck == null)
                return;

            Spell currentSpell = null;

            foreach (var item in cards)
            {
                Spell spell = item.GetComponent<Spell>();
                if (currentSpell == null)
                {
                    currentSpell = spell;
                }
                else
                {
                    if (currentSpell != spell)
                        return;
                }
            }

            if (currentSpell != null)
            {
                //Can upgrade, spawn highter tier in output deck
                outputDeck.SpawnUpgradeCard(currentSpell.nextTier);
            }
        }
    }
}
