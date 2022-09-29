using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    Material cardMaterial;

    private void Start()
    {
        cardMaterial = GetComponent<Renderer>().material;
    }

    public void HighlightCard(bool highlight)
    {
        cardMaterial.SetFloat("_Selected", highlight ? 1 : 0);
    }
}
