using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCard : MonoBehaviour
{
    CardDragController controller;
    Rigidbody rb;
    Material cardMaterial;
    bool selected = false;

    private void Start()
    {
        controller = CardDragController.instance;
        rb = GetComponent<Rigidbody>();
        cardMaterial = GetComponent<Renderer>().material;
    }

    public void HighlightCard(bool highlight)
    {
        if (!selected)
            cardMaterial.SetFloat("_Selected", highlight ? 1 : 0);
        else
            cardMaterial.SetFloat("_Selected", 0);

    }

    public void Selected()
    {
        selected = true;
        Debug.Log(name + " selected");
        HighlightCard(false);
    }

    public void Dropped()
    {
        selected = false;
        Debug.Log(name + " dropped");
    }

    public void MouseMovement(Vector3 mousePos)
    {
        Vector3 newPos = new Vector3(mousePos.x, mousePos.y + 1, mousePos.z);

        Vector3 difference = newPos - transform.position;

        rb.velocity = 10 * difference;
    }
}
