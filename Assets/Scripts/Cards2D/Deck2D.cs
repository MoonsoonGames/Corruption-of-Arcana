using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CardDrag2D[] cards;
    GeneralDragArea dragArea;
    DragManager dragManager;

    private void Start()
    {
        cards = GetComponentsInChildren<CardDrag2D>();

        foreach (CardDrag2D card in cards)
        {
            card.deck = this;

            card.gameObject.transform.SetParent(transform);
        }

        dragArea = GameObject.FindObjectOfType<GeneralDragArea>();
        dragManager = DragManager.instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
        if (eventData.dragging == true)
        {
            Debug.Log(dragManager.name);
            Debug.Log(dragManager.draggedCard.name);
            Debug.Log("Pointer enter, dragging");
            dragManager.draggedCard.newDeck = this;
        }
        else
        {
            Debug.Log("Pointer enter, not dragging");
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
    }

    public void AddCard(CardDrag2D card)
    {
        card.gameObject.transform.SetParent(transform);
        card.deck = this;
    }
}
