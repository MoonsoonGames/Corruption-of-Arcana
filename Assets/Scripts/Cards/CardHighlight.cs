using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    public Material cardMaterial;

    public void HighlightCard(bool highlight)
    {
        cardMaterial.SetInteger("_Selected", highlight ? 1 : 0);
    }
}
