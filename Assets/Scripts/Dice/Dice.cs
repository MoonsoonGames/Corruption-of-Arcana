using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour
{
    public Vector2 diceValues; //Minimum and maximum values for the dice

    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public int RollDice()
    {
        int value = (int)Random.Range(diceValues.x, diceValues.y);

        text.text = value.ToString();

        return value;
    }
}
