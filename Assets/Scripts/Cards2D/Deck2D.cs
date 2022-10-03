using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CardDrag2D[] cards;
    public int maxCards = 3;
    GeneralDragArea dragArea;
    DragManager dragManager;

    private void Start()
    {
        ResetArrays();

        dragArea = GameObject.FindObjectOfType<GeneralDragArea>();
        dragManager = DragManager.instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging == true)
        {
            if (cards.Length < maxCards)
            {
                Debug.Log(cards.Length + " / " + maxCards);
                dragManager.draggedCard.newDeck = this;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging == true)
        {
            dragManager.draggedCard.newDeck = null;
        }
    }

    public void RemoveCard(CardDrag2D card)
    {
        card.gameObject.transform.SetParent(dragArea.transform);
        card.deck = null;

        ResetArrays();
    }

    public void AddCard(CardDrag2D card)
    {
        card.gameObject.transform.SetParent(transform);
        card.deck = this;

        ResetArrays();
    }

    void ResetArrays()
    {
        cards = GetComponentsInChildren<CardDrag2D>();

        foreach (CardDrag2D card in cards)
        {
            card.deck = this;

            card.gameObject.transform.SetParent(transform);
        }
    }

    public int CurrentCardsLength()
    {
        return cards.Length;
    }
}
