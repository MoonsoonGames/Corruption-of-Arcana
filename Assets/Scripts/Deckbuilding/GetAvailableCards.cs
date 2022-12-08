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
        Deck2D collectionDeck, equipDeck;
        BuildDeck buildDeck;
        public Object cardPrefab;

        bool setup = false;

        public void LoadCards()
        {
            if (setup) return;

            setup = true;

            if (collectionContent == null) return;
            collectionDeck = collectionContent.GetComponentInParent<Deck2D>();

            if (equipContent == null) return;
            equipDeck = equipContent.GetComponentInParent<Deck2D>();

            buildDeck = GetComponent<BuildDeck>();

            foreach (Spell spell in DeckManager.instance.collection)
            {
                GameObject card = Instantiate(cardPrefab, collectionContent.transform) as GameObject;
                CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
                DrawCard drawCard = card.GetComponent<DrawCard>();
                //Add the card to the array
                collectionDeck.AddCard(cardDrag);

                //Reset card scales
                cardDrag.ScaleCard(1, false);

                drawCard.draw = false;
                drawCard.Setup(spell);

                buildDeck.collectedSpells.Add(spell);
            }

            foreach (Spell spell in DeckManager.instance.majorArcana)
            {
                GameObject card = Instantiate(cardPrefab, equipContent.transform) as GameObject;
                CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
                DrawCard drawCard = card.GetComponent<DrawCard>();
                //Add the card to the array
                equipDeck.AddCard(cardDrag);

                //Reset card scales
                cardDrag.ScaleCard(1, false);

                drawCard.draw = false;
                drawCard.Setup(spell);

                buildDeck.equippedSpells.Add(spell);
            }
        }
    }
}
