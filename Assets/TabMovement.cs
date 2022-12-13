using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabMovement : MonoBehaviour
{
    Vector3 originalPos = new Vector3();

    public void OnMouseEnter()
    {
        //Debug.Log("Hovering Over");
        originalPos = transform.position;
        Vector3 newPos = new Vector3();
        newPos.x = originalPos.x + 50;
        newPos.y = originalPos.y;
        newPos.z = originalPos.z;
        transform.position = newPos;
    }

    public void OnMouseExit()
    {
        //Debug.Log("Moved Off");
        Vector3 newPos = new Vector3();
        newPos.x = originalPos.x;
        newPos.y = originalPos.y;
        newPos.z = originalPos.z;
        transform.position = newPos;
    }
}
