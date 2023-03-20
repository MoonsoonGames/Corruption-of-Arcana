using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabMovement : MonoBehaviour
{
    Vector3 originalPos = new Vector3();
    public float tabOutAmount = 6f;

    private void Start()
    {
        originalPos = transform.position;
    }

    public void OnMouseEnter()
    {
        Vector3 newPos = new Vector3(originalPos.x - tabOutAmount, originalPos.y, originalPos.z);
        transform.position = newPos;
    }

    public void OnMouseExit()
    {
        transform.position = originalPos;
    }
}
