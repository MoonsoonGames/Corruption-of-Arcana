using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCard : MonoBehaviour
{
    CardDragController controller;
    Rigidbody rb;
    Material cardMaterial;
    bool selected = false;

    public Vector2 lockY;
    public float dropForce = 3;

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

        Vector3 newPos = new Vector3(transform.position.x, transform.position.y - dropForce, transform.position.z);

        Vector3 difference = newPos - transform.position;

        rb.velocity = 10 * difference;

        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(transform.localPosition.y, lockY.x, lockY.y), transform.localPosition.z);
    }

    public void MouseMovement(Vector3 mousePos)
    {
        Vector3 newPos = new Vector3(mousePos.x, mousePos.y + 1, mousePos.z);

        Vector3 difference = newPos - transform.position;

        rb.velocity = 10 * difference;

        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(transform.localPosition.y, lockY.x, lockY.y), transform.localPosition.z);
    }
}
