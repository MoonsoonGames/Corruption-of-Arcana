using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 offset;

    public float pickupScale = 2;
    Vector3 dropScale;

    private void Start()
    {
        dropScale = transform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");

        //Drags from where the player clicks instead of snapping center of card to the mouse
        offset = new Vector2(transform.position.x - eventData.position.x, transform.position.y - eventData.position.y);

        transform.localScale = dropScale * pickupScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        transform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
        transform.localScale = dropScale;
    }
}