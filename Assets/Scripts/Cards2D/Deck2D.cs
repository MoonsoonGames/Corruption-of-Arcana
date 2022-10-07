using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Setup

    Character character;
    Timeline timeline;

    HorizontalLayoutGroup layout;

    CardDrag2D[] cards;
    public int maxCards = 3;
    public int CurrentCardsLength()
    {
        return cards.Length;
    }

    public float deckScale = 1;

    GeneralDragArea dragArea;
    DragManager dragManager;

    #region Highlight Values
    Image deckBackground;
    Color baseColor;
    public Color highlightColor;
    public float highlightSpeed = 0.2f;
    Color desiredColor;
    #endregion

    private void Start()
    {
        ResetArrays();

        dragArea = GameObject.FindObjectOfType<GeneralDragArea>();
        dragManager = DragManager.instance;

        deckBackground = GetComponent<Image>();
        baseColor = deckBackground.color;
        desiredColor = baseColor;

        layout = GetComponent<HorizontalLayoutGroup>();

        character = GetComponentInParent<Character>();
        timeline = GameObject.FindObjectOfType<Timeline>();
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
        if (eventData.dragging == true)
        {
            if (cards.Length < maxCards)
            {
                //Debug.Log(cards.Length + " / " + maxCards);
                dragManager.draggedCard.newDeck = this;
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
        if (eventData.dragging == true)
        {
            dragManager.draggedCard.newDeck = null;
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
        card.gameObject.transform.SetParent(dragArea.transform);
        card.deck = null;

        ResetArrays();

        timeline.RemoveCard(card.GetComponent<Card>().spell, character);
    }

    public void RemoveAllCards()
    {
        foreach (CardDrag2D card in cards)
        {
            Destroy(card.gameObject);
        }

        cards = new CardDrag2D[0];
    }

    /// <summary>
    /// Called when player drops a card onto this deck
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(CardDrag2D card)
    {
        card.gameObject.transform.SetParent(transform);
        card.deck = this;

        ResetArrays();

        if (character != null)
        {
            timeline.AddCard(card.GetComponent<Card>().spell, character);
        }
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

            card.gameObject.transform.SetParent(transform);
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

    #endregion
}