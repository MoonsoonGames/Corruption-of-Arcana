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
        public DeckCollection collectedDeck;
        public DeckLoadout equippedDeck;

        // Start is called before the first frame update
        public virtual void OpenMenu()
        {
            if (collectedDeck != null)
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

            if (equippedDeck != null)
            {
                if (equippedDeck.CurrentCardsLength() > 0)
                {
                    equippedSpells = equippedDeck.GetSpells();
                }
                else
                {
                    equippedSpells = new List<Spell>();
                }
            }
        }

        public virtual void SaveCards()
        {
            Debug.Log("Deck menu saves cards");

            if (equippedDeck.AvailableSpaces())
            {
                DeckManager.instance.collection = collectedSpells;
                DeckManager.instance.majorArcana = equippedSpells;

                DeckManager.instance.SaveDeck();
            }
        }
    }
}