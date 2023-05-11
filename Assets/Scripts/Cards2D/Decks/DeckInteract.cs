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
    public class DeckInteract : Deck2D
    {
        public GetInteractCards getInteractCards;

        public override void AddCard(CardDrag2D card)
        {
            base.AddCard(card);
            CheckInteract();
        }

        public override void RemoveCard(CardDrag2D card)
        {
            base.RemoveCard(card);
            CheckInteract();
        }

        public override void RemoveAllCards(bool discard)
        {
            base.RemoveAllCards(discard);
            CheckInteract();
        }

        private void CheckInteract()
        {
            //Check combinations
            bool or = false;
            bool and = true;
            
            List<Spell> deckSpells = GetSpells();
            foreach (var item in getInteractCards.requireSpells)
            {
                if (deckSpells.Contains(item))
                    or = true;
                else
                    and = false;
            }

            bool success = false;

            switch (getInteractCards.operation)
            {
                case (E_Operations.AND):
                    success = and;
                    break;
                case (E_Operations.OR):
                    success = or;
                    break;
            }

            //Enable/disable button
            getInteractCards.SuccessfulInteract();
        }
    }
}
