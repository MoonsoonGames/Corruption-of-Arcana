using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabMovement : MonoBehaviour
{

    public void OnMouseEnter()
    {
        Debug.Log("Hovering Over");
        transform.position = new Vector3(655f, 0, 0);
    }

    public void OnMouseExit()
    {
        Debug.Log("Moved Off");
        transform.position = new Vector3(585f, 0, 0);
    }
}
