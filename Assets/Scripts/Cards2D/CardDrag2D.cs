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
    public class CardDrag2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Setup

        #region Variables

        public Deck2D deck;
        [HideInInspector]
        public Deck2D lastDeck;
        [HideInInspector]
        public Deck2D newDeck;

        public GameObject placeholder = null;

        Card card;

        public bool playerCard = true;

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

        Vector3 baseRot;
        public float rotationScale = 0.1f;
        public float rotateDeadZone = 5;

        public CharacterHealth casterHealth;
        public List<CharacterHealth> targetHealths;

        #endregion

        public void Setup(Card card)
        {
            this.card = card;

            dragManager = DragManager.instance;

            //Sets base scales and colours
            Debug.Log(card.spell.name + " | " + card.name + ": " + transform.localScale);
            baseScale = transform.localScale;
            desiredScale = baseScale;
            baseColor = cardBackground.color;
            desiredColor = baseColor;
            baseRot = transform.rotation.eulerAngles;

            ScaleCard(1, false);

            if (CombatManager.instance != null)
                casterHealth = CombatManager.instance.player.GetHealth();
        }

        #endregion

        #region Pointer Events

        /// <summary>
        /// Called when mouse hovers over card
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (dragManager.canDrag)
            {
                //Debug.Log("Pointer Enter");
                ScaleCard(hoverScale, false);
                Highlight(true);
                ShowArt(false);
                HighlightTarget();
            }
        }

        /// <summary>
        /// Called when mouse stops hovering over card
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (dragManager.canDrag)
            {
                //Debug.Log("Pointer Exit");
                ScaleCard(1, false);
                Highlight(false);
                if (deck != null)
                {
                    ShowArt(deck.showArt);
                }
                else
                {
                    ShowArt(false);
                }
                StopHighlightTarget();
            }
        }

        /// <summary>
        /// Called when starting to drag card
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (dragManager.canDrag && playerCard)
            {
                //Debug.Log("Drag Start");

                Highlight(false);
                ScaleCard(pickupScale, true);
                ShowArt(false);

                //Create a space where this card was
                placeholder = new GameObject();
                placeholder.transform.SetParent(this.transform.parent);
                LayoutElement le = placeholder.AddComponent<LayoutElement>();
                le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
                le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
                le.flexibleHeight = 0;
                le.flexibleWidth = 0;

                placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

                lastDeck = deck;
                deck.RemoveCard(this);

                dragManager.StartDragging(this);
                SetRayCastTargetAll(false);

                dragManager.canDrag = false;
            }
        }

        Vector2 lastPos = new Vector2(0, 0);

        /// <summary>
        /// Called when card is being dragged
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (dragManager.draggedCard != null && playerCard)
            {
                //Debug.Log("Dragging");
                Vector2 newPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(dragManager.canvas.transform as RectTransform, Input.mousePosition, dragManager.canvas.worldCamera, out newPos);

                //Determines the difference in the x movement to tell which direction it is being dragged in
                float dragSpeedX = lastPos.x - newPos.x;

                Vector3 newRot = new Vector3(baseRot.x, baseRot.y, baseRot.z);
                newRot.z = dragSpeedX * rotationScale;

                if (newRot.z > -rotateDeadZone && newRot.z < rotateDeadZone)
                {
                    //Debug.Log("Not rotating");
                    transform.eulerAngles = baseRot;
                }
                else
                {
                    //Debug.Log("Rotate");
                    transform.eulerAngles = newRot;
                }

                transform.position = dragManager.canvas.transform.TransformPoint(newPos);
                transform.position += new Vector3(0, 0, -50);

                lastPos = newPos;

                Deck2D deck = lastDeck;
                if (newDeck != null)
                    deck = newDeck;

                if (deck.useSibIndex)
                {
                    int newSibIndex = deck.transform.childCount;

                    //Move cards out of the way to make room for this one
                    for (int i = 0; i < deck.transform.childCount; i++)
                    {
                        if (this.transform.position.x < deck.transform.GetChild(i).position.x)
                        {
                            newSibIndex = i;

                            if (placeholder.transform.GetSiblingIndex() < newSibIndex)
                            {
                                newSibIndex--;
                            }

                            break;
                        }
                    }

                    placeholder.transform.SetSiblingIndex(newSibIndex);
                }
            }
        }

        /// <summary>
        /// Called when card stops being dragged
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragManager.draggedCard != null && playerCard)
            {
                //Debug.Log("Drag End");

                if (newDeck == null)
                {
                    lastDeck.AddCard(this);
                }
                else
                {
                    newDeck.AddCard(this);
                }

                SetRayCastTargetAll(true);

                ScaleCard(1, false);
                Highlight(false);
                StopHighlightTarget();
                ShowArt(deck.showArt);
                transform.eulerAngles = baseRot;

                dragManager.StopDragging(this, deck.GetCharacter());
            }

            if (placeholder != null)
            {
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                //TODO - If in new deck, ignore sibling index
                Destroy(placeholder);
                placeholder = null;
            }
        }

        void SetRayCastTargetAll(bool targettable)
        {
            CardDrag2D[] cards = GameObject.FindObjectsOfType<CardDrag2D>();

            foreach (CardDrag2D card in cards)
            {
                card.cardBackground.raycastTarget = targettable;
                card.cardBackground.maskable = targettable;
            }
        }

        #endregion

        #region Visual Feedback

        [ColorUsage(true, true)]
        public Color greenColour = Color.green;
        [ColorUsage(true, true)]
        public Color redColour = Color.red;
        [ColorUsage(true, true)]
        public Color yellowColour = Color.yellow;

        public void HighlightTarget()
        {
            if (CombatManager.instance == null) return;

            targetHealths = new List<CharacterHealth>();

            foreach (var item in CombatManager.instance.enemyTeamManager.team)
            {
                targetHealths.Add(item.GetHealth());
            }

            switch (card.spell.idealTarget)
            {
                case E_SpellTargetType.Caster:
                    //Debug.Log("Highlight caster for " + card.spell.spellName);
                    if (casterHealth.dying == false)
                        casterHealth.GetColorFlash().Highlight(greenColour);
                    break;
                case E_SpellTargetType.Target:
                    //Debug.Log("Highlight targets for " + card.spell.spellName);
                    foreach (var item in targetHealths)
                    {
                        if (item.dying == false)
                            item.GetColorFlash().Highlight(redColour);
                    }
                    break;
                case E_SpellTargetType.All:
                    //Debug.Log("Highlight all characters for " + card.spell.spellName);
                    casterHealth.GetColorFlash().Highlight(yellowColour);
                    foreach (var item in targetHealths)
                    {
                        if (item.dying == false)
                            item.GetColorFlash().Highlight(yellowColour);
                    }
                    break;
                default:
                    Debug.LogWarning("Ideal target is not valid for " + card.spell.spellName);
                    break;
            }
        }

        public void StopHighlightTarget()
        {
            if (CombatManager.instance == null) return;

            targetHealths = new List<CharacterHealth>();

            foreach (var item in CombatManager.instance.enemyTeamManager.team)
            {
                targetHealths.Add(item.GetHealth());
            }

            switch (card.spell.idealTarget)
            {
                case E_SpellTargetType.Caster:
                    //Debug.Log("Highlight caster for " + card.spell.spellName);
                    if (casterHealth != null)
                    {
                        if (casterHealth.dying == false)
                            casterHealth.GetColorFlash().RemoveHighlightColour();
                    }
                    break;
                case E_SpellTargetType.Target:
                    //Debug.Log("Highlight targets for " + card.spell.spellName);
                    foreach (var item in targetHealths)
                    {
                        if (item != null)
                        {
                            if (item.dying == false)
                                item.GetColorFlash().RemoveHighlightColour();
                        }
                    }
                    break;
                case E_SpellTargetType.All:
                    //Debug.Log("Highlight all characters for " + card.spell.spellName);
                    casterHealth.GetColorFlash().RemoveHighlightColour();
                    foreach (var item in targetHealths)
                    {
                        if (item != null)
                        {
                            if (item.dying == false)
                                item.GetColorFlash().RemoveHighlightColour();
                        }
                    }
                    break;
                default:
                    Debug.LogWarning("Ideal target is not valid for " + card.spell.spellName);
                    break;
            }
        }

        /// <summary>
        /// Turns the highlight colour on or off
        /// </summary>
        /// <param name="on">True for highlight color, False for base color</param>
        public void Highlight(bool on)
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
        public void ScaleCard(float scaleFactor, bool ignoreDeck)
        {
            //Desired scale is set so that the scale change can be smoothed in update
            if (deck == null || ignoreDeck)
            {
                desiredScale = baseScale * scaleFactor;
            }
            else
            {
                desiredScale = baseScale * scaleFactor * deck.deckScale;
            }
        }

        void ShowArt(bool show)
        {
            card.ShowArt(show);
        }

        private void FixedUpdate()
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
}