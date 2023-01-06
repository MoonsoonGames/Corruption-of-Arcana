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
    public class BuildDeck : MonoBehaviour
    {
        //[HideInInspector]
        public List<Spell> collectedSpells, equippedSpells;
        public Deck2D collectedDeck, equippedDeck;

        // Start is called before the first frame update
        void Start()
        {
            if (collectedDeck.CurrentCardsLength() > 0)
            {
                collectedSpells = collectedDeck.GetSpells();
            }
            else
            {
                collectedSpells = new List<Spell>();
            }

            if (equippedDeck.CurrentCardsLength() > 0)
            {
                equippedSpells = equippedDeck.GetSpells();
            }
            else
            {
                equippedSpells = new List<Spell>();
            }
        }

        public void SaveCards()
        {
            DeckManager.instance.collection = collectedSpells;
            DeckManager.instance.majorArcana = equippedSpells;
        }
    }
}
