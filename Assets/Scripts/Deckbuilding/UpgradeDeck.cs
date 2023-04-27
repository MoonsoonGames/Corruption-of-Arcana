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
    public class UpgradeDeck : BuildDeck
    {
        protected override void Start()
        {
            if (collectedDeck.CurrentCardsLength() > 0)
            {
                collectedSpells = collectedDeck.GetSpells();
            }
            else
            {
                collectedSpells = new List<Spell>();
            }
        }

        public override void SaveCards()
        {
            if (true /*Check that player has not left an upgraded card in the output*/)
            {
                DeckManager.instance.collection = collectedSpells;

                DeckManager.instance.SaveDeck();
            }
        }
    }
}
