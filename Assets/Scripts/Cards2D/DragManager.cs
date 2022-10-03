using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager instance;

    public CardDrag2D draggedCard;

    private void Start()
    {
        instance = this;
    }
}
