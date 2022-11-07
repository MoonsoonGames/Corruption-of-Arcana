using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class Deck2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Setup

        #region References

        Character player;
        Character character;
        Timeline timeline;
        DeckManager manager;

        public GameObject group;
        HorizontalLayoutGroup layout;

        GeneralDragArea dragArea;
        DragManager dragManager;

        bool open = true;
        UntargettableOverlay untargettableOverlay;

        #endregion

        #region Cards

        CardDrag2D[] cards;
        public int maxCards = 3;
        public int CurrentCardsLength()
        {
            return cards.Length;
        }

        public float deckScale = 1;

        #endregion

        #region Highlight Values
        Image deckBackground;
        Color baseColor;
        public Color highlightColor;
        public float highlightSpeed = 0.2f;
        Color desiredColor;
        #endregion

        private void Start()
        {
            //Sets up base values
            ResetArrays();

            dragArea = GameObject.FindObjectOfType<GeneralDragArea>();
            dragManager = DragManager.instance;

            deckBackground = group.GetComponent<Image>();
            baseColor = deckBackground.color;
            desiredColor = baseColor;

            layout = GetComponent<HorizontalLayoutGroup>();

            character = GetComponentInParent<Character>();
            timeline = GameObject.FindObjectOfType<Timeline>();
            player = timeline.player;

            untargettableOverlay = GetComponentInChildren<UntargettableOverlay>();
            SetOverlay(false, " ");
        }

        #endregion

        #region Pointer Events

        /// <summary>
        /// Called when mouse hovers over deck
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Only fires logic when player is dragging a card into the deck
            if (eventData.dragging == true && open)
            {
                if (cards.Length < maxCards)
                {
                    //Debug.Log(cards.Length + " / " + maxCards);
                    CardDrag2D currentCard = dragManager.draggedCard;
                    currentCard.newDeck = this;
                    currentCard.ScaleCard(currentCard.hoverScale, true);
                    Highlight(true);
                }
            }
        }

        /// <summary>
        /// Called when mouse stops hovering over deck
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            //Only fires logic when player is dragging a card off the deck
            if (eventData.dragging == true && open)
            {
                CardDrag2D currentCard = dragManager.draggedCard;
                currentCard.newDeck = null;
                currentCard.ScaleCard(currentCard.pickupScale, true);
                Highlight(false);
            }
        }

        #endregion

        #region Adding/Removing Cards

        /// <summary>
        /// Called when player drags a card away from the deck
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(CardDrag2D card)
        {
            bool empower = false;
            bool weaken = false;

            if (character != null)
            {
                empower = character.empowerDeck;
                weaken = character.weakenDeck;
            }

            card.gameObject.transform.SetParent(dragArea.transform);
            card.deck = null;

            ResetArrays();

            CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
            newSpellInstance.SetSpellInstance(card.GetComponent<Card>().spell, empower, weaken, character, player);

            timeline.RemoveSpellInstance(newSpellInstance);
            timeline.SimulateSpellEffects();
        }

        /// <summary>
        /// Removes all cards from the deck without taking them from the timeline
        /// </summary>
        public void RemoveAllCards()
        {
            foreach (CardDrag2D card in cards)
            {
                DrawCard drawCard = card.GetComponent<DrawCard>();
                if (drawCard != null)
                {
                    drawCard.ReturnToDeck();
                }
                Destroy(card.gameObject);
            }

            cards = new CardDrag2D[0];
            timeline.SimulateSpellEffects();
        }

        /// <summary>
        /// Called when player drops a card onto this deck
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(CardDrag2D card)
        {
            card.gameObject.transform.SetParent(group.transform);
            card.deck = this;

            ResetArrays();

            if (character != null && card.playerCard)
            {
                CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
                newSpellInstance.SetSpellInstance(card.GetComponent<Card>().spell, character.empowerDeck, character.weakenDeck, character, player);

                timeline.AddSpellInstance(newSpellInstance);
            }
            timeline.SimulateSpellEffects();
        }

        /// <summary>
        /// Needs to be called whenever a card is removed or added to this deck
        /// Clears the array and resets it to what is currently in the deck
        /// </summary>
        void ResetArrays()
        {
            Highlight(false);

            cards = GetComponentsInChildren<CardDrag2D>();

            foreach (CardDrag2D card in cards)
            {
                card.deck = this;

                card.gameObject.transform.SetParent(group.transform);
            }
        }

        #endregion

        #region Visual Feedback

        /// <summary>
        /// Turns the highlight colour on or off
        /// </summary>
        /// <param name="on">True for highlight color, False for base color</param>
        void Highlight(bool on)
        {
            //Desired color is set so that the color change can be smoothed in update
            if (on)
            {
                desiredColor = highlightColor;
            }
            else
            {
                desiredColor = baseColor;
            }
        }

        private void Update()
        {
            //Lerps color to smoothen transitions
            if (deckBackground.color != desiredColor)
            {
                float lerpR = Mathf.Lerp(deckBackground.color.r, desiredColor.r, highlightSpeed);
                float lerpG = Mathf.Lerp(deckBackground.color.g, desiredColor.g, highlightSpeed);
                float lerpB = Mathf.Lerp(deckBackground.color.b, desiredColor.b, highlightSpeed);
                float lerpA = Mathf.Lerp(deckBackground.color.a, desiredColor.a, highlightSpeed);

                deckBackground.color = new Color(lerpR, lerpG, lerpB, lerpA);
            }
        }

        public void CheckOverlay()
        {
            //Player Stun Check
            if (player.stun)
            {
                //Debug.Log("Target stunned, apply overlay");
                SetOverlay(true, "Cannot Target - Player Stunned");
            }
            else if (player.banish && player != character)
            {
                //Debug.Log("Target stunned, apply overlay");
                SetOverlay(true, "Cannot Target - Player Banished");
            }
            else if (character.banish && player != character)
            {
                //Debug.Log("Target stunned, apply overlay");
                SetOverlay(true, "Cannot Target - Target Banished");
            }
            else if (character.GetHealth().GetHealth() < 1)
            {
                //Debug.Log("Target killed, apply overlay");
                SetOverlay(true, "Cannot Target - Target Killed");
            }
            else
            {
                //Debug.Log("Target ok, remove overlay");
                SetOverlay(false, " ");
            }
        }

        void SetOverlay(bool active, string message)
        {
            open = !active;
            if (untargettableOverlay != null)
                untargettableOverlay.SetOverlay(active, message);
        }

        #endregion
    }
}