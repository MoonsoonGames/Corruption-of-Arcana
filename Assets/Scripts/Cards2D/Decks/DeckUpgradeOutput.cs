using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class DeckUpgradeOutput : Deck2D, IPointerEnterHandler
    {
        BuildDeck buildDeck;
        public DeckUpgradeInput inputDeck
            ;
        protected override void Start()
        {
            base.Start();

            buildDeck = GetComponentInParent<BuildDeck>();
        }

        public Object cardPrefab;

        public void SpawnUpgradeCard(Spell spell)
        {
            if (spell == null)
                return;

            GameObject card = Instantiate(cardPrefab, transform) as GameObject;
            CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
            DrawCard drawCard = card.GetComponent<DrawCard>();

            //Reset card scales
            cardDrag.ScaleCard(1, false);

            drawCard.draw = false;
            drawCard.Setup(spell);
        }

        /// <summary>
        /// Called when mouse hovers over deck
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            //Override function so player cannot drag a card into the deck
        }

        public override void RemoveCard(CardDrag2D card)
        {
            base.RemoveCard(card);
            inputDeck.RemoveAllCards(true);
        }
    }
}
