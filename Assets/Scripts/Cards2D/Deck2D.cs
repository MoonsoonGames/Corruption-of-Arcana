using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Setup

    CardDrag2D[] cards;
    public int CurrentCardsLength()
    {
        return cards.Length;
    }

    public int maxCards = 3;
    GeneralDragArea dragArea;
    DragManager dragManager;

    private void Start()
    {
        ResetArrays();

        dragArea = GameObject.FindObjectOfType<GeneralDragArea>();
        dragManager = DragManager.instance;
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
                Debug.Log(cards.Length + " / " + maxCards);
                dragManager.draggedCard.newDeck = this;
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
    }

    /// <summary>
    /// Needs to be called whenever a card is removed or added to this deck
    /// Clears the array and resets it to what is currently in the deck
    /// </summary>
    void ResetArrays()
    {
        cards = GetComponentsInChildren<CardDrag2D>();

        foreach (CardDrag2D card in cards)
        {
            card.deck = this;

            card.gameObject.transform.SetParent(transform);
        }
    }

    #endregion
}
