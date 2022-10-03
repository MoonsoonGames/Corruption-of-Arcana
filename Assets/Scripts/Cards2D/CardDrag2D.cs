using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    Vector2 offset;

    public float hoverScale = 1.2f;
    public float pickupScale = 2;
    Vector3 baseScale;

    public Image cardBackground;
    Color baseColor;
    public Color highlightColor;

    private void Start()
    {
        baseScale = transform.localScale;
        baseColor = cardBackground.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        ScaleCard(hoverScale);
        Highlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        ScaleCard(1);
        Highlight(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");

        Highlight(false);

        //Drags from where the player clicks instead of snapping center of card to the mouse
        offset = new Vector2(transform.position.x - eventData.position.x, transform.position.y - eventData.position.y);

        ScaleCard(pickupScale);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        transform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
        ScaleCard(hoverScale);
        Highlight(true);
    }

    void Highlight(bool on)
    {
        if (on)
        {
            cardBackground.color = baseColor * highlightColor;
        }
        else
        {
            cardBackground.color = baseColor;
        }
    }

    void ScaleCard(float scaleFactor)
    {
        transform.localScale = baseScale * scaleFactor;
    }
}