using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinorArcana : MonoBehaviour
{
    [HideInInspector]
    public string cardName;
    public Suit cardSuit;
    public int cardNumber;

    [TextArea(3, 10)]
    public string cardDescription;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void Setup()
    {
        cardName = RoyalCards(cardNumber) + " of " + cardSuit.suitName;
        cardDescription = cardSuit.suitDescription;
        cardDescription = cardDescription.Replace("$value$", cardNumber.ToString());

        nameText.text = cardName;
        descriptionText.text = cardDescription;

        gameObject.name = cardName;
    }

    string RoyalCards(int number)
    {
        switch (number)
        {
            case 11:
                return "Knight";
            case 12:
                return "Queen";
            case 13:
                return "King";
            default:
                return number.ToString();
        }
    }

    public void CastSpell(Character target)
    {
        cardSuit.CastSpell(target, cardNumber);
    }
}
