using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCardValues : MonoBehaviour
{
    public int[] cardNumber;
    public Suit[] cardSuit;

    // Start is called before the first frame update
    void Start()
    {
        MinorArcana minorArcana = GetComponent<MinorArcana>();

        minorArcana.cardSuit = cardSuit[Random.Range(0, cardSuit.Length)];
        minorArcana.cardNumber = cardNumber[Random.Range(0, cardNumber.Length)];
        minorArcana.Setup();
    }
}
