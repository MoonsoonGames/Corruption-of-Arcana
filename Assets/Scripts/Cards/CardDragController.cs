using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDragController : MonoBehaviour
{
    #region Singleton
    //Code from last year

    public static CardDragController instance = null;

    void Singleton()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    Vector3 mousePos;
    Camera cam;

    DraggableCard currentCard;
    DraggableCard heldCard;

    // Start is called before the first frame update
    void Start()
    {
        Singleton();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        #region Highlighting Card

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Card"))
            {
                Debug.DrawLine(ray.origin, ray.origin + hit.point);
                mousePos = hit.point;

                if (heldCard == null)
                {
                    //Highlight Cards
                    DraggableCard hitHighlight = hit.collider.gameObject.GetComponent<DraggableCard>();

                    if (hitHighlight != null)
                    {
                        if (currentCard != null)
                        {
                            currentCard.HighlightCard(false);
                        }

                        currentCard = hitHighlight;
                        currentCard.HighlightCard(true);
                    }
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
            {
                Debug.DrawLine(ray.origin, ray.origin + hit.point);
                mousePos = hit.point;

                if (currentCard != null)
                {
                    currentCard.HighlightCard(false);
                }

                currentCard = null;
            }
        }

        #endregion

        if (Input.GetButtonDown("Select"))
        {
            if (currentCard != null)
            {
                heldCard = currentCard;
                currentCard.Selected();
            }
        }

        if (Input.GetButtonUp("Select"))
        {
            if (heldCard != null)
            {
                heldCard.Dropped();
                heldCard = null;
            }
        }

        if (heldCard != null)
        {
            heldCard.MouseMovement(mousePos);
        }
    }
}
