using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Setup

    #region Variables

    //[HideInInspector]
    public Deck2D deck;
    //[HideInInspector]
    public Deck2D lastDeck;
    //[HideInInspector]
    public Deck2D newDeck;

    DragManager dragManager;

    Vector2 offset;

    #region Scale Values
    public float hoverScale = 1.2f;
    public float pickupScale = 2;
    public float scaleSpeed = 0.1f;
    Vector3 baseScale;
    Vector3 desiredScale;
    #endregion

    #region Highlight Values
    public Image cardBackground;
    Color baseColor;
    public Color highlightColor;
    public float highlightSpeed = 0.2f;
    Color desiredColor;
    #endregion

    #endregion

    private void Start()
    {
        dragManager = DragManager.instance;

        //Sets base scales and colours
        baseScale = transform.localScale;
        desiredScale = baseScale;
        baseColor = cardBackground.color;
        desiredColor = baseColor;
    }

    #endregion

    #region Pointer Events

    /// <summary>
    /// Called when mouse hovers over card
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging == false)
        {
            Debug.Log("Pointer Enter");
            ScaleCard(hoverScale);
            Highlight(true);
        }
    }

    /// <summary>
    /// Called when mouse stops hovering over card
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging == false)
        {
            Debug.Log("Pointer Exit");
            ScaleCard(1);
            Highlight(false);
        }
    }

    /// <summary>
    /// Called when starting to drag card
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");

        Highlight(false);
        ScaleCard(pickupScale);

        //Drags from where the player clicks instead of snapping center of card to the mouse
        offset = new Vector2(transform.position.x - eventData.position.x, transform.position.y - eventData.position.y);

        lastDeck = deck;
        deck.RemoveCard(this);

        dragManager.draggedCard = this;
        SetRayCastTargetAll(false);
    }

    /// <summary>
    /// Called when card is being dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        //TODO: Could lerp?
        transform.position = eventData.position + offset;
    }

    /// <summary>
    /// Called when card stops being dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
        ScaleCard(hoverScale);
        Highlight(true);

        if (newDeck == null)
        {
            lastDeck.AddCard(this);
        }
        else
        {
            newDeck.AddCard(this);
        }

        SetRayCastTargetAll(true);
    }

    void SetRayCastTargetAll(bool targettable)
    {
        CardDrag2D[] cards = GameObject.FindObjectsOfType<CardDrag2D>();

        foreach(CardDrag2D card in cards)
        {
            card.cardBackground.raycastTarget = targettable;
            card.cardBackground.maskable = targettable;
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
            desiredColor = baseColor * highlightColor;
        }
        else
        {
            desiredColor = baseColor;
        }
    }

    /// <summary>
    /// Determines the scale multiplier for the card
    /// </summary>
    /// <param name="scaleFactor">Multiplies the base scale by the scale factor</param>
    void ScaleCard(float scaleFactor)
    {
        //Desired scale is set so that the scale change can be smoothed in update
        desiredScale = baseScale * scaleFactor;
    }

    private void Update()
    {
        //Lerps scale and color to smoothen transitions

        if (transform.localScale != desiredScale)
        {
            float lerpX = Mathf.Lerp(transform.localScale.x, desiredScale.x, scaleSpeed);
            float lerpY = Mathf.Lerp(transform.localScale.y, desiredScale.y, scaleSpeed);
            float lerpZ = Mathf.Lerp(transform.localScale.z, desiredScale.z, scaleSpeed);

            transform.localScale = new Vector3(lerpX, lerpY, lerpZ);
        }

        if (cardBackground.color != desiredColor)
        {
            float lerpR = Mathf.Lerp(cardBackground.color.r, desiredColor.r, highlightSpeed);
            float lerpG = Mathf.Lerp(cardBackground.color.g, desiredColor.g, highlightSpeed);
            float lerpB = Mathf.Lerp(cardBackground.color.b, desiredColor.b, highlightSpeed);
            float lerpA = Mathf.Lerp(cardBackground.color.a, desiredColor.a, highlightSpeed);

            cardBackground.color = new Color(lerpR, lerpG, lerpB, lerpA);
        }
    }

    #endregion
}