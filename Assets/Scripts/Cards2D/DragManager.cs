using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    #region Singleton
    //Code from last year

    public static DragManager instance = null;

    void Singleton()
    {
        if (instance == null)
        {
            instance = this;

            //DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public CardDrag2D draggedCard;

    private void Awake()
    {
        Singleton();
    }
}
