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
    public float scaleSpeed = 0.1f;
    Vector3 baseScale;
    Vector3 desiredScale;

    public Image cardBackground;
    Color baseColor;
    public Color highlightColor;
    Color desiredColor;

    private void Start()
    {
        baseScale = transform.localScale;
        desiredScale = baseScale;
        baseColor = cardBackground.color;
        desiredColor = baseColor;
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
            desiredColor = baseColor * highlightColor;
        }
        else
        {
            desiredColor = baseColor;
        }
    }

    void ScaleCard(float scaleFactor)
    {
        desiredScale = baseScale * scaleFactor;
    }

    private void Update()
    {
        //Lerps scale and color

        if (transform.localScale != desiredScale)
        {
            float lerpX = Mathf.Lerp(transform.localScale.x, desiredScale.x, scaleSpeed);
            float lerpY = Mathf.Lerp(transform.localScale.y, desiredScale.y, scaleSpeed);
            float lerpZ = Mathf.Lerp(transform.localScale.z, desiredScale.z, scaleSpeed);

            transform.localScale = new Vector3(lerpX, lerpY, lerpZ);
        }

        if (cardBackground.color != desiredColor)
        {
            float lerpR = Mathf.Lerp(cardBackground.color.r, desiredColor.r, scaleSpeed);
            float lerpG = Mathf.Lerp(cardBackground.color.g, desiredColor.g, scaleSpeed);
            float lerpB = Mathf.Lerp(cardBackground.color.b, desiredColor.b, scaleSpeed);
            float lerpA = Mathf.Lerp(cardBackground.color.a, desiredColor.a, scaleSpeed);

            cardBackground.color = new Color(lerpR, lerpG, lerpB, lerpA);
        }
    }
}