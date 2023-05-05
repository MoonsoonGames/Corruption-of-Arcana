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
    public class DeckWeaponCards : Deck2D
    {
        public override void RemoveAllCards(bool discard)
        {
            ResetArrays();
            foreach (CardDrag2D card in cards)
            {
                Destroy(card.gameObject);
            }

            cards = new CardDrag2D[0];
            if (timeline != null)
                timeline.SimulateSpellEffects();
        }
    }
}
