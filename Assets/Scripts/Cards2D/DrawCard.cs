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
        DeckManager deckManager;
        Card card;

        // Start is called before the first frame update
        void Start()
        {
            deckManager = GameObject.FindObjectOfType<DeckManager>();

            if (deckManager != null)
            {
                Spell spell = deckManager.GetSpell();

                card = GetComponent<Card>();
                card.spell = spell;
                card.Setup();
            }
        }

        public void ReturnToDeck()
        {
            deckManager.ReturnCard(card.spell);
        }
    }
}