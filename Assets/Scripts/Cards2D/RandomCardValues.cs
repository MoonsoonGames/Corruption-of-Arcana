using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomCardValues : MonoBehaviour
{
    public string[] cardNumber;
    public string[] cardSuit;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        string cardName = (cardNumber[Random.Range(0, cardNumber.Length)] + " of " + cardSuit[Random.Range(0, cardSuit.Length)]);
        text.text = cardName;

        gameObject.name = cardName;
    }
}
