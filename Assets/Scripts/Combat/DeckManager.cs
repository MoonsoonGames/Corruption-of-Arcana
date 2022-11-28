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

        #region Decks

        public List<Spell> minorArcana;
        public List<Spell> majorArcana;
        public List<Spell> playerDeck;
        public List<Spell> playedCards;
        public List<Spell> discardPile;

        #endregion

        /// <summary>
        /// Draws a card from the player deck and returns it, if the player deck is empty, shuffle cards from the discard pile into the player deck and then draw a card
        /// </summary>
        /// <returns>The spell drawn from the player deck</returns>
        public Spell GetSpell()
        {
            if (playerDeck.Count == 0)
            {
                //If deck is empty, shuffle played pile into deck
                DrawFromPlayed();
            }

            if (playerDeck.Count > 0)
            {
                //As long as there are cards, remove the spell from the list and return it
                Spell spell = playerDeck[0];

                playerDeck.Remove(spell);

                return spell;
            }

            //Error with getting spell, return null
            return null;
        }

        private void Start()
        {
            Singleton();

            SetupDecks();
        }

        public void SetupDecks()
        {
            playerDeck = HelperFunctions.CombineLists(minorArcana, majorArcana);

            for (int i = 0; i < 5; i++)
                playerDeck.Sort(HelperFunctions.RandomSort);
        }

        /// <summary>
        /// Adds a card to the start of the spell list
        /// </summary>
        /// <param name="spell"></param>
        public void AddToStart(Spell spell)
        {
            playerDeck.Insert(0, spell);
        }

        /// <summary>
        /// Adds card to the played cards pile
        /// </summary>
        /// <param name="spell"></param>
        public void ReturnCard(Spell spell)
        {
            playedCards.Add(spell);
        }

        /// <summary>
        /// Adds card to the discard pile
        /// </summary>
        /// <param name="spell"></param>
        public void DiscardCard(Spell spell)
        {
            discardPile.Add(spell);
        }

        /// <summary>
        /// Returns all discarded cards to the deck
        /// </summary>
        /// <param name="start">Determines whether the cards appear at the start of the deck (meaning they get drawn first) or the end</param>
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

        public void SetDeck(List<Spell> spells)
        {
            majorArcana = new List<Spell>();

            foreach (Spell spell in spells)
            {
                majorArcana.Add(spell);
            }

            SetupDecks();
        }
    }
}