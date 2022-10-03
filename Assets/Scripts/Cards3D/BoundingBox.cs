using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    public Vector2 lockY;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Card"))
        {
            DraggableCard card = other.gameObject.GetComponent<DraggableCard>();

            if (card != null)
            {
                EnterBounds(card);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Card"))
        {
            DraggableCard card = other.gameObject.GetComponent<DraggableCard>();

            if (card != null)
            {
                ExitBounds(card);
            }
        }
    }

    public virtual void EnterBounds(DraggableCard card)
    {
        Debug.Log("Enter bounds");
        card.lockY = lockY;
    }

    public virtual void ExitBounds(DraggableCard card)
    {
        Debug.Log("Exit bounds");
        card.ResetHeight();
    }
}
