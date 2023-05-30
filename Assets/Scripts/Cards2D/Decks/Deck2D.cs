using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;

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

        protected Character player;
        protected Character character; public Character GetCharacter() { return character; }
        protected Timeline timeline;
        protected DeckManager manager;

        public GameObject group;
        protected LayoutGroup layout;

        public Object cardSpace;
        public int minCardCount;
        List<GameObject> currentCardSpaces = new List<GameObject>();

        protected GeneralDragArea dragArea;
        protected DragManager dragManager;

        protected bool open = true;
        protected UntargettableOverlay untargettableOverlay;

        public bool showArt = false;
        public bool useSibIndex = true;

        #endregion

        #region Cards

        protected CardDrag2D[] cards = new CardDrag2D[0];

        public List<GameObject> GetCards()
        {
            List<GameObject> cardObjects = new List<GameObject>();

            if (cards == null)
                return cardObjects;

            foreach (var item in cards)
            {
                cardObjects.Add(item.gameObject);
            }
            //Debug.Log(cards.Length + " cards in hand || " + cardObjects.Count + " objects in list");
            return cardObjects;
        }

        public List<E_CardTypes> availableCards;
        public int maxCards = 3;
        public int CurrentCardsLength()
        {
            if (cards == null) return 0;
            return cards.Length;
        }

        public List<Spell> GetSpells()
        {
            List<Spell> spells = new List<Spell>();

            if (cards.Length <= 0) return spells;

            foreach (CardDrag2D card in cards)
            {
                Card cardInstance = card.GetComponent<Card>();

                Spell spellInstance = cardInstance.spell;

                spells.Add(spellInstance);
            }

            return spells;
        }

        public float deckScale = 1;

        #endregion

        #region Highlight Values
        protected Image deckBackground;
        protected Color baseColor;
        public Color highlightColor;
        public float highlightSpeed = 0.2f;
        protected Color desiredColor;
        #endregion

        protected virtual void Start()
        {
            //Sets up base values
            ResetArrays();

            dragArea = GameObject.FindObjectOfType<GeneralDragArea>();
            dragManager = DragManager.instance;

            deckBackground = group.GetComponent<Image>();
            baseColor = deckBackground.color;
            desiredColor = baseColor;

            layout = GetComponent<LayoutGroup>();

            untargettableOverlay = transform.parent.GetComponentInChildren<UntargettableOverlay>();
            SetOverlay(false, " ");

            useSibIndex = layout != null;

            CheckCardSpace();
        }

        private void OnEnable()
        {
            CheckCardSpace();
        }

        #endregion

        #region Pointer Events

        /// <summary>
        /// Called when mouse hovers over deck
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            //Only fires logic when player is dragging a card into the deck
            if (eventData.dragging == true && dragManager.draggedCard != null && open)
            {
                if (availableCards.Contains(dragManager.draggedCard.GetComponent<Card>().spell.cardType) == false)
                    return;

                if (cards.Length < maxCards)
                {
                    //Debug.Log(cards.Length + " / " + maxCards);
                    CardDrag2D currentCard = dragManager.draggedCard;
                    currentCard.newDeck = this;
                    currentCard.ScaleCard(currentCard.hoverScale, true);
                    currentCard.placeholder.transform.SetParent(this.transform);
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
            if (eventData.dragging == true && dragManager.draggedCard != null && open)
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
        public virtual void RemoveCard(CardDrag2D card)
        {
            card.gameObject.transform.SetParent(dragArea.transform);
            card.deck = null;

            ResetArrays();
        }

        /// <summary>
        /// Removes all cards from the deck without taking them from the timeline
        /// </summary>
        public virtual void RemoveAllCards(bool discard)
        {
            //Overrided in most children, this is the base behaviour
            ResetArrays();

            cards = new CardDrag2D[0];
            if (timeline != null)
                timeline.SimulateSpellEffects();
        }

        /// <summary>
        /// Called when player drops a card onto this deck
        /// </summary>
        /// <param name="card"></param>
        public virtual void AddCard(CardDrag2D card)
        {
            card.gameObject.transform.SetParent(group.transform);
            card.gameObject.transform.position = gameObject.transform.position;
            card.deck = this;
            card.GetComponent<Card>().ShowArt(showArt);

            ResetArrays();

            PlayCardSound();
        }

        /// <summary>
        /// Needs to be called whenever a card is removed or added to this deck
        /// Clears the array and resets it to what is currently in the deck
        /// </summary>
        protected virtual void ResetArrays()
        {
            Highlight(false);

            cards = GetComponentsInChildren<CardDrag2D>();

            foreach (CardDrag2D card in cards)
            {
                card.deck = this;

                card.gameObject.transform.SetParent(group.transform);
            }

            CheckCardSpace();
        }

        protected void CheckCardSpace()
        {
            if (cardSpace == null)
                return;

            foreach(var item in currentCardSpaces)
            {
                Destroy(item.gameObject);
            }

            currentCardSpaces.Clear();

            int cardDiff = minCardCount - cards.Length;

            for(int i = 0; i < cardDiff; i++)
            {
                currentCardSpaces.Add(Instantiate(cardSpace, transform) as GameObject);
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
            else if (player.banish)
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

        #region Sound Effects

        [Header("Sound Effects")]
        public EventReference cardPlacedSound;

        public void PlayCardSound()
        {
            RuntimeManager.PlayOneShot(cardPlacedSound);
        }

        #endregion
    }

    public enum E_CardTypes
    {
        All, Cards, Potions, Bombs, Weapons, Trinkets, None
    }
}