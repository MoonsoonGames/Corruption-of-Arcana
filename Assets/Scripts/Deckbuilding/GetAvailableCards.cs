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
        public GameObject content;
        Deck2D availableCardsDeck;
        public List<Spell> playerDeck;
        public Object cardPrefab;

        // Start is called before the first frame update
        void Start()
        {
            if (content == null) return;
            availableCardsDeck = GetComponent<Deck2D>();

            foreach(Spell spell in playerDeck)
            {
                GameObject card = Instantiate(cardPrefab, content.transform) as GameObject;
                CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
                DrawCard drawCard = card.GetComponent<DrawCard>();
                //Add the card to the array
                availableCardsDeck.AddCard(cardDrag);

                //Reset card scales
                cardDrag.ScaleCard(1, false);

                drawCard.draw = false;
                drawCard.Setup(spell);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
