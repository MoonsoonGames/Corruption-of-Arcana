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
    public class DeckManager : MonoBehaviour
    {
        #region Singleton
        //Code from last year

        public static DeckManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                //DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        public List<Spell> playerDeck;
        public List<Spell> playedCards;
        public List<Spell> discardPile;

        /// <summary>
        /// Draws a card from the player deck and returns it, if the player deck is empty, shuffle cards from the discard pile into the player deck and then draw a card
        /// </summary>
        /// <returns>The spell drawn from the player deck</returns>
        public Spell GetSpell()
        {
            if (playerDeck.Count == 0)
            {
                DrawFromPlayed();
            }

            if (playerDeck.Count != 0)
            {
                Spell spell = playerDeck[0];

                playerDeck.Remove(spell);

                return spell;
            }

            return null;
        }

        private void Start()
        {
            Singleton();
            playerDeck.Sort(HelperFunctions.RandomSort);
        }

        public void AddToStart(Spell spell)
        {
            playerDeck.Insert(0, spell);
        }

        /// <summary>
        /// Adds card to the discard pile
        /// </summary>
        /// <param name="spell"></param>
        public void ReturnCard(Spell spell)
        {
            playedCards.Add(spell);
        }

        public void DiscardCard(Spell spell)
        {
            discardPile.Add(spell);
        }

        public void DiscardPileToDeck(bool start)
        {
            foreach(Spell spell in discardPile)
            {
                if (start)
                {
                    playerDeck.Insert(0, spell);
                }
                else
                {
                    playerDeck.Add(spell);
                }
            }

            discardPile.Clear();
        }

        /// <summary>
        /// Shuffled cards in the discard pile and then adds them to the player deck
        /// </summary>
        void DrawFromPlayed()
        {
            playedCards.Sort(HelperFunctions.RandomSort);

            foreach (Spell spell in playedCards)
            {
                playerDeck.Add(spell);
            }

            playedCards.Clear();
        }
    }
}