using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class DrawCard : MonoBehaviour
    {
        public bool draw = true;
        DeckManager deckManager;
        Card card;

        // Start is called before the first frame update
        void Start()
        {
            deckManager = GameObject.FindObjectOfType<DeckManager>();

            if (deckManager != null && draw)
            {
                Setup(deckManager.GetSpell());
            }
        }

        public void Setup(Spell spell)
        {
            card = GetComponent<Card>();
            card.Setup(spell);
        }

        public void ReturnToDeck()
        {
            if (card.spell.potionCost > 0) return;

            if (card.spell.discardAfterCasting)
            {
                deckManager.DiscardCard(card.spell);
            }
            else
            {
                deckManager.ReturnCard(card.spell);
            }
        }

        public void DiscardCard()
        {
            deckManager.DiscardCard(card.spell);
        }
    }
}