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
    public class GetAvailableCards : MonoBehaviour
    {
        public GameObject collectionContent, equipContent;
        BuildDeck buildDeck;
        public Object cardPrefab;

        bool setup = false;

        public void LoadCards()
        {
            if (setup) return;

            setup = true;

            DeckManager.instance.LoadDeck();

            buildDeck = GetComponent<BuildDeck>();

            if (collectionContent != null)
            {
                SetupDeck(collectionContent, DeckManager.instance.collection, buildDeck.collectedSpells);
            }

            if (equipContent != null)
            {
                SetupDeck(equipContent, DeckManager.instance.majorArcana, buildDeck.equippedSpells);
            }
        }

        void SetupDeck(GameObject content, List<Spell> collection, List<Spell> buildDeckSpells)
        {
            if (content == null) return;
            Deck2D collectionDeck = content.GetComponentInParent<Deck2D>();

            foreach (Spell spell in collection)
            {
                Debug.Log(spell.spellName + " should be in collection");
                GameObject card = Instantiate(cardPrefab, content.transform) as GameObject;
                CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
                DrawCard drawCard = card.GetComponent<DrawCard>();
                //Add the card to the array
                collectionDeck.AddCard(cardDrag);

                //Reset card scales
                cardDrag.ScaleCard(1, false);

                drawCard.draw = false;
                drawCard.Setup(spell);

                buildDeckSpells.Add(spell);
            }
        }
    }
}
