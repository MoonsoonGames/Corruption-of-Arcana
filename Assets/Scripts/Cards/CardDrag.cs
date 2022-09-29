using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrag : MonoBehaviour
{
    Vector3 mousePos;
    Camera cam;

    CardHighlight currentCard;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Card"))
            {
                Debug.DrawLine(ray.origin, ray.origin + hit.point);
                mousePos = hit.point;

                //Highlight Cards
                CardHighlight hitHighlight = hit.collider.gameObject.GetComponent<CardHighlight>();

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
    }
}
